/*
*   FlexCHESS - day-ahead scheduling sourced from an IDTA Energy Flexibility Data Model (EFDM)
*   submodel instance, plus EFDM-formatted export of a computed schedule as flexibleLoadMeasures
*   tim@toshiba-bril.com
*/
using IO.Swagger.Attributes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IoT.Services;
using Newtonsoft.Json.Linq;

namespace IO.Swagger.Controllers
{
    public partial class CHESSNetworkController : Controller
    {
        private const String EfdmNamespace = "https://admin-shell.io/idta/EnergyFlexibilityDataModel/1/0/";

        // ---- EFDM instance parsing helpers -------------------------------------------------
        // These walk the raw AAS JSON (idShort/value trees) directly rather than the generated
        // AAS model classes, since the latter need a polymorphic SubmodelElementChoice converter
        // that isn't wired up in this project and the EFDM tree is only ever read here, never
        // round-tripped through the SDK types.

        private JToken EfdmFindFirst(JToken container, String idShort)
        {
            JToken value = container?["value"];
            if (!(value is JArray arr)) return null;
            foreach (JToken child in arr)
                if (String.Equals((String)child["idShort"], idShort, StringComparison.OrdinalIgnoreCase))
                    return child;
            return null;
        }

        private IEnumerable<JToken> EfdmFindAll(JToken container, String idShort)
        {
            JToken value = container?["value"];
            if (!(value is JArray arr)) return Enumerable.Empty<JToken>();
            return arr.Where(child => String.Equals((String)child["idShort"], idShort, StringComparison.OrdinalIgnoreCase));
        }

        private String EfdmProperty(JToken container, String idShort)
        {
            return (String)EfdmFindFirst(container, idShort)?["value"];
        }

        private Double EfdmDouble(JToken container, String idShort, Double fallback = 0)
        {
            String s = EfdmProperty(container, idShort);
            return Double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out Double v) ? v : fallback;
        }

        // A Range element's min/max (unset bounds default to 0)
        private (Double Min, Double Max) EfdmRange(JToken container, String idShort)
        {
            JToken r = EfdmFindFirst(container, idShort);
            Double.TryParse((String)r?["min"], NumberStyles.Float, CultureInfo.InvariantCulture, out Double min);
            Double.TryParse((String)r?["max"], NumberStyles.Float, CultureInfo.InvariantCulture, out Double max);
            return (min, max);
        }

        // Normalise to UTC - a DateTime with no explicit offset (Kind Unspecified, e.g. as
        // deserialised from a bare "2026-08-16T00:00:00" with no "Z"/offset) is assumed to
        // already be UTC rather than the host server's local time.
        private DateTime? EfdmAsUtc(DateTime? value)
        {
            if (!value.HasValue) return null;
            return value.Value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
                : value.Value.ToUniversalTime();
        }

        // Locate the submodel carrying the EFDM flexibilitySpace, accepting either a bare
        // Submodel object or a full AAS environment export.
        private JToken EfdmResolveSubmodel(JObject body)
        {
            if (body["submodels"] is JArray submodels)
                return submodels.FirstOrDefault(sm => EfdmFindFlexibilitySpace(sm) != null);
            return body;
        }

        // Prefer the operational potential (site- and time-specific availability); fall back to
        // the application-tailored, then the general technical potential.
        private JToken EfdmFindFlexibilitySpace(JToken submodel)
        {
            JToken elements = submodel?["submodelElements"];
            if (!(elements is JArray)) return null;
            JToken elementsContainer = new JObject { ["value"] = elements };
            return EfdmFindFirst(elementsContainer, "flexibilitySpace_operationalPotential")
                ?? EfdmFindFirst(elementsContainer, "flexibilitySpace_applicationTailoredPotential")
                ?? EfdmFindFirst(elementsContainer, "flexibilitySpace_generalTechnicalPotential");
        }

        /// <summary>
        /// Compute a day-ahead schedule that keeps predicted grid import at or below a maximum
        /// power limit while minimising predicted cost, using an Energy Flexibility Data Model
        /// (EFDM) instance to describe the available Flexible Loads, then dispatch it via the
        /// EMS adapter
        /// </summary>
        /// <remarks>
        /// For each flexibleLoad in the submitted flexibilitySpace, sums the available power
        /// range of its powerStates (positive = increase in consumption / charge headroom,
        /// negative = decrease in consumption / discharge capacity) over their declared duration
        /// to build a discharge and/or charge resource, priced from flexibleLoadCosts.variableCost
        /// and restricted to the periods that fall within the Flexible Load's validity window.
        /// Uses the same greedy shave-then-replenish allocation as /run/dayahead: every period
        /// where forecast demand exceeds the limit is shaved using the cheapest eligible discharge
        /// resource first, then the energy used is replenished during the cheapest-tariff periods
        /// that still have headroom under the limit, using the cheapest eligible charge resource
        /// first. Storages and dependencies described in the EFDM instance are not yet
        /// incorporated into the allocation. The resulting per-asset schedule is POSTed to the EMS
        /// adapter's /status/{id} operation for each affected CHESS (unless Dispatch is false),
        /// keyed by flexibleLoadId.
        /// </remarks>
        /// <param name="body"></param>
        /// <response code="200">Successfully computed the day-ahead schedule</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unprocessable entity</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Route("/run/dayahead/flexibility")]
        [Route("/opt/1.0.0/run/dayahead/flexibility")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("runDayAheadFlexibilityPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(DayAheadResult), description: "Successfully computed the day-ahead schedule")]
        public virtual IActionResult runDayAheadFlexibilityPost([FromBody] FlexibilityDayAheadRequest body, [FromHeader] String Authorization)
        {
            if (Authorization != null)
                authToken = Authorization;

            if (body == null)
                return BadRequest();

            if (Authorization == null)
                return StatusCode(401);

            if (body.Demand == null || body.Tariff == null || body.Demand.Length == 0 || body.Demand.Length != body.Tariff.Length)
                return StatusCode(422, "Demand and Tariff must both be supplied and of equal, non-zero length");

            if (body.FlexibilitySubmodel == null)
                return StatusCode(422, "FlexibilitySubmodel must be supplied");

            Double limit = 0;
            if (body.Limits != null)
                foreach (Limit l in body.Limits)
                    if (l.Name != null && l.Name.ToLower().Equals("maxpower"))
                        limit = l.Value;

            if (limit <= 0)
                return StatusCode(422, "A positive 'maxpower' entry must be supplied in Limits");

            Double periodHours = body.PeriodHours > 0 ? body.PeriodHours : 1;
            Int32 periods = body.Demand.Length;
            String recurrence = String.IsNullOrEmpty(body.Recurrence) ? "daily" : body.Recurrence;
            String objective = (body.Options != null && body.Options.Length > 0 && body.Options[0].objective != null) ? body.Options[0].objective : "mincost";
            String option = (body.Options != null && body.Options.Length > 0 && body.Options[0].option != null) ? body.Options[0].option : "dayahead";
            // Compared against validity.from/until in UTC throughout (a timestamp with no
            // explicit offset is assumed to already be UTC), so the result doesn't depend on the
            // host server's local timezone.
            DateTime planStart = EfdmAsUtc(body.PlanStart) ?? DateTime.UtcNow.Date;

            JToken submodel = EfdmResolveSubmodel(body.FlexibilitySubmodel);
            JToken flexSpace = EfdmFindFlexibilitySpace(submodel);
            if (flexSpace == null)
                return StatusCode(422, "FlexibilitySubmodel does not contain a flexibilitySpace_operationalPotential, flexibilitySpace_applicationTailoredPotential or flexibilitySpace_generalTechnicalPotential element");

            JToken flexibleLoadsList = EfdmFindFirst(flexSpace, "flexibleLoads");
            List<JToken> loads = EfdmFindAll(flexibleLoadsList, "flexibleLoad").ToList();
            if (loads.Count == 0)
                return StatusCode(422, "FlexibilitySubmodel does not describe any flexibleLoads");

            // 1) Turn each flexibleLoad's declared powerStates and validity window into a
            //    discharge and/or charge resource, priced from its flexibleLoadCosts.
            List<FlexResource> dischargeResources = new List<FlexResource>();
            List<FlexResource> chargeResources = new List<FlexResource>();

            foreach (JToken load in loads)
            {
                String loadId = EfdmProperty(load, "flexibleLoadId");
                if (String.IsNullOrEmpty(loadId)) continue;

                Int32 eligibleStart = 0, eligibleEnd = periods - 1;
                JToken validity = EfdmFindFirst(load, "validity");
                if (validity != null)
                {
                    String fromStr = EfdmProperty(validity, "from");
                    String untilStr = EfdmProperty(validity, "until");
                    DateTimeStyles utcStyles = DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;
                    if (DateTime.TryParse(fromStr, CultureInfo.InvariantCulture, utcStyles, out DateTime from))
                        eligibleStart = Math.Max(0, (Int32)Math.Floor((from - planStart).TotalHours / periodHours));
                    if (DateTime.TryParse(untilStr, CultureInfo.InvariantCulture, utcStyles, out DateTime until))
                        eligibleEnd = Math.Min(periods - 1, (Int32)Math.Ceiling((until - planStart).TotalHours / periodHours) - 1);
                    if (eligibleEnd < eligibleStart) continue; // validity window falls outside the requested horizon
                }

                // variableCost follows the same currency/kWh convention as Tariff (the EFDM does
                // not carry an explicit unit for it); normalise to currency/Wh here exactly as
                // CostPerWh() does for the EMS adapter's cycleCost, so the two sourcing paths are
                // comparable and combine correctly in a single allocation.
                Double variableCostPerKwh = EfdmDouble(EfdmFindFirst(load, "flexibleLoadCosts"), "variableCost", 0);
                Double costPerWh = variableCostPerKwh / 1000.0;

                Double dischargeWh = 0, chargeWh = 0;
                JToken powerStatesList = EfdmFindFirst(load, "powerStates");
                foreach (JToken ps in EfdmFindAll(powerStatesList, "powerState"))
                {
                    (Double pMin, Double pMax) = EfdmRange(ps, "power");
                    (Double dMin, Double dMax) = EfdmRange(ps, "duration");
                    // Use the conservative (minimum) bound of an uncertain duration; fall back to
                    // whichever bound was actually supplied.
                    Double durationHours = dMin > 0 ? dMin : dMax;
                    if (durationHours <= 0) continue;
                    if (pMin < 0) dischargeWh += -pMin * durationHours;
                    if (pMax > 0) chargeWh += pMax * durationHours;
                }

                if (dischargeWh > 0)
                    dischargeResources.Add(new FlexResource
                    {
                        ChessId = loadId,
                        Entry = new IoT.Services.ChessStatus { status = "ForceDischarge", capacity=dischargeWh.ToString(), service = loadId, starttime = eligibleStart.ToString("00")+":00", endtime = eligibleEnd.ToString("00")+":00", recurrence = recurrence, cycleCost = variableCostPerKwh, cycleCostUnit = "currency/kWh" },
                        IsDischarge = true,
                        Remaining = dischargeWh,
                        CostPerWh = costPerWh,
                        EligibleStart = eligibleStart,
                        EligibleEnd = eligibleEnd
                    });

                if (chargeWh > 0)
                    chargeResources.Add(new FlexResource
                    {
                        ChessId = loadId,
                        Entry = new IoT.Services.ChessStatus { status = "ForceCharge", capacity=chargeWh.ToString(), service = loadId, starttime = eligibleStart.ToString("00")+":00", endtime = eligibleEnd.ToString("00")+":00", recurrence = recurrence, cycleCost = variableCostPerKwh, cycleCostUnit = "currency/kWh" },
                        IsDischarge = false,
                        Remaining = chargeWh,
                        CostPerWh = costPerWh,
                        EligibleStart = eligibleStart,
                        EligibleEnd = eligibleEnd
                    });
            }

            if (dischargeResources.Count == 0 && chargeResources.Count == 0)
                return StatusCode(422, "No usable discharge or charge capacity could be derived from the supplied flexibleLoads");

            dischargeResources = dischargeResources.OrderBy(r => r.CostPerWh).ToList();
            chargeResources = chargeResources.OrderBy(r => r.CostPerWh).ToList();

            // 2) Shave/replenish, then build and (optionally) dispatch the resulting schedule -
            //    identical to /run/dayahead from this point on.
            var allocation = AllocateDayAhead(body.Demand, body.Tariff, periodHours, limit, dischargeResources, chargeResources);
            List<CHESSStatus> schedules = BuildSchedules(dischargeResources.Concat(chargeResources), periodHours, recurrence);

            if (body.Dispatch)
                DispatchSchedules(schedules, Authorization);

            DayAheadPeriod[] periodsOut = new DayAheadPeriod[periods];
            for (Int32 t = 0; t < periods; t++)
                periodsOut[t] = new DayAheadPeriod
                {
                    Period = t,
                    Demand = body.Demand[t],
                    GridImport = allocation.GridImport[t] / periodHours,
                    Unserved = allocation.Unserved[t] / periodHours,
                    Tariff = body.Tariff[t],
                    Cost = body.Tariff[t] / 1000.0 * allocation.GridImport[t]
                };

            DayAheadResult resultOut = new DayAheadResult
            {
                Objective = objective,
                Limit = limit,
                PredictedCost = allocation.TotalCost,
                BaselineCost = allocation.BaselineCost,
                UnservedEnergy = allocation.Unserved.Sum(),
                UnreplenishedEnergy = Math.Max(0, allocation.ToReplenish),
                PeriodHours = periodHours,
                Periods = periodsOut,
                Schedules = schedules.ToArray()
            };

            Console.WriteLine("Day-ahead schedule computed from EFDM flexibility - predicted cost " + allocation.TotalCost + " vs baseline " + allocation.BaselineCost);

            return Json(resultOut);
        }

        // ---- EFDM flexibleLoadMeasuresPackage export ---------------------------------------

        private JObject EfdmSemanticRef(String value)
        {
            return new JObject
            {
                ["type"] = "ExternalReference",
                ["keys"] = new JArray(new JObject { ["type"] = "GlobalReference", ["value"] = value })
            };
        }

        private JObject EfdmPropertyElement(String idShort, String semanticId, String value, String valueType = "xs:string")
        {
            return new JObject
            {
                ["idShort"] = idShort,
                ["modelType"] = "Property",
                ["valueType"] = valueType,
                ["value"] = value,
                ["semanticId"] = EfdmSemanticRef(semanticId)
            };
        }

        private Double ParseHour(String hhmm)
        {
            if (String.IsNullOrEmpty(hhmm)) return 0;
            String[] parts = hhmm.Split(':');
            if (parts.Length < 2) return 0;
            Double.TryParse(parts[0], out Double hh);
            Double.TryParse(parts[1], out Double mm);
            return hh + mm / 60.0;
        }

        // A single point (power, timestamp) of a loadChangeProfile.
        private JObject EfdmLoadChangeProfilePoint(Double power, DateTime timestamp)
        {
            return new JObject
            {
                ["idShort"] = "loadChangeProfile",
                ["modelType"] = "SubmodelElementCollection",
                ["semanticId"] = EfdmSemanticRef(EfdmNamespace + "powerState"),
                ["value"] = new JArray(
                    EfdmPropertyElement("power", "0173-1#02-AAZ820#001", power.ToString(CultureInfo.InvariantCulture), "xs:float"),
                    EfdmPropertyElement("timestamp", "0173-1#02-ABF198#001", timestamp.ToString("o"), "xs:dateTime"))
            };
        }

        // Express a computed day-ahead schedule as an EFDM flexibleLoadMeasuresPackage: one
        // flexibleLoadMeasure per scheduled asset, whose loadChangeProfile traces the signed
        // power (negative = discharge / demand decrease, positive = charge / demand increase) at
        // the start of each window, closing back to zero at the end of the window.
        private JObject BuildFlexibleLoadMeasuresPackage(DayAheadResult result, DateTime planStart)
        {
            JArray measures = new JArray();
            foreach (CHESSStatus schedule in result.Schedules ?? Array.Empty<CHESSStatus>())
            {
                if (schedule.status == null || schedule.status.Length == 0) continue;

                JArray profile = new JArray();
                foreach (IoT.Services.ChessStatus window in schedule.status)
                {
                    Double startHour = ParseHour(window.starttime);
                    Double endHour = ParseHour(window.endtime);
                    Double durationHours = Math.Max(endHour - startHour, 1.0 / 3600.0);
                    Double.TryParse(window.capacity, NumberStyles.Float, CultureInfo.InvariantCulture, out Double wh);
                    Boolean isDischarge = window.status != null && window.status.ToLower().Contains("discharge");
                    Double signedPower = (isDischarge ? -1 : 1) * (wh / durationHours);

                    profile.Add(EfdmLoadChangeProfilePoint(signedPower, planStart.AddHours(startHour)));
                    profile.Add(EfdmLoadChangeProfilePoint(0, planStart.AddHours(endHour)));
                }

                measures.Add(new JObject
                {
                    ["idShort"] = "flexibleLoadMeasure",
                    ["modelType"] = "SubmodelElementCollection",
                    ["semanticId"] = EfdmSemanticRef(EfdmNamespace + "flexibleLoad"),
                    ["value"] = new JArray(
                        EfdmPropertyElement("flexibleLoadMeasureId", EfdmNamespace + "UUID", Guid.NewGuid().ToString()),
                        EfdmPropertyElement("status", EfdmNamespace + "status", "toExecute"),
                        EfdmPropertyElement("flexibleLoadId", EfdmNamespace + "UUID", schedule.id),
                        new JObject
                        {
                            ["idShort"] = "loadChangeProfiles",
                            ["modelType"] = "SubmodelElementList",
                            ["semanticId"] = EfdmSemanticRef(EfdmNamespace + "loadChangeProfiles"),
                            ["value"] = profile
                        })
                });
            }

            return new JObject
            {
                ["idShort"] = "flexibleLoadMeasuresPackage",
                ["modelType"] = "SubmodelElementCollection",
                ["semanticId"] = EfdmSemanticRef(EfdmNamespace + "flexibleLoadMeasuresPackage"),
                ["value"] = new JArray(
                    new JObject
                    {
                        ["idShort"] = "metadata",
                        ["modelType"] = "SubmodelElementCollection",
                        ["semanticId"] = EfdmSemanticRef(EfdmNamespace + "metadata"),
                        ["value"] = new JArray(
                            EfdmPropertyElement("instanceId", EfdmNamespace + "UUID", Guid.NewGuid().ToString()),
                            new JObject
                            {
                                ["idShort"] = "efdmVersion",
                                ["modelType"] = "SubmodelElementCollection",
                                ["semanticId"] = EfdmSemanticRef(EfdmNamespace + "efdmVersion"),
                                ["value"] = new JArray(EfdmPropertyElement("versionNumber", EfdmNamespace + "versionNumber", "1.0"))
                            })
                    },
                    new JObject
                    {
                        ["idShort"] = "flexibleLoadMeasures",
                        ["modelType"] = "SubmodelElementList",
                        ["semanticId"] = EfdmSemanticRef(EfdmNamespace + "flexibleLoadMeasures"),
                        ["value"] = measures
                    })
            };
        }

        /// <summary>
        /// Express a previously computed day-ahead schedule as an EFDM flexibleLoadMeasuresPackage
        /// instance
        /// </summary>
        /// <remarks>
        /// Takes the DayAheadResult produced by /run/dayahead or /run/dayahead/flexibility and
        /// re-expresses its per-asset Schedules as an EFDM flexibleLoadMeasuresPackage - one
        /// flexibleLoadMeasure per asset, whose loadChangeProfile carries the signed power
        /// (negative for discharge/demand-reduction, positive for charge/demand-increase) at the
        /// start and end of each scheduled window. This lets a schedule computed here be handed
        /// to, or filed alongside, other EFDM-speaking systems in the data model they expect.
        /// </remarks>
        /// <param name="body"></param>
        /// <response code="200">The supplied day-ahead result, expressed as an EFDM flexibleLoadMeasuresPackage</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Route("/run/dayahead/flexibility/measures")]
        [Route("/opt/1.0.0/run/dayahead/flexibility/measures")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("runDayAheadFlexibilityMeasuresPost")]
        [SwaggerResponse(statusCode: 200, description: "The supplied day-ahead result, expressed as an EFDM flexibleLoadMeasuresPackage")]
        public virtual IActionResult runDayAheadFlexibilityMeasuresPost([FromBody] FlexibleLoadMeasuresRequest body, [FromHeader] String Authorization)
        {
            if (Authorization != null)
                authToken = Authorization;

            if (body == null || body.Result == null)
                return BadRequest();

            if (Authorization == null)
                return StatusCode(401);

            DateTime planStart = EfdmAsUtc(body.PlanStart) ?? DateTime.UtcNow.Date;
            return Json(BuildFlexibleLoadMeasuresPackage(body.Result, planStart));
        }
    }
}
