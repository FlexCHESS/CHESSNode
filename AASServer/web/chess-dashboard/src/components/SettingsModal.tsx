import { useState } from "react";
import { useSettings } from "../state/SettingsContext";
import type { ConnectionSettings } from "../types";

export function SettingsModal({ onClose }: { onClose: () => void }) {
  const { settings, setSettings } = useSettings();
  const [draft, setDraft] = useState<ConnectionSettings>(settings);

  function save() {
    setSettings(draft);
    onClose();
  }

  return (
    <div className="modal-backdrop" onClick={onClose}>
      <div className="modal" onClick={(e) => e.stopPropagation()}>
        <div className="modal-head">
          <h3>Connection settings</h3>
          <button className="icon-btn" onClick={onClose} aria-label="Close">
            ✕
          </button>
        </div>

        <div className="field">
          <label>Data source</label>
          <div className="radio-row">
            <label>
              <input type="radio" checked={draft.mode === "demo"} onChange={() => setDraft({ ...draft, mode: "demo" })} />
              Demo data
            </label>
            <label>
              <input type="radio" checked={draft.mode === "live"} onChange={() => setDraft({ ...draft, mode: "live" })} />
              Live AAS server
            </label>
          </div>
          <span className="hint">
            Demo mode needs no backend. Live mode talks to a running AAS Server's{" "}
            <code>CHESSNetworkController</code> routes.
          </span>
        </div>

        {draft.mode === "live" && (
          <>
            <div className="field">
              <label htmlFor="base-url">API base URL</label>
              <input
                id="base-url"
                type="text"
                value={draft.baseUrl}
                onChange={(e) => setDraft({ ...draft, baseUrl: e.target.value })}
                placeholder="http://aasserver.default.svc"
              />
            </div>
            <div className="field">
              <label htmlFor="token">Authorization token</label>
              <input
                id="token"
                type="password"
                value={draft.token}
                onChange={(e) => setDraft({ ...draft, token: e.target.value })}
                placeholder="Bearer eyJ..."
              />
              <span className="hint">Sent as the Authorization header; needs the saFlexibilityProvider scope.</span>
            </div>
            <div className="field">
              <label htmlFor="known-ids">Known CHESS ids (comma separated)</label>
              <input
                id="known-ids"
                type="text"
                value={draft.knownIds.join(", ")}
                onChange={(e) =>
                  setDraft({ ...draft, knownIds: e.target.value.split(",").map((s) => s.trim()).filter(Boolean) })
                }
                placeholder="chess-battery-01, chess-evcharger-04"
              />
              <span className="hint">
                The API has no "list all" capability route - each asset id is fetched individually.
              </span>
            </div>
          </>
        )}

        <div className="btn-row" style={{ justifyContent: "flex-end" }}>
          <button className="btn" onClick={onClose}>
            Cancel
          </button>
          <button className="btn primary" onClick={save}>
            Save
          </button>
        </div>
      </div>
    </div>
  );
}
