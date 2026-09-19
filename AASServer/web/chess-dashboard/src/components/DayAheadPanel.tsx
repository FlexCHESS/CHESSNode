import { useMemo, useState } from "react";
import {
  Area,
  Bar,
  BarChart,
  CartesianGrid,
  ComposedChart,
  Legend,
  Line,
  ReferenceLine,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import type { ChessCapability, ChessStatusProfile, DayAheadResult, Objective, OptimiserResultOption } from "../types";
import { useSettings } from "../state/SettingsContext";
import { runDayAhead, runOptimiser } from "../api/client";
import { aggregateOptimiserResults } from "../lib/kpi";
import { mergeSchedulesByAsset } from "../lib/schedule";
import { formatMoney, formatW } from "../lib/time";
import { DAY_AHEAD_PRESETS, DEFAULT_MAX_POWER_W } from "../lib/dayAheadPresets";
import { FlexibilityTimeline } from "./FlexibilityTimeline";
import { KpiBreakdownChart } from "./KpiBreakdownChart";

interface Props {
  capabilities: ChessCapability[];
  onDispatched: () => void;
}

function currencySymbol(value: number, unit: string): string {
  return formatMoney(value, unit).replace(/[\d.,-]/g, "");
}

export function DayAheadPanel({ capabilities, onDispatched }: Props) {
  const { settings } = useSettings();
  const [presetKey, setPresetKey] = useState<keyof typeof DAY_AHEAD_PRESETS>("weekday");
  const [maxPower, setMaxPower] = useState(DEFAULT_MAX_POWER_W);
  const [objective, setObjective] = useState<Objective>("cost");
  const [dispatch, setDispatch] = useState(false);
  const [result, setResult] = useState<DayAheadResult | null>(null);
  const [busy, setBusy] = useState<"none" | "run" | "kpi">("none");
  const [message, setMessage] = useState<{ text: string; error?: boolean } | null>(null);
  const [kpiResult, setKpiResult] = useState<OptimiserResultOption | null>(null);

  const preset = DAY_AHEAD_PRESETS[presetKey];

  const forecastRows = useMemo(
    () =>
      preset.demand.map((d, h) => ({
        hour: `${String(h).padStart(2, "0")}:00`,
        Demand: d,
        Tariff: preset.tariff[h],
      })),
    [preset]
  );

  const lookup = (id: string) => {
    const cap = capabilities.find((c) => c.id === id);
    return cap ? { identifier: cap.identifier, location: cap.location } : undefined;
  };

  const mergedSchedules: ChessStatusProfile[] = useMemo(
    () => (result ? mergeSchedulesByAsset(result.schedules, lookup) : []),
    [result, capabilities]
  );

  const periodRows = useMemo(
    () =>
      result
        ? result.periods.map((p) => ({
            hour: `${String(p.period).padStart(2, "0")}:00`,
            Demand: p.demand,
            "Grid import": p.gridImport,
            Cost: p.cost,
          }))
        : [],
    [result]
  );

  async function handleRun() {
    setBusy("run");
    setMessage(null);
    setKpiResult(null);
    try {
      const response = await runDayAhead(settings, {
        Limits: [{ Name: "maxpower", Unit: "W", Value: maxPower }],
        Options: [{ objective, option: "dayahead", status: [] }],
        Demand: preset.demand,
        Tariff: preset.tariff,
        PeriodHours: 1,
        Recurrence: "daily",
        Dispatch: dispatch,
      });
      setResult(response);
      setMessage({
        text: dispatch
          ? `Day-ahead schedule computed and dispatched to ${mergeSchedulesByAsset(response.schedules).length} asset(s).`
          : "Day-ahead schedule computed (not dispatched).",
      });
      if (dispatch) onDispatched();
    } catch (err) {
      setMessage({ text: (err as Error).message, error: true });
    } finally {
      setBusy("none");
    }
  }

  async function handleRunKpi() {
    if (!result) return;
    setBusy("kpi");
    try {
      const merged = mergeSchedulesByAsset(result.schedules);
      const responses = await Promise.all(
        merged.map((m) =>
          runOptimiser(settings, { Limits: [], Options: [{ objective, option: m.id, status: m.status }] })
        )
      );
      const aggregated = aggregateOptimiserResults(
        responses.map((r) => r[0]).filter(Boolean),
        "dayahead-network"
      );
      setKpiResult(aggregated);
    } catch (err) {
      setMessage({ text: (err as Error).message, error: true });
    } finally {
      setBusy("none");
    }
  }

  const savings = result ? result.baselineCost - result.predictedCost : 0;
  const savingsPct = result && result.baselineCost > 0 ? (savings / result.baselineCost) * 100 : 0;
  // Demand crossed the limit in at least one period but nothing was allocated to shave it -
  // distinct from "the limit was never crossed, so no flexibility was needed" (see empty-state
  // below). In live mode this combination almost always means the AAS server's call to the EMS
  // adapter (http://emsadapter.default.svc/current) returned nothing - it's unreachable, not
  // deployed, or has no assets with spare capacity right now - rather than a dashboard issue.
  const demandExceededLimit = result ? result.periods.some((p) => p.demand > result.limit) : false;

  return (
    <div style={{ display: "flex", flexDirection: "column", gap: 20 }}>
      <div className="card">
        <div className="form-grid" style={{ gridTemplateColumns: "1fr 1fr 1fr", display: "grid" }}>
          <div className="field">
            <label>Demand &amp; tariff forecast</label>
            <div className="radio-row">
              {Object.entries(DAY_AHEAD_PRESETS).map(([key, p]) => (
                <label key={key}>
                  <input
                    type="radio"
                    checked={presetKey === key}
                    onChange={() => setPresetKey(key as keyof typeof DAY_AHEAD_PRESETS)}
                  />
                  {p.label}
                </label>
              ))}
            </div>
          </div>
          <div className="field">
            <label htmlFor="max-power">Max site import (maxpower limit)</label>
            <input
              id="max-power"
              type="range"
              min={1000}
              max={8000}
              step={100}
              value={maxPower}
              onChange={(e) => setMaxPower(Number(e.target.value))}
            />
            <span className="hint">{formatW(maxPower)}</span>
          </div>
          <div className="field">
            <label>Objective</label>
            <div className="radio-row">
              <label>
                <input type="radio" checked={objective === "cost"} onChange={() => setObjective("cost")} />
                Minimise cost
              </label>
              <label>
                <input type="radio" checked={objective === "carbon"} onChange={() => setObjective("carbon")} />
                Minimise carbon
              </label>
            </div>
          </div>
        </div>

        <div className="section-title" style={{ marginTop: 18 }}>
          24h forecast preview
        </div>
        <ResponsiveContainer width="100%" height={140}>
          <ComposedChart data={forecastRows} margin={{ top: 4, right: 12, left: 0, bottom: 0 }}>
            <CartesianGrid stroke="var(--gridline)" vertical={false} />
            <XAxis dataKey="hour" interval={3} tick={{ fill: "var(--text-muted)", fontSize: 11 }} axisLine={{ stroke: "var(--baseline)" }} tickLine={false} />
            <YAxis tick={{ fill: "var(--text-muted)", fontSize: 11 }} axisLine={false} tickLine={false} tickFormatter={(v: number) => formatW(v)} width={56} />
            <ReferenceLine y={maxPower} stroke="var(--status-critical)" strokeDasharray="4 4" label={{ value: "Limit", position: "right", fill: "var(--status-critical)", fontSize: 11 }} />
            <Tooltip
              contentStyle={{ background: "var(--surface-2)", border: "1px solid var(--border)", borderRadius: 8, fontSize: 12.5 }}
              formatter={(value, name) => (name === "Tariff" ? [`${(Number(value) * 100).toFixed(1)}p/kWh`, name] : [formatW(Number(value)), name])}
            />
            <Area type="monotone" dataKey="Demand" stroke="var(--series-1)" fill="var(--series-1)" fillOpacity={0.18} strokeWidth={2} />
          </ComposedChart>
        </ResponsiveContainer>

        <div className="btn-row" style={{ marginTop: 16 }}>
          <button className="btn primary" onClick={handleRun} disabled={busy !== "none"}>
            {busy === "run" ? "Running…" : "Run day-ahead schedule"}
          </button>
          <label style={{ display: "flex", alignItems: "center", gap: 6, fontSize: 13 }}>
            <input type="checkbox" checked={dispatch} onChange={(e) => setDispatch(e.target.checked)} />
            Dispatch to assets
          </label>
          {message && <span className={"status-line" + (message.error ? " error" : " good")}>{message.text}</span>}
        </div>
      </div>

      {result && (
        <>
          <div className="stat-grid">
            <div className="stat-tile">
              <span className="label">Predicted cost</span>
              <span className="value">{formatMoney(result.predictedCost, "GBP")}</span>
            </div>
            <div className="stat-tile">
              <span className="label">Baseline cost (no flexibility)</span>
              <span className="value">{formatMoney(result.baselineCost, "GBP")}</span>
            </div>
            <div className="stat-tile">
              <span className="label">Predicted savings</span>
              <span className="value good">
                {formatMoney(savings, "GBP")} ({savingsPct.toFixed(0)}%)
              </span>
            </div>
            <div className="stat-tile">
              <span className="label">Unserved demand</span>
              <span className="value" style={{ color: result.unservedEnergy > 0 ? "var(--status-critical)" : undefined }}>
                {(result.unservedEnergy / 1000).toFixed(2)} kWh
              </span>
            </div>
          </div>

          <div className="card">
            <div className="section-title">Demand vs grid import (next 24h)</div>
            <ResponsiveContainer width="100%" height={240}>
              <ComposedChart data={periodRows} margin={{ top: 8, right: 12, left: 0, bottom: 0 }}>
                <CartesianGrid stroke="var(--gridline)" vertical={false} />
                <XAxis dataKey="hour" interval={2} tick={{ fill: "var(--text-muted)", fontSize: 12 }} axisLine={{ stroke: "var(--baseline)" }} tickLine={false} />
                <YAxis tick={{ fill: "var(--text-muted)", fontSize: 12 }} axisLine={false} tickLine={false} tickFormatter={(v: number) => formatW(v)} width={64} />
                <ReferenceLine y={result.limit} stroke="var(--status-critical)" strokeDasharray="4 4" label={{ value: "Limit", position: "right", fill: "var(--status-critical)", fontSize: 11 }} />
                <Tooltip
                  contentStyle={{ background: "var(--surface-2)", border: "1px solid var(--border)", borderRadius: 8, fontSize: 12.5 }}
                  formatter={(value) => formatW(Number(value))}
                />
                <Legend wrapperStyle={{ fontSize: 12.5, color: "var(--text-secondary)" }} />
                <Area type="monotone" dataKey="Demand" stroke="var(--series-3)" fill="var(--series-3)" fillOpacity={0.12} strokeWidth={1.5} />
                <Line type="monotone" dataKey="Grid import" stroke="var(--series-1)" strokeWidth={2} dot={false} />
              </ComposedChart>
            </ResponsiveContainer>
          </div>

          <div className="card">
            <div className="section-title">Predicted cost per hour</div>
            <ResponsiveContainer width="100%" height={200}>
              <BarChart data={periodRows} margin={{ top: 8, right: 12, left: 0, bottom: 0 }}>
                <CartesianGrid stroke="var(--gridline)" vertical={false} />
                <XAxis dataKey="hour" interval={2} tick={{ fill: "var(--text-muted)", fontSize: 12 }} axisLine={{ stroke: "var(--baseline)" }} tickLine={false} />
                <YAxis
                  tick={{ fill: "var(--text-muted)", fontSize: 12 }}
                  axisLine={false}
                  tickLine={false}
                  tickFormatter={(v: number) => `${currencySymbol(1, "GBP")}${v.toFixed(2)}`}
                  width={56}
                />
                <Tooltip
                  contentStyle={{ background: "var(--surface-2)", border: "1px solid var(--border)", borderRadius: 8, fontSize: 12.5 }}
                  formatter={(value) => formatMoney(Number(value), "GBP")}
                />
                <Bar dataKey="Cost" fill="var(--series-1)" radius={[4, 4, 0, 0]} maxBarSize={22} />
              </BarChart>
            </ResponsiveContainer>
          </div>

          <div className="card">
            <div className="section-title">
              Dispatched schedule ({mergedSchedules.length} asset{mergedSchedules.length === 1 ? "" : "s"} allocated)
            </div>
            {mergedSchedules.length ? (
              <FlexibilityTimeline profiles={mergedSchedules} />
            ) : demandExceededLimit ? (
              <div className="empty-state status-line error">
                Demand exceeded the {formatW(result!.limit)} limit in at least one period ({(result!.unservedEnergy / 1000).toFixed(2)} kWh
                unserved), but no flexibility capacity was available to shave it.
                {settings.mode === "live" &&
                  " Check that the EMS adapter (emsadapter.default.svc) is deployed and reachable from the AAS server, and that its registered assets currently have spare capacity."}
              </div>
            ) : (
              <div className="empty-state">No flexibility capacity was needed to stay under the limit.</div>
            )}
          </div>

          {mergedSchedules.length > 0 && (
            <div className="card">
              <div className="btn-row">
                <button className="btn" onClick={handleRunKpi} disabled={busy !== "none"}>
                  {busy === "kpi" ? "Running…" : "Run /opt/1.0.0/run on this schedule"}
                </button>
                <span className="field hint" style={{ margin: 0 }}>
                  Cross-checks the day-ahead plan against the optimiser's per-priority-level KPI breakdown.
                </span>
              </div>
              {kpiResult && (
                <div style={{ marginTop: 16 }}>
                  <KpiBreakdownChart result={kpiResult} />
                </div>
              )}
            </div>
          )}
        </>
      )}
    </div>
  );
}
