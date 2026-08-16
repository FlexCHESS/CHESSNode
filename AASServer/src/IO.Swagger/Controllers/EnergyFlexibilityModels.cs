/*
*   FlexCHESS - day-ahead scheduling request/response models sourced from an IDTA Energy
*   Flexibility Data Model (EFDM) submodel instance
*   tim@toshiba-bril.com
*/
using System;
using IoT.Services;
using Newtonsoft.Json.Linq;

namespace IO.Swagger.Controllers
{
    // Request to compute (and optionally dispatch) a day-ahead schedule using a Flexible Load's
    // declared availability (powerStates and validity window) from an EFDM instance, instead of
    // querying the EMS adapter for live capacity.
    public class FlexibilityDayAheadRequest
    {
        // The AAS Submodel instance conforming to the IDTA Energy Flexibility Data Model,
        // describing the flexibilitySpace of one or more Flexible Loads. Accepts either a bare
        // Submodel object (as EnergyFlexibilityDataModel.json's "submodels[0]"), or the full
        // { assetAdministrationShells, submodels, conceptDescriptions } environment export - in
        // which case the first submodel carrying a flexibilitySpace_* element is used.
        public JObject FlexibilitySubmodel { get; set; }

        // Expects one entry named "maxpower" (unit "W") giving the demand limit to respect
        public Limit[] Limits { get; set; }

        // Reused from the /run contract so callers can label the run - Objective/Option are
        // echoed back in the result.
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

        // Clock time that period 0 of Demand/Tariff represents (default: today 00:00) - used to
        // resolve each Flexible Load's validity.from/until into the [0, periods) period range.
        public DateTime? PlanStart { get; set; }
    }

    // Request to express a previously computed day-ahead schedule as an EFDM
    // flexibleLoadMeasuresPackage instance.
    public class FlexibleLoadMeasuresRequest
    {
        // Output of a prior /run/dayahead or /run/dayahead/flexibility call
        public DayAheadResult Result { get; set; }

        // Clock time that period 0 of the result's Periods represents (default: today 00:00) -
        // used to compute the absolute timestamps of each loadChangeProfile point.
        public DateTime? PlanStart { get; set; }
    }
}
