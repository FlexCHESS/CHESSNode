import { useEffect, useMemo, useState } from "react";
import type { ChessCapability, ChessStatusProfile, ChessStatusWindow, Objective, OptimiserResultOption } from "../types";
import { statusColor } from "../statusColors";
import { formatWh } from "../lib/time";
import { runOptimiser, saveStatusProfile } from "../api/client";
import { useSettings } from "../state/SettingsContext";
import { KpiBreakdownChart } from "./KpiBreakdownChart";

interface EditableWindow extends ChessStatusWindow {
  enabled: boolean;
}

interface Props {
  capabilities: ChessCapability[];
  statusProfiles: ChessStatusProfile[];
  selectedId: string | null;
  onSelect: (id: string) => void;
  onSaved: () => void;
}

export function PreferencesPanel({ capabilities, statusProfiles, selectedId, onSelect, onSaved }: Props) {
  const { settings } = useSettings();
  const profile = statusProfiles.find((p) => p.id === selectedId) ?? statusProfiles[0];
  const [windows, setWindows] = useState<EditableWindow[]>([]);
  const [objective, setObjective] = useState<Objective>("cost");
  const [result, setResult] = useState<OptimiserResultOption | null>(null);
  const [busy, setBusy] = useState<"none" | "run" | "save">("none");
  const [message, setMessage] = useState<{ text: string; error?: boolean } | null>(null);

  useEffect(() => {
    if (!profile) return;
    setWindows(profile.status.map((w) => ({ ...w, enabled: true })));
    setResult(null);
    setMessage(null);
  }, [profile?.id]);

  const activeCount = useMemo(() => windows.filter((w) => w.enabled).length, [windows]);

  if (!profile) {
    return <div className="empty-state">Select a CHESS asset to set its flexibility preferences.</div>;
  }

  function updateWindow(index: number, patch: Partial<EditableWindow>) {
    setWindows((prev) => prev.map((w, i) => (i === index ? { ...w, ...patch } : w)));
  }

  async function handlePreview() {
    setBusy("run");
    setMessage(null);
    try {
      const enabled = windows.filter((w) => w.enabled).map(({ enabled: _enabled, ...w }) => w);
      const response = await runOptimiser(settings, {
        Limits: [],
        Options: [{ objective, option: profile!.id, status: enabled }],
      });
      setResult(response[0] ?? null);
      setMessage({ text: "Optimiser preview updated below." });
    } catch (err) {
      setMessage({ text: (err as Error).message, error: true });
    } finally {
      setBusy("none");
    }
  }

  async function handleSave() {
    setBusy("save");
    setMessage(null);
    try {
      const enabled = windows.filter((w) => w.enabled).map(({ enabled: _enabled, ...w }) => w);
      await saveStatusProfile(settings, profile!.id, enabled);
      setMessage({ text: `Saved ${enabled.length} window(s) to ${profile!.id}.` });
      onSaved();
    } catch (err) {
      setMessage({ text: (err as Error).message, error: true });
    } finally {
      setBusy("none");
    }
  }

  return (
    <div style={{ display: "flex", flexDirection: "column", gap: 20 }}>
      <div className="card">
        <div className="form-grid" style={{ gridTemplateColumns: "1fr 1fr", display: "grid" }}>
          <div className="field">
            <label htmlFor="asset-select">CHESS asset</label>
            <select id="asset-select" value={profile.id} onChange={(e) => onSelect(e.target.value)}>
              {capabilities.map((c) => (
                <option key={c.id} value={c.id}>
                  {c.identifier}
                </option>
              ))}
            </select>
          </div>
          <div className="field">
            <label>Optimisation objective</label>
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
      </div>

      <div className="card">
        <div className="section-title">
          Flexibility windows - {activeCount} of {windows.length} selected
        </div>
        <div className="window-editor">
          {windows.map((w, i) => (
            <div className={"window-row" + (w.enabled ? "" : " disabled")} key={w.id ?? i}>
              <input type="checkbox" checked={w.enabled} onChange={(e) => updateWindow(i, { enabled: e.target.checked })} />
              <div className="window-row-main">
                <div className="window-row-title">
                  <span className="legend-swatch" style={{ background: statusColor(w.status) }} />
                  {w.status} · {w.starttime}–{w.endtime}
                </div>
                <span className="window-row-sub">
                  {w.service} · {w.recurrence} · {formatWh(w.capacityEnd || Number(w.capacity))}
                </span>
              </div>
              <div className="priority-control">
                Priority
                <input
                  type="range"
                  min={0}
                  max={9}
                  value={w.priority}
                  disabled={!w.enabled}
                  onChange={(e) => updateWindow(i, { priority: Number(e.target.value) })}
                />
                <strong style={{ color: "var(--text-primary)" }}>{w.priority}</strong>
              </div>
              <span className="field hint" style={{ minWidth: 60, textAlign: "right" }}>
                {w.priority === 0 ? "highest" : w.priority >= 8 ? "lowest" : ""}
              </span>
            </div>
          ))}
        </div>

        <div className="btn-row" style={{ marginTop: 16 }}>
          <button className="btn primary" onClick={handlePreview} disabled={busy !== "none" || activeCount === 0}>
            {busy === "run" ? "Running…" : "Preview optimised outcome"}
          </button>
          <button className="btn" onClick={handleSave} disabled={busy !== "none"}>
            {busy === "save" ? "Saving…" : "Save as CHESS schedule"}
          </button>
          {message && <span className={"status-line" + (message.error ? " error" : " good")}>{message.text}</span>}
        </div>
      </div>

      {result && (
        <div className="card">
          <div className="section-title">Optimiser preview - {objective === "cost" ? "cost-minimising" : "carbon-minimising"}</div>
          <KpiBreakdownChart result={result} />
        </div>
      )}
    </div>
  );
}
