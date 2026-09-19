// Mirrors the C# digital-twin models in src/IO.Swagger/flexchess/*.cs and
// CHESSNetworkController.cs. Kept as plain data shapes (no class behaviour).

export type FlexStatusKind =
  | "Idle"
  | "Available"
  | "Charge"
  | "Discharge"
  | "ForceCharge"
  | "ForceDischarge"
  | "Curtail";

export type Recurrence = "daily" | "weekdays" | "weekends";

// One scheduled/available flexibility window for a CHESS asset.
// Mirrors IoT.Services.ChessStatus.
export interface ChessStatusWindow {
  id?: string;
  status: FlexStatusKind | string;
  service: string;
  starttime: string; // "HH:mm"
  endtime: string; // "HH:mm"
  capacity: string; // Wh, kept as string to match the wire format
  recurrence: Recurrence | string;
  efficiency: number;
  priority: number; // 0 (highest) .. 9 (lowest / excluded from KPI aggregation)
  probability: number;
  capacityStart: number;
  capacityEnd: number;
  capacityMax: number;
  cycleCost: number;
  cycleCarbon: number;
  cycleCostUnit: string;
  cycleCarbonUnit: string;
}

// The capability record for a registered CHESS asset. Mirrors IoT.Services.Chess
// plus the "submodels" / "status" fields the controller adds to Contents.
export interface ChessCapability {
  id: string;
  identifier: string;
  location: string;
  standard: string;
  version: string;
  submodels?: string;
  assetType?: string; // not on the wire model; inferred client-side for iconography
}

// Per-asset flexibility status profile. Mirrors CHESSNetworkController.CHESSStatus.
export interface ChessStatusProfile {
  identifier: string;
  location: string;
  id: string;
  currentStatus?: string;
  status: ChessStatusWindow[];
}

export type Objective = "cost" | "carbon";

// Mirrors IoT.Services.OptionIn - one asset's proposed set of windows, submitted
// to the optimiser or used to persist a new schedule.
export interface OptionIn {
  objective: Objective | string;
  option: string; // CHESS id
  status: ChessStatusWindow[];
}

export interface Limit {
  Name: string;
  Unit: string;
  Value: number;
}

export interface OptimiserRequest {
  Limits: Limit[];
  Options: OptionIn[];
}

// The /opt/1.0.0/run response is hand-built JSON in the controller (see
// CHESSNetworkController.runPost): six named KPI series of 10 values (one per
// priority level 0..9), plus the CHESS status profiles that were considered.
export interface KpiSeries {
  name: "costin" | "carbonin" | "capacityin" | "costout" | "carbonout" | "capacityout";
  unit: string;
  value: number[];
}

export interface OptimiserResultOption {
  Objective: string;
  Option: string;
  KPI: KpiSeries[];
  CHESS: ChessStatusProfile[];
}

export type OptimiserResponse = OptimiserResultOption[];

// Mirrors DayAheadRequest/DayAheadPeriod/DayAheadResult in
// IO.Swagger.Controllers.DayAheadModels.cs - the request/response for
// POST /run/dayahead.
export interface DayAheadRequest {
  Limits: Limit[]; // expects one entry named "maxpower" (Unit "W")
  Options: OptionIn[]; // Options[0].objective/option are echoed back; Status is ignored
  Demand: number[]; // forecast net demand before flexibility, one per period, in W
  Tariff: number[]; // day-ahead price forecast, one per period, in currency/kWh
  PeriodHours: number;
  Recurrence: Recurrence | string;
  Dispatch: boolean;
}

// NOTE: DayAheadPeriod/DayAheadResult are plain POCOs (unlike OptimiserResultOption,
// which the controller hand-builds as literal-cased JSON) - CHESSNetworkController
// returns them via `Json(resultOut)`, which goes through the app's globally
// configured Newtonsoft CamelCasePropertyNamesContractResolver (see Startup.cs).
// So on the wire these come back camelCased, not matching the C# PascalCase
// property names - these fields must stay lowercase-first to match the live API.
export interface DayAheadPeriod {
  period: number;
  demand: number; // W
  gridImport: number; // W, after flexibility is applied
  unserved: number; // W, demand above the limit that no resource could cover
  tariff: number; // currency/kWh
  cost: number; // currency, for this period's grid import
}

export interface DayAheadResult {
  objective: string;
  limit: number;
  predictedCost: number;
  baselineCost: number;
  unservedEnergy: number; // Wh
  unreplenishedEnergy: number; // Wh
  periodHours: number;
  periods: DayAheadPeriod[];
  schedules: ChessStatusProfile[];
}

export interface ConnectionSettings {
  mode: "demo" | "live";
  baseUrl: string;
  token: string;
  // The real API has no "list all capabilities" route (only /status does) -
  // live mode needs to know which CHESS ids to ask for individually.
  knownIds: string[];
}
