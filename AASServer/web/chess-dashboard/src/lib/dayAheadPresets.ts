// Sample next-day demand and day-ahead tariff forecasts, hourly (24 periods),
// for populating the Day-ahead tab's inputs. Shapes are illustrative only -
// live mode users would source these from their own forecast/tariff feeds.

function demandCurve(base: number, morningPeak: number, eveningPeak: number): number[] {
  return Array.from({ length: 24 }, (_, h) => {
    let v = base;
    v += morningPeak * Math.exp(-((h - 8) ** 2) / 8);
    v += eveningPeak * Math.exp(-((h - 18) ** 2) / 6);
    return Math.round(v);
  });
}

function tariffCurve(offpeak: number, day: number, peak: number): number[] {
  return Array.from({ length: 24 }, (_, h) => {
    if (h < 6) return offpeak;
    if (h >= 16 && h < 20) return peak;
    return day;
  });
}

export interface DayAheadPreset {
  label: string;
  demand: number[];
  tariff: number[];
}

export const DAY_AHEAD_PRESETS: Record<string, DayAheadPreset> = {
  weekday: {
    label: "Typical weekday",
    demand: demandCurve(1200, 1800, 3200),
    tariff: tariffCurve(0.1111, 0.1780, 0.3204),
  },
  weekend: {
    label: "Weekend (flatter)",
    demand: demandCurve(1400, 900, 1800),
    tariff: tariffCurve(0.1240, 0.1602, 0.2214),
  },
};

export const DEFAULT_MAX_POWER_W = 4000;
