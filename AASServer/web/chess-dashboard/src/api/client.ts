import { mockCapabilities, mockStatusProfiles } from "../data/mockData";
import { mergeSchedulesByAsset } from "../lib/schedule";
import type {
  ChessCapability,
  ChessStatusProfile,
  ChessStatusWindow,
  ConnectionSettings,
  DayAheadPeriod,
  DayAheadRequest,
  DayAheadResult,
  KpiSeries,
  OptimiserRequest,
  OptimiserResponse,
} from "../types";

// Talks to the routes CHESSNetworkController.cs exposes:
//   GET  /chess/1.0/capability/{id}
//   GET  /chess/1.0/status
//   GET  /chess/1.0/status/{id}
//   POST /chess/1.0/status/{id}
//   POST /opt/1.0.0/run
//   POST /run/dayahead
// In "demo" mode (the default - no backend required) every call instead reads
// from / mutates an in-memory copy of src/data/mockData.ts, and runOptimiser /
// runDayAhead reproduce the aggregation CHESSNetworkController.runPost /
// runDayAheadPost perform server-side, so the UI behaves the same either way.

class ApiError extends Error {}

let demoCapabilities: ChessCapability[] = mockCapabilities.map((c) => ({ ...c }));
let demoStatusProfiles: ChessStatusProfile[] = mockStatusProfiles.map((p) => ({
  ...p,
  status: p.status.map((w) => ({ ...w })),
}));

function authHeaders(settings: ConnectionSettings): HeadersInit {
  const headers: HeadersInit = { "Content-Type": "application/json" };
  if (settings.token) headers["Authorization"] = settings.token;
  return headers;
}

async function liveFetch<T>(settings: ConnectionSettings, path: string, init?: RequestInit): Promise<T> {
  const url = settings.baseUrl.replace(/\/$/, "") + path;
  const res = await fetch(url, { ...init, headers: { ...authHeaders(settings), ...(init?.headers ?? {}) } });
  if (!res.ok) {
    throw new ApiError(`${init?.method ?? "GET"} ${path} failed: ${res.status} ${res.statusText}`);
  }
  return (await res.json()) as T;
}

// CapabilityCHESSGet (CHESSNetworkController.cs) adds "submodels" (and,
// optionally, "status") to the digital twin's `Contents` dictionary -
// Azure.DigitalTwins.Core.BasicDigitalTwin.Contents is marked with
// System.Text.Json's [JsonExtensionData], which System.Text.Json flattens
// onto the parent object, but Newtonsoft (what Startup.cs actually
// configures for MVC) doesn't recognise that attribute and serializes
// Contents as an ordinary nested `contents` object instead. So on the wire
// it's `twin.contents.submodels`, not a top-level `twin.submodels`.
interface RawChessCapability extends ChessCapability {
  contents?: { submodels?: string };
}

function normalizeCapability(raw: RawChessCapability): ChessCapability {
  const { contents, ...capability } = raw;
  return { ...capability, submodels: capability.submodels ?? contents?.submodels };
}

export async function fetchCapabilities(settings: ConnectionSettings, knownIds: string[]): Promise<ChessCapability[]> {
  if (settings.mode === "demo") return demoCapabilities;

  // The real API has no "list all" capability route - each asset id must be
  // requested individually (see Settings: "Known CHESS ids").
  const results = await Promise.all(
    knownIds.map((id) => liveFetch<RawChessCapability[]>(settings, `/chess/1.0/capability/${encodeURIComponent(id)}`))
  );
  return results.flat().map(normalizeCapability);
}

export async function fetchAllStatusProfiles(settings: ConnectionSettings): Promise<ChessStatusProfile[]> {
  if (settings.mode === "demo") return demoStatusProfiles;
  return liveFetch<ChessStatusProfile[]>(settings, "/chess/1.0/status");
}

export async function saveStatusProfile(
  settings: ConnectionSettings,
  id: string,
  windows: ChessStatusWindow[]
): Promise<ChessStatusWindow[]> {
  if (settings.mode === "demo") {
    demoStatusProfiles = demoStatusProfiles.map((p) => (p.id === id ? { ...p, status: windows } : p));
    return windows;
  }
  const body = { status: windows };
  return liveFetch<ChessStatusWindow[]>(settings, `/chess/1.0/status/${encodeURIComponent(id)}`, {
    method: "POST",
    body: JSON.stringify(body),
  });
}

// Reproduces CHESSNetworkController.runPost's KPI aggregation for demo mode:
// for every window with priority < 10, bucket capacity/cost/carbon by priority
// level, split into "in" (charging) vs "out" (discharging).
function simulateOptimiser(request: OptimiserRequest): OptimiserResponse {
  return request.Options.map((option) => {
    const costForLevel = Array.from({ length: 10 }, () => [0, 0]); // [out, in]
    const capacityForLevel = Array.from({ length: 10 }, () => [0, 0]);
    const carbonForLevel = Array.from({ length: 10 }, () => [0, 0]);
    let costUnit = "";
    let carbonUnit = "";

    for (const w of option.status) {
      if (w.priority >= 10) continue;
      const factor = w.cycleCostUnit?.toLowerCase().includes("k") === false ? 1 : 1000;
      if (w.cycleCarbonUnit) {
        carbonUnit = w.cycleCarbonUnit[0];
        costUnit = w.cycleCostUnit[0];
      }
      const col = w.status.toLowerCase().includes("discharge") ? 0 : 1;
      capacityForLevel[w.priority][col] += w.capacityEnd;
      costForLevel[w.priority][col] += (w.cycleCost * w.capacityEnd) / factor;
      carbonForLevel[w.priority][col] += (w.cycleCarbon * w.capacityEnd) / factor;
    }

    const series = (
      name: KpiSeries["name"],
      unit: string,
      col: 0 | 1,
      table: number[][]
    ): KpiSeries => ({ name, unit, value: table.map((row) => row[col]) });

    const KPI: KpiSeries[] = [
      series("costin", costUnit, 1, costForLevel),
      series("carbonin", carbonUnit, 1, carbonForLevel),
      series("capacityin", "Wh", 1, capacityForLevel),
      series("costout", costUnit, 0, costForLevel),
      series("carbonout", carbonUnit, 0, carbonForLevel),
      series("capacityout", "Wh", 0, capacityForLevel),
    ];

    const chess = demoStatusProfiles.filter((p) => p.id === option.option);

    return { Objective: option.objective, Option: option.option, KPI, CHESS: chess };
  });
}

export async function runOptimiser(settings: ConnectionSettings, request: OptimiserRequest): Promise<OptimiserResponse> {
  if (settings.mode === "demo") return simulateOptimiser(request);
  return liveFetch<OptimiserResponse>(settings, "/opt/1.0.0/run", {
    method: "POST",
    body: JSON.stringify(request.Options.map((o) => ({ Options: [o], Limits: request.Limits }))),
  });
}

// --- Day-ahead (POST /run/dayahead) -----------------------------------------
// Reproduces CHESSNetworkController.runDayAheadPost + AllocateDayAhead +
// BuildSchedules for demo mode: flatten every asset's non-Idle windows into
// per-Wh-priced discharge/charge resources (cheapest first), greedily shave
// every period where forecast demand exceeds the limit using the cheapest
// discharge capacity, then replenish the energy used during the
// cheapest-tariff periods that still have headroom under the limit.

interface DemoFlexResource {
  chessId: string;
  entry: ChessStatusWindow;
  remaining: number;
  used: number;
  costPerWh: number;
  allocations: { period: number; wh: number }[];
}

function costPerWh(entry: ChessStatusWindow): number {
  const factor = entry.cycleCostUnit && !entry.cycleCostUnit.toLowerCase().includes("k") ? 1 : 1000;
  return entry.cycleCost / factor;
}

function buildDemoResources(profiles: ChessStatusProfile[]): { discharge: DemoFlexResource[]; charge: DemoFlexResource[] } {
  const discharge: DemoFlexResource[] = [];
  const charge: DemoFlexResource[] = [];
  for (const profile of profiles) {
    for (const entry of profile.status) {
      if (entry.priority >= 10 || entry.capacityEnd <= 0) continue;
      const resource: DemoFlexResource = {
        chessId: profile.id,
        entry,
        remaining: entry.capacityEnd,
        used: 0,
        costPerWh: costPerWh(entry),
        allocations: [],
      };
      (entry.status.toLowerCase().includes("discharge") ? discharge : charge).push(resource);
    }
  }
  discharge.sort((a, b) => a.costPerWh - b.costPerWh);
  charge.sort((a, b) => a.costPerWh - b.costPerWh);
  return { discharge, charge };
}

function allocateDayAhead(
  demand: number[],
  tariff: number[],
  periodHours: number,
  limit: number,
  dischargeResources: DemoFlexResource[],
  chargeResources: DemoFlexResource[]
) {
  const periods = demand.length;
  const gridImport = new Array<number>(periods).fill(0);
  const unserved = new Array<number>(periods).fill(0);
  const chargedThisPeriod = new Array<number>(periods).fill(0);
  let totalCost = 0;
  let baselineCost = 0;

  for (let t = 0; t < periods; t++) {
    const baselineWh = demand[t] * periodHours;
    baselineCost += (tariff[t] / 1000) * baselineWh;

    let deficitWh = Math.max(0, demand[t] - limit) * periodHours;
    let servedWh = 0;
    for (const r of dischargeResources) {
      if (deficitWh <= 0) break;
      if (r.remaining <= 0) continue;
      const use = Math.min(deficitWh, r.remaining);
      r.remaining -= use;
      r.used += use;
      r.allocations.push({ period: t, wh: use });
      deficitWh -= use;
      servedWh += use;
      totalCost += use * r.costPerWh;
    }

    const importWh = baselineWh - servedWh;
    gridImport[t] = importWh;
    unserved[t] = Math.max(0, importWh - limit * periodHours);
    totalCost += (tariff[t] / 1000) * importWh;
  }

  let toReplenish = dischargeResources.reduce((sum, r) => sum + r.used, 0);
  const cheapestPeriodsFirst = Array.from({ length: periods }, (_, t) => t)
    .filter((t) => demand[t] < limit)
    .sort((a, b) => tariff[a] - tariff[b]);

  for (const t of cheapestPeriodsFirst) {
    if (toReplenish <= 0) break;
    let headroomWh = (limit - demand[t]) * periodHours - chargedThisPeriod[t];
    for (const r of chargeResources) {
      if (toReplenish <= 0 || headroomWh <= 0) break;
      if (r.remaining <= 0) continue;
      const use = Math.min(headroomWh, r.remaining, toReplenish);
      r.remaining -= use;
      r.used += use;
      r.allocations.push({ period: t, wh: use });
      headroomWh -= use;
      toReplenish -= use;
      chargedThisPeriod[t] += use;
      gridImport[t] += use;
      totalCost += use * r.costPerWh;
      totalCost += (tariff[t] / 1000) * use;
    }
  }

  return { gridImport, unserved, totalCost, baselineCost, toReplenish };
}

function formatHourString(hour: number): string {
  if (hour >= 24) return "23:59";
  let hh = Math.floor(hour);
  let mm = Math.round((hour - hh) * 60);
  if (mm === 60) {
    mm = 0;
    hh += 1;
  }
  return `${String(hh).padStart(2, "0")}:${String(mm).padStart(2, "0")}`;
}

function buildWindow(
  template: ChessStatusWindow,
  startPeriod: number,
  endPeriod: number,
  wh: number,
  periodHours: number,
  recurrence: string
): ChessStatusWindow {
  const startHour = startPeriod * periodHours;
  const endHour = Math.min(24, (endPeriod + 1) * periodHours);
  return {
    status: template.status,
    service: template.service,
    starttime: formatHourString(startHour),
    endtime: formatHourString(endHour),
    capacity: String(wh),
    recurrence,
    priority: template.priority,
    efficiency: template.efficiency,
    probability: template.probability,
    capacityStart: 0,
    capacityEnd: wh,
    capacityMax: template.capacityMax,
    cycleCost: template.cycleCost,
    cycleCarbon: template.cycleCarbon,
    cycleCostUnit: template.cycleCostUnit,
    cycleCarbonUnit: template.cycleCarbonUnit,
  };
}

function buildDemoSchedules(resources: DemoFlexResource[], periodHours: number, recurrence: string): ChessStatusProfile[] {
  const schedules: ChessStatusProfile[] = [];
  for (const r of resources) {
    if (r.used <= 0) continue;
    r.allocations.sort((a, b) => a.period - b.period);

    const windows: ChessStatusWindow[] = [];
    let runStart = r.allocations[0].period;
    let runEnd = runStart;
    let runWh = r.allocations[0].wh;
    for (let i = 1; i < r.allocations.length; i++) {
      const a = r.allocations[i];
      if (a.period === runEnd + 1) {
        runEnd = a.period;
        runWh += a.wh;
        continue;
      }
      windows.push(buildWindow(r.entry, runStart, runEnd, runWh, periodHours, recurrence));
      runStart = a.period;
      runEnd = a.period;
      runWh = a.wh;
    }
    windows.push(buildWindow(r.entry, runStart, runEnd, runWh, periodHours, recurrence));

    schedules.push({ identifier: r.chessId, location: "", id: r.chessId, status: windows });
  }
  return schedules;
}

function simulateDayAhead(request: DayAheadRequest): DayAheadResult {
  const periods = request.Demand.length;
  const periodHours = request.PeriodHours > 0 ? request.PeriodHours : 1;
  const limit = request.Limits.find((l) => l.Name?.toLowerCase() === "maxpower")?.Value ?? 0;
  const recurrence = request.Recurrence || "daily";
  const objective = request.Options?.[0]?.objective || "mincost";

  const { discharge, charge } = buildDemoResources(demoStatusProfiles);
  const alloc = allocateDayAhead(request.Demand, request.Tariff, periodHours, limit, discharge, charge);
  const schedules = buildDemoSchedules([...discharge, ...charge], periodHours, recurrence);

  if (request.Dispatch) {
    const merged = mergeSchedulesByAsset(schedules);
    for (const m of merged) {
      demoStatusProfiles = demoStatusProfiles.map((p) => (p.id === m.id ? { ...p, status: m.status } : p));
    }
  }

  const periodsOut: DayAheadPeriod[] = Array.from({ length: periods }, (_, t) => ({
    period: t,
    demand: request.Demand[t],
    gridImport: alloc.gridImport[t] / periodHours,
    unserved: alloc.unserved[t] / periodHours,
    tariff: request.Tariff[t],
    cost: (request.Tariff[t] / 1000) * alloc.gridImport[t],
  }));

  return {
    objective,
    limit,
    predictedCost: alloc.totalCost,
    baselineCost: alloc.baselineCost,
    unservedEnergy: alloc.unserved.reduce((a, b) => a + b, 0),
    unreplenishedEnergy: Math.max(0, alloc.toReplenish),
    periodHours,
    periods: periodsOut,
    schedules,
  };
}

export async function runDayAhead(settings: ConnectionSettings, request: DayAheadRequest): Promise<DayAheadResult> {
  if (settings.mode === "demo") return simulateDayAhead(request);
  return liveFetch<DayAheadResult>(settings, "/run/dayahead", {
    method: "POST",
    body: JSON.stringify(request),
  });
}
