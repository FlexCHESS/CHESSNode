import { useState } from "react";
import "./app.css";
import { useSettings } from "./state/SettingsContext";
import { useChessData } from "./hooks/useChessData";
import { NetworkOverview } from "./components/NetworkOverview";
import { AssetDetail } from "./components/AssetDetail";
import { FlexibilityTimeline } from "./components/FlexibilityTimeline";
import { PreferencesPanel } from "./components/PreferencesPanel";
import { DayAheadPanel } from "./components/DayAheadPanel";
import { SettingsModal } from "./components/SettingsModal";

type Tab = "network" | "flexibility" | "preferences" | "dayahead";

const TABS: { id: Tab; label: string }[] = [
  { id: "network", label: "Network & capabilities" },
  { id: "flexibility", label: "Flexibility schedule" },
  { id: "preferences", label: "Preferences" },
  { id: "dayahead", label: "Day-ahead" },
];

function App() {
  const { settings } = useSettings();
  const { capabilities, statusProfiles, loading, error, reload } = useChessData();
  const [tab, setTab] = useState<Tab>("network");
  const [selectedId, setSelectedId] = useState<string | null>(null);
  const [settingsOpen, setSettingsOpen] = useState(false);

  const selected = capabilities.find((c) => c.id === selectedId) ?? capabilities[0] ?? null;
  const selectedProfile = statusProfiles.find((p) => p.id === selected?.id);

  // Only block on the very first fetch. A background reload triggered by
  // saving preferences or dispatching a day-ahead schedule must not unmount
  // the tab body underneath - that would wipe out the result/confirmation
  // state a panel just set right after the action that triggered it.
  const initialLoading = loading && capabilities.length === 0 && statusProfiles.length === 0;

  return (
    <div className="app">
      <header className="topbar">
        <div className="brand">
          <strong>⚡ CHESS Flexibility Dashboard</strong>
          <span>FlexCHESS network</span>
        </div>
        <nav className="tabs" role="tablist">
          {TABS.map((t) => (
            <button key={t.id} role="tab" className="tab" aria-selected={tab === t.id} onClick={() => setTab(t.id)}>
              {t.label}
            </button>
          ))}
        </nav>
        <span className={"mode-pill" + (settings.mode === "live" ? " live" : "")}>
          {settings.mode === "live" ? "Live" : "Demo data"}
        </span>
        <button className="settings-btn" onClick={() => setSettingsOpen(true)}>
          Settings
        </button>
      </header>

      <main className="content">
        {error && (
          <div className="card status-line error">
            Failed to load CHESS network data: {error}{" "}
            <button className="btn" onClick={reload} style={{ marginLeft: 8 }}>
              Retry
            </button>
          </div>
        )}

        {initialLoading && !error && <div className="empty-state">Loading CHESS network…</div>}

        {!initialLoading && !error && tab === "network" && (
          <>
            <div className="page-heading">
              <h2>Registered CHESS assets</h2>
              <p>
                Capability records pulled from the digital twin (<code>/chess/1.0/capability/&#123;id&#125;</code>).
                Select an asset to inspect its full capability and today's flexibility windows.
              </p>
            </div>
            <NetworkOverview
              capabilities={capabilities}
              statusProfiles={statusProfiles}
              selectedId={selected?.id ?? null}
              onSelect={setSelectedId}
            />
            {selected && <AssetDetail capability={selected} profile={selectedProfile} />}
          </>
        )}

        {!initialLoading && !error && tab === "flexibility" && (
          <>
            <div className="page-heading">
              <h2>Network-wide flexibility schedule</h2>
              <p>
                Charge / discharge / force windows for every registered asset over a 24h day, from{" "}
                <code>/chess/1.0/status</code>. Hover a segment for capacity, cost and priority.
              </p>
            </div>
            <div className="card">
              <FlexibilityTimeline profiles={statusProfiles} />
            </div>
          </>
        )}

        {!initialLoading && !error && tab === "preferences" && (
          <>
            <div className="page-heading">
              <h2>Select flexibility options</h2>
              <p>
                Choose which windows an asset offers, tune their priority, and preview the cost/carbon outcome
                before saving the schedule back to the digital twin.
              </p>
            </div>
            <PreferencesPanel
              capabilities={capabilities}
              statusProfiles={statusProfiles}
              selectedId={selected?.id ?? null}
              onSelect={setSelectedId}
              onSaved={reload}
            />
          </>
        )}

        {!initialLoading && !error && tab === "dayahead" && (
          <>
            <div className="page-heading">
              <h2>Day-ahead cost estimate</h2>
              <p>
                Runs <code>POST /run/dayahead</code> against a demand and tariff forecast to shave peaks with the
                cheapest available flexibility, then <code>POST /opt/1.0.0/run</code> on the resulting schedule for
                a per-priority-level cost/carbon breakdown.
              </p>
            </div>
            <DayAheadPanel capabilities={capabilities} onDispatched={reload} />
          </>
        )}
      </main>

      {settingsOpen && <SettingsModal onClose={() => setSettingsOpen(false)} />}
    </div>
  );
}

export default App;
