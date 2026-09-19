import { useMemo, useState } from "react";
import { Bar, BarChart, CartesianGrid, Legend, ResponsiveContainer, Tooltip, XAxis, YAxis } from "recharts";
import type { OptimiserResultOption } from "../types";
import { formatMoney, formatWh } from "../lib/time";

type Metric = "capacity" | "cost" | "carbon";

const METRIC_LABEL: Record<Metric, string> = {
  capacity: "Capacity",
  cost: "Degradation cost",
  carbon: "Cycle carbon",
};

function seriesFor(result: OptimiserResultOption, metric: Metric) {
  const inSeries = result.KPI.find((k) => k.name === `${metric}in`);
  const outSeries = result.KPI.find((k) => k.name === `${metric}out`);
  const unit = inSeries?.unit || outSeries?.unit || "";
  const rows = Array.from({ length: 10 }, (_, level) => ({
    level: `P${level}`,
    Charging: inSeries?.value[level] ?? 0,
    Discharging: outSeries?.value[level] ?? 0,
  })).filter((r) => r.Charging !== 0 || r.Discharging !== 0);
  return { rows, unit };
}

function formatValue(metric: Metric, value: number, unit: string): string {
  if (metric === "capacity") return formatWh(value);
  if (metric === "cost") return formatMoney(value, unit);
  return `${value.toFixed(2)} ${unit}`;
}

export function KpiBreakdownChart({ result }: { result: OptimiserResultOption }) {
  const [metric, setMetric] = useState<Metric>("capacity");
  const { rows, unit } = useMemo(() => seriesFor(result, metric), [result, metric]);

  const totals = useMemo(() => {
    const sum = (name: string) => result.KPI.find((k) => k.name === name)?.value.reduce((a, b) => a + b, 0) ?? 0;
    return {
      capacityIn: sum("capacityin"),
      capacityOut: sum("capacityout"),
      costIn: sum("costin"),
      costOut: sum("costout"),
      carbonIn: sum("carbonin"),
      carbonOut: sum("carbonout"),
    };
  }, [result]);

  const costUnit = result.KPI.find((k) => k.name === "costin")?.unit ?? "";
  const carbonUnit = result.KPI.find((k) => k.name === "carbonin")?.unit ?? "";

  return (
    <div style={{ display: "flex", flexDirection: "column", gap: 16 }}>
      <div className="stat-grid">
        <div className="stat-tile">
          <span className="label">Capacity charging</span>
          <span className="value">{formatWh(totals.capacityIn)}</span>
        </div>
        <div className="stat-tile">
          <span className="label">Capacity discharging</span>
          <span className="value good">{formatWh(totals.capacityOut)}</span>
        </div>
        <div className="stat-tile">
          <span className="label">Degradation cost</span>
          <span className="value">{formatMoney(totals.costIn + totals.costOut, costUnit)}</span>
        </div>
        <div className="stat-tile">
          <span className="label">Cycle carbon</span>
          <span className="value">
            {(totals.carbonIn + totals.carbonOut).toFixed(2)} {carbonUnit}
          </span>
        </div>
      </div>

      <div className="radio-row" role="tablist" aria-label="Metric">
        {(Object.keys(METRIC_LABEL) as Metric[]).map((m) => (
          <label key={m}>
            <input type="radio" name="metric" checked={metric === m} onChange={() => setMetric(m)} />
            {METRIC_LABEL[m]}
          </label>
        ))}
      </div>

      {rows.length === 0 ? (
        <div className="empty-state">No priority levels with allocated capacity yet - enable some windows above.</div>
      ) : (
        <ResponsiveContainer width="100%" height={260}>
          <BarChart data={rows} margin={{ top: 8, right: 12, left: 0, bottom: 0 }} barGap={2}>
            <CartesianGrid stroke="var(--gridline)" vertical={false} />
            <XAxis dataKey="level" tick={{ fill: "var(--text-muted)", fontSize: 12 }} axisLine={{ stroke: "var(--baseline)" }} tickLine={false} />
            <YAxis
              tick={{ fill: "var(--text-muted)", fontSize: 12 }}
              axisLine={false}
              tickLine={false}
              tickFormatter={(v: number) => formatValue(metric, v, unit)}
              width={72}
            />
            <Tooltip
              cursor={{ fill: "var(--gridline)" }}
              contentStyle={{ background: "var(--surface-2)", border: "1px solid var(--border)", borderRadius: 8, fontSize: 12.5 }}
              formatter={(value) => formatValue(metric, Number(value), unit)}
            />
            <Legend wrapperStyle={{ fontSize: 12.5, color: "var(--text-secondary)" }} />
            <Bar dataKey="Charging" fill="var(--series-1)" radius={[4, 4, 0, 0]} maxBarSize={28} />
            <Bar dataKey="Discharging" fill="var(--series-2)" radius={[4, 4, 0, 0]} maxBarSize={28} />
          </BarChart>
        </ResponsiveContainer>
      )}
      <p className="field hint" style={{ margin: 0 }}>
        Priority levels (P0 = highest) mirror the optimiser's KPI aggregation in{" "}
        <code>CHESSNetworkController.runPost</code> - lower levels are shaved/replenished first.
      </p>
    </div>
  );
}
