import { useState } from "react";
import type { ChessStatusProfile, ChessStatusWindow } from "../types";
import { statusColor, STATUS_ORDER } from "../statusColors";
import { toMinutes, formatWh, formatMoney } from "../lib/time";

const DAY_MINUTES = 24 * 60;
const HOUR_TICKS = [0, 6, 12, 18, 24];

interface TooltipState {
  x: number;
  y: number;
  window: ChessStatusWindow;
}

function Segment({
  window: w,
  onHover,
  onLeave,
}: {
  window: ChessStatusWindow;
  onHover: (e: React.MouseEvent, w: ChessStatusWindow) => void;
  onLeave: () => void;
}) {
  const start = toMinutes(w.starttime);
  let end = toMinutes(w.endtime);
  if (end <= start) end = DAY_MINUTES; // window runs to end of day (e.g. "23:59")
  const left = (start / DAY_MINUTES) * 100;
  const width = Math.max(((end - start) / DAY_MINUTES) * 100, 0.6);

  return (
    <div
      className="timeline-seg"
      style={{
        left: `${left}%`,
        width: `${width}%`,
        background: statusColor(w.status),
        boxShadow: "0 0 0 1px var(--surface-1)",
      }}
      onMouseEnter={(e) => onHover(e, w)}
      onMouseMove={(e) => onHover(e, w)}
      onMouseLeave={onLeave}
    />
  );
}

function AssetRow({
  profile,
  onHover,
  onLeave,
}: {
  profile: ChessStatusProfile;
  onHover: (e: React.MouseEvent, w: ChessStatusWindow) => void;
  onLeave: () => void;
}) {
  return (
    <div className="timeline-row">
      <div className="timeline-row-label">
        {profile.id}
        <span className="timeline-row-sub">{profile.location}</span>
      </div>
      <div className="timeline-track">
        {profile.status.map((w, i) => (
          <Segment key={w.id ?? i} window={w} onHover={onHover} onLeave={onLeave} />
        ))}
      </div>
    </div>
  );
}

export function FlexibilityTimeline({ profiles }: { profiles: ChessStatusProfile[] }) {
  const [tooltip, setTooltip] = useState<TooltipState | null>(null);
  const [wrapRef, setWrapRef] = useState<HTMLDivElement | null>(null);

  function handleHover(e: React.MouseEvent, w: ChessStatusWindow) {
    if (!wrapRef) return;
    const rect = wrapRef.getBoundingClientRect();
    setTooltip({ x: e.clientX - rect.left, y: e.clientY - rect.top, window: w });
  }

  if (!profiles.length) {
    return <div className="empty-state">No flexibility schedule available.</div>;
  }

  return (
    <div className="timeline-block" ref={setWrapRef} style={{ position: "relative" }}>
      {profiles.map((p) => (
        <AssetRow key={p.id} profile={p} onHover={handleHover} onLeave={() => setTooltip(null)} />
      ))}

      <div className="timeline-row" aria-hidden>
        <div />
        <div style={{ position: "relative", height: 14 }}>
          {HOUR_TICKS.map((h) => (
            <span
              key={h}
              style={{
                position: "absolute",
                left: `${(h / 24) * 100}%`,
                transform: h === 24 ? "translateX(-100%)" : h === 0 ? "none" : "translateX(-50%)",
                fontSize: 11,
                color: "var(--text-muted)",
              }}
            >
              {String(h).padStart(2, "0")}:00
            </span>
          ))}
        </div>
      </div>

      <div className="legend">
        {STATUS_ORDER.map((s) => (
          <div className="legend-item" key={s}>
            <span className="legend-swatch" style={{ background: statusColor(s) }} />
            {s}
          </div>
        ))}
        <div className="legend-item">
          <span className="legend-swatch" style={{ background: "var(--baseline)" }} />
          Idle
        </div>
      </div>

      {tooltip && (
        <div className="timeline-tooltip" style={{ left: tooltip.x + 12, top: tooltip.y - 10 }}>
          <div className="tt-title">
            {tooltip.window.status} - {tooltip.window.starttime}–{tooltip.window.endtime}
          </div>
          <div className="tt-row">
            <span>Capacity</span>
            <span>{formatWh(tooltip.window.capacityEnd || Number(tooltip.window.capacity))}</span>
          </div>
          <div className="tt-row">
            <span>Service</span>
            <span>{tooltip.window.service}</span>
          </div>
          <div className="tt-row">
            <span>Priority</span>
            <span>{tooltip.window.priority}</span>
          </div>
          <div className="tt-row">
            <span>Recurrence</span>
            <span>{tooltip.window.recurrence}</span>
          </div>
          {tooltip.window.cycleCost > 0 && (
            <div className="tt-row">
              <span>Cycle cost</span>
              <span>{formatMoney(tooltip.window.cycleCost, tooltip.window.cycleCostUnit)}/kWh</span>
            </div>
          )}
        </div>
      )}
    </div>
  );
}
