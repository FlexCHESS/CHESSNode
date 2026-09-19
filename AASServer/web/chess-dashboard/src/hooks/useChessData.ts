import { useCallback, useEffect, useState } from "react";
import { fetchAllStatusProfiles, fetchCapabilities } from "../api/client";
import { useSettings } from "../state/SettingsContext";
import type { ChessCapability, ChessStatusProfile } from "../types";

interface ChessDataState {
  capabilities: ChessCapability[];
  statusProfiles: ChessStatusProfile[];
  loading: boolean;
  error: string | null;
  reload: () => void;
}

export function useChessData(): ChessDataState {
  const { settings } = useSettings();
  const [capabilities, setCapabilities] = useState<ChessCapability[]>([]);
  const [statusProfiles, setStatusProfiles] = useState<ChessStatusProfile[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [tick, setTick] = useState(0);

  useEffect(() => {
    let cancelled = false;
    setLoading(true);
    setError(null);

    Promise.all([fetchCapabilities(settings, settings.knownIds), fetchAllStatusProfiles(settings)])
      .then(([caps, statuses]) => {
        if (cancelled) return;
        setCapabilities(caps);
        setStatusProfiles(statuses);
      })
      .catch((err: Error) => {
        if (cancelled) return;
        setError(err.message);
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });

    return () => {
      cancelled = true;
    };
  }, [settings, tick]);

  const reload = useCallback(() => setTick((t) => t + 1), []);

  return { capabilities, statusProfiles, loading, error, reload };
}
