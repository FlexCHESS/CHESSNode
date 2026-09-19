import { createContext, useContext, useEffect, useMemo, useState, type ReactNode } from "react";
import type { ConnectionSettings } from "../types";

const STORAGE_KEY = "chess-dashboard.settings";

const defaultSettings: ConnectionSettings = {
  mode: "demo",
  baseUrl: "http://localhost:5000",
  token: "",
  knownIds: ["chess-battery-01", "chess-evcharger-04", "chess-heatpump-02"],
};

function load(): ConnectionSettings {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) return defaultSettings;
    return { ...defaultSettings, ...JSON.parse(raw) };
  } catch {
    return defaultSettings;
  }
}

interface SettingsContextValue {
  settings: ConnectionSettings;
  setSettings: (next: ConnectionSettings) => void;
}

const SettingsContext = createContext<SettingsContextValue | null>(null);

export function SettingsProvider({ children }: { children: ReactNode }) {
  const [settings, setSettingsState] = useState<ConnectionSettings>(load);

  useEffect(() => {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(settings));
  }, [settings]);

  const value = useMemo(() => ({ settings, setSettings: setSettingsState }), [settings]);

  return <SettingsContext.Provider value={value}>{children}</SettingsContext.Provider>;
}

export function useSettings(): SettingsContextValue {
  const ctx = useContext(SettingsContext);
  if (!ctx) throw new Error("useSettings must be used within a SettingsProvider");
  return ctx;
}
