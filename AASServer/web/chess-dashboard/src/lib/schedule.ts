import type { ChessStatusProfile } from "../types";
import { toMinutes } from "./time";

// DayAheadResult.Schedules can contain more than one entry per CHESS id - the
// backend's BuildSchedules emits one entry per allocated resource (e.g. a
// charge resource and a discharge resource on the same asset), not one per
// asset. Merge them for display/persistence so each asset appears once.
export function mergeSchedulesByAsset(
  schedules: ChessStatusProfile[],
  lookup?: (id: string) => { identifier: string; location: string } | undefined
): ChessStatusProfile[] {
  const byId = new Map<string, ChessStatusProfile>();

  for (const s of schedules) {
    const meta = lookup?.(s.id);
    const existing = byId.get(s.id);
    if (existing) {
      existing.status = [...existing.status, ...s.status];
    } else {
      byId.set(s.id, {
        identifier: meta?.identifier ?? s.identifier ?? s.id,
        location: meta?.location ?? s.location ?? "",
        id: s.id,
        status: [...s.status],
      });
    }
  }

  for (const p of byId.values()) {
    p.status.sort((a, b) => toMinutes(a.starttime) - toMinutes(b.starttime));
  }

  return Array.from(byId.values());
}
