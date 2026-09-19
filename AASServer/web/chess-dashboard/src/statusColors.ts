// Fixed categorical order for flexibility status kinds - identity, never
// re-ordered when a filter changes which kinds are present (dataviz skill:
// "color follows the entity, never its rank"). Idle/Available are inactive
// states, so they get neutral ink rather than a categorical slot.
//
// Demo data only emits ForceCharge/ForceDischarge/Curtail (plain Charge/
// Discharge were replaced network-wide), so the legend only lists those -
// but Charge/Discharge keep their already-assigned slots below so a live
// backend that still reports them renders with a stable, correct color.
export const STATUS_ORDER = ["ForceCharge", "ForceDischarge", "Curtail"] as const;

export const STATUS_COLOR_VAR: Record<string, string> = {
  Charge: "var(--series-1)",
  Discharge: "var(--series-2)",
  ForceCharge: "var(--series-3)",
  ForceDischarge: "var(--series-4)",
  Curtail: "var(--series-5)",
  Idle: "var(--baseline)",
  Available: "var(--baseline)",
};

export function statusColor(status: string): string {
  return STATUS_COLOR_VAR[status] ?? "var(--series-5)";
}

export function isActiveWindow(status: string): boolean {
  return status !== "Idle" && status !== "Available";
}
