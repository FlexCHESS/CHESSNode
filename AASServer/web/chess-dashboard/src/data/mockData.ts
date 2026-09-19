import type { ChessCapability, ChessStatusProfile, ChessStatusWindow } from "../types";

// A small demonstration CHESS network - enough variety (battery, EV charger,
// heat pump) to exercise every visualisation without needing a live AAS server.
// Shapes match the real /chess/1.0/capability and /chess/1.0/status responses,
// so switching Settings to "live" against a real node needs no code changes.

function win(w: Partial<ChessStatusWindow> & Pick<ChessStatusWindow, "status" | "starttime" | "endtime" | "capacity">): ChessStatusWindow {
  return {
    service: "all",
    recurrence: "daily",
    efficiency: 0.92,
    priority: 5,
    probability: 0.9,
    capacityStart: 0,
    capacityEnd: Number(w.capacity) || 0,
    capacityMax: Number(w.capacity) || 0,
    cycleCost: 0.04,
    cycleCarbon: 0.012,
    cycleCostUnit: "GBP/kWh",
    cycleCarbonUnit: "kgCO2/kWh",
    ...w,
  };
}

export const mockCapabilities: ChessCapability[] = [
  {
    id: "chess-battery-01",
    identifier: "battery-01.site-a.chess",
    location: "Site A - Substation 3",
    standard: "IEC61850, MQTT",
    version: "1.2.0",
    submodels: "Nameplate Operational EnergyStorage",
    assetType: "battery",
  },
  {
    id: "chess-evcharger-04",
    identifier: "evcharger-04.site-a.chess",
    location: "Site A - Car Park",
    standard: "OCPP1.6, MQTT",
    version: "1.1.3",
    submodels: "Nameplate Operational ChargingPoint",
    assetType: "ev",
  },
  {
    id: "chess-heatpump-02",
    identifier: "heatpump-02.site-b.chess",
    location: "Site B - Plant Room",
    standard: "Modbus, MQTT",
    version: "1.0.5",
    submodels: "Nameplate Operational HVAC",
    assetType: "heatpump",
  },
];

export const mockStatusProfiles: ChessStatusProfile[] = [
  {
    identifier: "battery-01.site-a.chess",
    location: "Site A - Substation 3",
    id: "chess-battery-01",
    currentStatus: "available",
    status: [
      win({ id: "chess-battery-01-status-1", status: "ForceCharge", starttime: "01:00", endtime: "05:00", capacity: "4000", priority: 3, cycleCost: 0.032, cycleCarbon: 0.02, service: "all" }),
      win({ id: "chess-battery-01-status-2", status: "ForceDischarge", starttime: "07:00", endtime: "09:30", capacity: "3200", priority: 1, cycleCost: 0.06, cycleCarbon: 0.09, service: "dsr" }),
      win({ id: "chess-battery-01-status-3", status: "ForceDischarge", starttime: "16:30", endtime: "19:30", capacity: "4500", priority: 0, cycleCost: 0.09, cycleCarbon: 0.14, service: "dsr" }),
      win({ id: "chess-battery-01-status-4", status: "Idle", starttime: "19:30", endtime: "01:00", capacity: "0", priority: 9, cycleCost: 0, cycleCarbon: 0 }),
    ],
  },
  {
    identifier: "evcharger-04.site-a.chess",
    location: "Site A - Car Park",
    id: "chess-evcharger-04",
    currentStatus: "available",
    status: [
      win({ id: "chess-evcharger-04-status-1", status: "ForceCharge", starttime: "00:00", endtime: "06:00", capacity: "7000", priority: 2, cycleCost: 0.028, cycleCarbon: 0.018, recurrence: "weekdays", service: "all" }),
      win({ id: "chess-evcharger-04-status-2", status: "Curtail", starttime: "09:00", endtime: "17:00", capacity: "3500", priority: 4, cycleCost: 0.05, cycleCarbon: 0.05, service: "fcr" }),
      win({ id: "chess-evcharger-04-status-3", status: "Curtail", starttime: "17:00", endtime: "20:00", capacity: "2500", priority: 2, cycleCost: 0.07, cycleCarbon: 0.08, service: "dsr" }),
    ],
  },
  {
    identifier: "heatpump-02.site-b.chess",
    location: "Site B - Plant Room",
    id: "chess-heatpump-02",
    currentStatus: "available",
    status: [
      win({ id: "chess-heatpump-02-status-1", status: "ForceDischarge", starttime: "06:00", endtime: "08:00", capacity: "1800", priority: 1, cycleCost: 0.045, cycleCarbon: 0.03, recurrence: "daily", service: "dsr" }),
      win({ id: "chess-heatpump-02-status-2", status: "ForceCharge", starttime: "12:00", endtime: "15:00", capacity: "2200", priority: 5, cycleCost: 0.033, cycleCarbon: 0.021, service: "all" }),
      win({ id: "chess-heatpump-02-status-3", status: "ForceDischarge", starttime: "18:00", endtime: "21:00", capacity: "1600", priority: 3, cycleCost: 0.055, cycleCarbon: 0.06, service: "dsr" }),
      win({ id: "chess-heatpump-02-status-4", status: "Idle", starttime: "21:00", endtime: "06:00", capacity: "0", priority: 9, cycleCost: 0, cycleCarbon: 0 }),
    ],
  },
];
