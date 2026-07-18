/*
*   FlexCHESS - day-ahead cost-minimising scheduler request/response models
*   tim@toshiba-bril.com
*/
using System;
using IoT.Services;

namespace IO.Swagger.Controllers
{
    // Request to compute (and optionally dispatch) a day-ahead schedule that keeps
    // predicted grid import at or below a maximum power limit while minimising predicted cost.
    public class DayAheadRequest
    {
        // Expects one entry named "maxpower" (unit "W") giving the demand limit to respect
        public Limit[] Limits { get; set; }

        // Reused from the /run contract so callers can label the run - Objective/Option are
        // echoed back in the result. Status is ignored: the discharge/charge query windows
        // used to ask the EMS adapter for available flexibility are built internally.
        public OptionIn[] Options { get; set; }

        // Forecast net demand (before any flexibility is applied), one value per period, in W
        public Double[] Demand { get; set; }

        // Day-ahead price forecast, one value per period, in currency/kWh, aligned with Demand
        public Double[] Tariff { get; set; }

        // Duration represented by each Demand/Tariff entry, in hours (default: 1 = hourly)
        public Double PeriodHours { get; set; } = 1;

        // Recurrence label applied to the generated schedule entries (default: "daily")
        public String Recurrence { get; set; } = "daily";

        // If true (default), POST the resulting schedule to each affected CHESS via the EMS
        // adapter's /status/{id} operation. If false, only the computed plan is returned.
        public Boolean Dispatch { get; set; } = true;
    }

    public class DayAheadPeriod
    {
        public Int32 Period { get; set; }
        public Double Demand { get; set; }
        public Double GridImport { get; set; }
        public Double Unserved { get; set; }
        public Double Tariff { get; set; }
        public Double Cost { get; set; }
    }

    public class DayAheadResult
    {
        public String Objective { get; set; }
        public Double Limit { get; set; }
        public Double PredictedCost { get; set; }
        public Double BaselineCost { get; set; }
        public Double UnservedEnergy { get; set; }
        public Double UnreplenishedEnergy { get; set; }
        public DayAheadPeriod[] Periods { get; set; }
        public CHESSStatus[] Schedules { get; set; }
    }
}
