import { AssetIcon } from "./AssetIcon";
import type { ChessCapability, ChessStatusProfile } from "../types";
import { isActiveWindow } from "../statusColors";

interface Props {
  capabilities: ChessCapability[];
  statusProfiles: ChessStatusProfile[];
  selectedId: string | null;
  onSelect: (id: string) => void;
}

function currentActivity(profile: ChessStatusProfile | undefined): { label: string; active: boolean } {
  if (!profile) return { label: "unknown", active: false };
  const active = profile.status.filter((w) => isActiveWindow(w.status));
  if (!active.length) return { label: "idle", active: false };
  return { label: `${active.length} active window${active.length > 1 ? "s" : ""}`, active: true };
}

export function NetworkOverview({ capabilities, statusProfiles, selectedId, onSelect }: Props) {
  if (!capabilities.length) {
    return <div className="empty-state">No CHESS assets registered yet.</div>;
  }

  return (
    <div className="card-grid">
      {capabilities.map((c) => {
        const profile = statusProfiles.find((p) => p.id === c.id);
        const activity = currentActivity(profile);
        return (
          <button
            key={c.id}
            className={"card asset-card" + (selectedId === c.id ? " selected" : "")}
            onClick={() => onSelect(c.id)}
            style={{ textAlign: "left" }}
          >
            <div className="asset-card-head">
              <div style={{ display: "flex", gap: 10 }}>
                <AssetIcon assetType={c.assetType} />
                <div className="asset-title">
                  <strong>{c.identifier}</strong>
                  <span>{c.id}</span>
                </div>
              </div>
              <span className={"badge " + (activity.active ? "good" : "neutral")}>
                {activity.active ? "● " : "○ "}
                {activity.label}
              </span>
            </div>
            <dl className="asset-meta">
              <dt>Location</dt>
              <dd>{c.location}</dd>
              <dt>Standard</dt>
              <dd>{c.standard}</dd>
              <dt>Version</dt>
              <dd>{c.version}</dd>
              {c.submodels && (
                <>
                  <dt>Submodels</dt>
                  <dd>{c.submodels.trim()}</dd>
                </>
              )}
            </dl>
          </button>
        );
      })}
    </div>
  );
}
