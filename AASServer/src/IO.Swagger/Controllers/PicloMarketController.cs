/*
*   FlexCHESS - Piclo Flex marketplace integration - CHESS network core API
*   Registers local CHESS assets (and planned CHESS builds) with the Piclo Flex marketplace
*   (https://docs.picloflex.com), and submits Bid ballots derived from the day-ahead
*   cost-minimising optimiser's output (see CHESSNetworkController.runDayAheadPost).
*   tim@toshiba-bril.com
*/

using IO.Swagger.Attributes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Azure;
using Azure.DigitalTwins.Core;
using IO.Swagger.Piclo;

namespace IO.Swagger.Controllers
{
    [ApiController]
    public class PicloMarketController : Controller
    {
        // Confirm a CHESS is actually registered in the digital twin before linking a
        // Piclo Asset to it - avoids silently bidding/registering against a typo'd id.
        private Boolean ChessExists(String chessId)
        {
            String query = "SELECT * FROM DigitalTwins where $metadata.$model = 'dtmi:com:flexchess:chess;1' and $dtId='" + chessId + "'";
            Pageable<IoT.Services.Chess> twinResponse = Program.dtClient.Query<IoT.Services.Chess>(query);
            return twinResponse != null && twinResponse.Count() > 0;
        }

        // Create or update one Flex Asset in Piclo, keyed on its Ref (Piclo has no
        // upsert-by-ref operation, so we look it up first).
        private PicloAssetSyncResult SyncAsset(PicloAssetSyncRequest request)
        {
            PicloAssetSyncResult result = new PicloAssetSyncResult { Ref = request.Asset?.Ref, ChessId = request.ChessId };
            try
            {
                if (request.Asset == null || String.IsNullOrEmpty(request.Asset.Ref))
                    throw new ArgumentException("Asset.Ref is required");

                if (!String.IsNullOrEmpty(request.ChessId))
                {
                    if (!ChessExists(request.ChessId))
                        throw new ArgumentException("No registered CHESS found for ChessId " + request.ChessId);
                    if (String.IsNullOrEmpty(request.Asset.Status))
                        request.Asset.Status = "operational";
                }

                if (String.IsNullOrEmpty(request.Asset.Provider))
                    request.Asset.Provider = Program.picloProviderId;

                PicloAsset[] existing = Program.picloClient.GetAssetByRef(request.Asset.Ref);
                if (existing.Length > 0)
                {
                    PicloAsset updated = Program.picloClient.UpdateAsset(existing[0].Id, request.Asset);
                    result.Action = "updated";
                    result.Id = updated.Id;
                }
                else
                {
                    PicloAsset created = Program.picloClient.CreateAsset(request.Asset);
                    result.Action = "created";
                    result.Id = created.Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error syncing Piclo asset " + request.Asset?.Ref + " - " + ex.ToString());
                result.Action = "error";
                result.Error = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Register or update local CHESS assets as Piclo Flex Assets
        /// </summary>
        /// <remarks>
        /// Creates (or, if a matching Ref already exists in Piclo, updates) one Flex Asset
        /// per entry. When an entry's ChessId is supplied, it must match a CHESS already
        /// registered via POST /register - the linked CHESS is used only to confirm the
        /// asset is real and to default Status to "operational"; all other Piclo asset
        /// fields (location, meter ids, capacities, technology classification, etc.) must
        /// be supplied explicitly, since Piclo's taxonomy and MW power ratings don't map
        /// from the CHESS digital twin's Wh-based flexibility model.
        /// </remarks>
        /// <param name="body"></param>
        /// <response code="200">Processed every requested asset (see each result for its outcome)</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unprocessable entity</response>
        /// <response code="503">Piclo integration not configured</response>
        [HttpPost]
        [Route("/piclo/assets")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("picloAssetsPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(PicloAssetSyncResult[]), description: "Processed every requested asset")]
        public virtual IActionResult PicloAssetsPost([Required][FromBody][SwaggerRequestBody("application/json")] PicloAssetSyncRequest[] body, [FromHeader] String Authorization)
        {
            if (Authorization == null)
                return StatusCode(401);

            if (Program.picloClient == null)
                return StatusCode(503, "Piclo integration is not configured - set PICLO_CLIENT_ID / PICLO_API_KEY");

            if (body == null || body.Length == 0)
                return StatusCode(422, "At least one asset must be supplied");

            return Json(body.Select(SyncAsset).ToArray());
        }

        /// <summary>
        /// Register planned (not-yet-built) CHESS capacity as Piclo Planned Assets
        /// </summary>
        /// <remarks>
        /// Planned Assets have no live CHESS telemetry to draw on, so each entry is passed
        /// through to Piclo as supplied (Piclo does not support looking Planned Assets up
        /// by reference, so re-submitting an existing reference will return an error for
        /// that entry rather than updating it - use PATCH /planned-assets/v1/{id}/ directly
        /// via Piclo for updates).
        /// </remarks>
        /// <param name="body"></param>
        /// <response code="200">Processed every requested planned asset (see each result for its outcome)</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unprocessable entity</response>
        /// <response code="503">Piclo integration not configured</response>
        [HttpPost]
        [Route("/piclo/planned-assets")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("picloPlannedAssetsPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(PicloPlannedAssetSyncResult[]), description: "Processed every requested planned asset")]
        public virtual IActionResult PicloPlannedAssetsPost([Required][FromBody][SwaggerRequestBody("application/json")] PicloPlannedAsset[] body, [FromHeader] String Authorization)
        {
            if (Authorization == null)
                return StatusCode(401);

            if (Program.picloClient == null)
                return StatusCode(503, "Piclo integration is not configured - set PICLO_CLIENT_ID / PICLO_API_KEY");

            if (body == null || body.Length == 0)
                return StatusCode(422, "At least one planned asset must be supplied");

            List<PicloPlannedAssetSyncResult> results = new List<PicloPlannedAssetSyncResult>();
            foreach (PicloPlannedAsset asset in body)
            {
                PicloPlannedAssetSyncResult result = new PicloPlannedAssetSyncResult { Reference = asset.Reference };
                try
                {
                    if (String.IsNullOrEmpty(asset.Provider))
                        asset.Provider = Program.picloProviderId;

                    PicloPlannedAsset created = Program.picloClient.CreatePlannedAsset(asset);
                    result.Action = "created";
                    result.Id = created.Id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error creating Piclo planned asset " + asset.Reference + " - " + ex.ToString());
                    result.Action = "error";
                    result.Error = ex.Message;
                }
                results.Add(result);
            }

            return Json(results.ToArray());
        }

        // Format a hour duration (e.g. 6.5) as Piclo's "DDTHH:mm:ss" (or "HH:mm:ss" if
        // under 24h) duration pattern, as required by FPBid.max_runtime.
        private String FormatDuration(Double hours)
        {
            Int32 totalMinutes = (Int32)Math.Round(Math.Max(0, hours) * 60);
            Int32 days = totalMinutes / (24 * 60);
            Int32 remainder = totalMinutes % (24 * 60);
            String hms = (remainder / 60).ToString("00") + ":" + (remainder % 60).ToString("00") + ":00";
            return days > 0 ? days + "T" + hms : hms;
        }

        /// <summary>
        /// Submit Piclo bid ballots derived from a day-ahead schedule
        /// </summary>
        /// <remarks>
        /// Takes the DayAheadResult produced by POST /run/dayahead (Dispatch may be true or
        /// false - bidding does not require the schedule to have already been dispatched)
        /// and a set of Piclo Service Windows to bid into. For each Service Window, the
        /// [StartHour, EndHour) slice of the day-ahead Periods is used to derive:
        /// capacity, as the average flexibility delivered (forecast Demand minus resulting
        /// GridImport) over that slice, in MW; and, unless RateValue overrides it, a
        /// cost-reflective rate from the slice's average Tariff (currency/kWh, converted to
        /// £/MW/h) times MarginMultiplier. Service Windows with no capacity to offer are
        /// skipped (see the Windows result) rather than submitted as zero-capacity bids.
        /// One Ballot is submitted per distinct CompetitionId, containing all of that
        /// competition's (non-skipped) Service Window bids.
        /// </remarks>
        /// <param name="body"></param>
        /// <response code="200">Successfully derived and submitted bid ballots</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="422">Unprocessable entity</response>
        /// <response code="503">Piclo integration not configured</response>
        [HttpPost]
        [Route("/piclo/bids")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("picloBidsPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(PicloBidResult), description: "Successfully derived and submitted bid ballots")]
        public virtual IActionResult PicloBidsPost([Required][FromBody][SwaggerRequestBody("application/json")] PicloBidRequest body, [FromHeader] String Authorization)
        {
            if (Authorization == null)
                return StatusCode(401);

            if (Program.picloClient == null)
                return StatusCode(503, "Piclo integration is not configured - set PICLO_CLIENT_ID / PICLO_API_KEY");

            if (body?.DayAhead?.Periods == null || body.DayAhead.Periods.Length == 0)
                return StatusCode(422, "A computed DayAhead result (from /run/dayahead) with Periods must be supplied");

            if (body.ServiceWindows == null || body.ServiceWindows.Length == 0)
                return StatusCode(422, "At least one ServiceWindows entry must be supplied");

            Double periodHours = body.DayAhead.PeriodHours > 0 ? body.DayAhead.PeriodHours : 1;

            List<PicloBidWindowResult> windowResults = new List<PicloBidWindowResult>();
            Dictionary<String, List<PicloBid>> bidsByCompetition = new Dictionary<String, List<PicloBid>>();

            foreach (PicloServiceWindowBid window in body.ServiceWindows)
            {
                DayAheadPeriod[] windowPeriods = body.DayAhead.Periods
                    .Where(p => p.Period * periodHours < window.EndHour && (p.Period + 1) * periodHours > window.StartHour)
                    .ToArray();

                PicloBidWindowResult windowResult = new PicloBidWindowResult { CompetitionId = window.CompetitionId, ServiceWindowId = window.ServiceWindowId };

                if (windowPeriods.Length == 0)
                {
                    windowResult.Skipped = "No day-ahead periods fall within the supplied window";
                    windowResults.Add(windowResult);
                    continue;
                }

                Double capacityW = windowPeriods.Average(p => p.Demand - p.GridImport);
                Double capacityMW = Math.Round(Math.Max(0, capacityW) / 1_000_000.0, 5);

                if (capacityMW <= 0)
                {
                    windowResult.Skipped = "No flexibility capacity delivered by the day-ahead schedule in this window";
                    windowResults.Add(windowResult);
                    continue;
                }

                Double rateValue = Math.Round((window.RateValue ?? windowPeriods.Average(p => p.Tariff) * 1000.0) * window.MarginMultiplier, 2);

                PicloBid bid = new PicloBid
                {
                    ServiceWindowId = window.ServiceWindowId,
                    Capacity = capacityMW.ToString("0.#####"),
                    MaxRuntime = FormatDuration(window.EndHour - window.StartHour),
                    Rates = new[] { new PicloRate { Type = String.IsNullOrEmpty(window.RateType) ? "utilisation" : window.RateType, Value = rateValue.ToString("0.00") } }
                };

                if (!bidsByCompetition.TryGetValue(window.CompetitionId, out List<PicloBid> bids))
                    bidsByCompetition[window.CompetitionId] = bids = new List<PicloBid>();
                bids.Add(bid);

                windowResult.CapacityMW = capacityMW;
                windowResult.RateValue = rateValue;
                windowResults.Add(windowResult);
            }

            List<PicloBallotSubmission> ballots = new List<PicloBallotSubmission>();
            foreach (KeyValuePair<String, List<PicloBid>> entry in bidsByCompetition)
            {
                PicloBallotSubmission submission = new PicloBallotSubmission { CompetitionId = entry.Key };
                try
                {
                    PicloBallotRequest ballot = new PicloBallotRequest { CompetitionId = entry.Key, Bids = entry.Value.ToArray(), Assets = body.Assets };
                    PicloBallotResponse response = Program.picloClient.SubmitBallot(entry.Key, ballot);
                    submission.BallotId = response.Id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error submitting Piclo ballot for competition " + entry.Key + " - " + ex.ToString());
                    submission.Error = ex.Message;
                }
                ballots.Add(submission);
            }

            return Json(new PicloBidResult { Windows = windowResults.ToArray(), Ballots = ballots.ToArray() });
        }
    }
}
