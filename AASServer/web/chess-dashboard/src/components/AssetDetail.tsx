import type { ChessCapability, ChessStatusProfile } from "../types";
import { statusColor } from "../statusColors";
import { formatMoney, formatWh } from "../lib/time";
import { FlexibilityTimeline } from "./FlexibilityTimeline";

export function AssetDetail({ capability, profile }: { capability: ChessCapability; profile?: ChessStatusProfile }) {
  return (
    <div style={{ display: "flex", flexDirection: "column", gap: 16 }}>
      <div className="card">
        <div className="section-title">Capability</div>
        <dl className="asset-meta" style={{ gridTemplateColumns: "140px 1fr" }}>
          <dt>Digital twin id</dt>
          <dd>{capability.id}</dd>
          <dt>Identifier</dt>
          <dd>{capability.identifier}</dd>
          <dt>Location</dt>
          <dd>{capability.location}</dd>
          <dt>Standard</dt>
          <dd>{capability.standard}</dd>
          <dt>Version</dt>
          <dd>{capability.version}</dd>
          <dt>Submodels</dt>
          <dd>{capability.submodels?.trim() || "—"}</dd>
        </dl>
      </div>

      {profile && (
        <>
          <div className="card">
            <div className="section-title">Today's flexibility schedule</div>
            <FlexibilityTimeline profiles={[profile]} />
          </div>

          <div className="card table-wrap">
            <div className="section-title">Windows ({profile.status.length})</div>
            <table className="data-table">
              <thead>
                <tr>
                  <th>Status</th>
                  <th>Window</th>
                  <th>Service</th>
                  <th>Recurrence</th>
                  <th>Priority</th>
                  <th>Capacity</th>
                  <th>Efficiency</th>
                  <th>Cycle cost</th>
                  <th>Cycle carbon</th>
                </tr>
              </thead>
              <tbody>
                {profile.status.map((w, i) => (
                  <tr key={w.id ?? i}>
                    <td>
                      <span className="legend-item">
                        <span className="legend-swatch" style={{ background: statusColor(w.status) }} />
                        {w.status}
                      </span>
                    </td>
                    <td>
                      {w.starttime}–{w.endtime}
                    </td>
                    <td>{w.service}</td>
                    <td>{w.recurrence}</td>
                    <td>{w.priority}</td>
                    <td>{formatWh(w.capacityEnd || Number(w.capacity))}</td>
                    <td>{(w.efficiency * 100).toFixed(0)}%</td>
                    <td>{w.cycleCost ? `${formatMoney(w.cycleCost, w.cycleCostUnit)}/kWh` : "—"}</td>
                    <td>{w.cycleCarbon ? `${w.cycleCarbon.toFixed(3)} ${w.cycleCarbonUnit}` : "—"}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </>
      )}
    </div>
  );
}
