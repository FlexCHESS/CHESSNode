import type { ChessStatusProfile, KpiSeries, OptimiserResponse, OptimiserResultOption } from "../types";

// Combines one OptimiserResultOption per asset (as returned by runOptimiser
// called once per asset) into a single network-wide result, by summing each
// KPI series element-wise across assets. Used by the Day-ahead tab to show
// one KPI breakdown for the whole dispatched schedule rather than one chart
// per asset.
export function aggregateOptimiserResults(results: OptimiserResponse, label: string): OptimiserResultOption | null {
  if (results.length === 0) return null;

  const names = results[0].KPI.map((k) => k.name);
  const KPI: KpiSeries[] = names.map((name) => {
    const matching = results.map((r) => r.KPI.find((k) => k.name === name)).filter((k): k is KpiSeries => !!k);
    const unit = matching.find((k) => k.unit)?.unit ?? "";
    const value = Array.from({ length: 10 }, (_, level) => matching.reduce((sum, k) => sum + (k.value[level] ?? 0), 0));
    return { name, unit, value };
  });

  const CHESS: ChessStatusProfile[] = results.flatMap((r) => r.CHESS);

  return { Objective: results[0].Objective, Option: label, KPI, CHESS };
}
