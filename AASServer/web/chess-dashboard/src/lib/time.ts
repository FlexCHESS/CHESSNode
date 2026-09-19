export function toMinutes(hhmm: string): number {
  const [h, m] = hhmm.split(":").map(Number);
  return (h % 24) * 60 + (m || 0);
}

export function formatWh(wh: number): string {
  if (Math.abs(wh) >= 1000) return `${(wh / 1000).toFixed(wh % 1000 === 0 ? 0 : 1)} kWh`;
  return `${wh.toFixed(0)} Wh`;
}

export function formatW(w: number): string {
  if (Math.abs(w) >= 1000) return `${(w / 1000).toFixed(w % 1000 === 0 ? 0 : 1)} kW`;
  return `${w.toFixed(0)} W`;
}

// Accepts either a full unit ("GBP/kWh") or the single-letter code the optimiser's
// KPI aggregation reduces it to (CHESSNetworkController.runPost takes cycleCostUnit[0]).
export function formatMoney(value: number, unit: string): string {
  const code = (unit || "").charAt(0).toUpperCase();
  const symbol = code === "G" || code === "£" ? "£" : code === "E" || code === "€" ? "€" : code === "U" || code === "$" ? "$" : "";
  return `${symbol}${value.toFixed(2)}`;
}
