/*
*   FlexCHESS - Piclo Flex marketplace integration - AASServer-facing request/response models
*   tim@toshiba-bril.com
*/
using System;
using IO.Swagger.Piclo;

namespace IO.Swagger.Controllers
{
    // One Flex Asset to create/update in Piclo, optionally linked to a locally registered
    // CHESS. When ChessId is supplied, the controller verifies that CHESS is registered in
    // the digital twin and (if the caller left it blank) defaults Asset.Status to
    // "operational" - it does not invent capacity/timing figures, since ChessStatus tracks
    // flexible energy (Wh), not the power ratings (MW) Piclo's Asset schema expects.
    public class PicloAssetSyncRequest
    {
        public String ChessId { get; set; }
        public PicloAsset Asset { get; set; }
    }

    public class PicloAssetSyncResult
    {
        public String Ref { get; set; }
        public String ChessId { get; set; }
        // "created", "updated" or "error"
        public String Action { get; set; }
        public String Id { get; set; }
        public String Error { get; set; }
    }

    public class PicloPlannedAssetSyncResult
    {
        public String Reference { get; set; }
        // "created" or "error"
        public String Action { get; set; }
        public String Id { get; set; }
        public String Error { get; set; }
    }

    // Describes one Piclo Service Window to bid into, and the [StartHour, EndHour) slice
    // of a day-ahead schedule (see DayAheadResult) that its capacity/price should be
    // derived from.
    public class PicloServiceWindowBid
    {
        public String CompetitionId { get; set; }
        public String ServiceWindowId { get; set; }

        // Must match the parent Competition's type - "availability", "utilisation" or
        // "service_fee". Defaults to "utilisation" (the day-ahead schedule already
        // represents actual dispatched utilisation, not standing availability).
        public String RateType { get; set; } = "utilisation";

        public Double StartHour { get; set; }
        public Double EndHour { get; set; } = 24;

        // Explicit override for the offered rate (£/MW/h) - if omitted, derived from the
        // window's average day-ahead Tariff.
        public Double? RateValue { get; set; }

        // Multiplier applied to a derived (non-overridden) rate, e.g. 1.1 for a 10% margin
        // over the cost-reflective price. Default 1 bids at cost.
        public Double MarginMultiplier { get; set; } = 1;
    }

    // POST /piclo/bids body - bids are derived from the supplied day-ahead schedule
    // (typically the response of a prior POST /run/dayahead call) rather than recomputed,
    // so the same plan that was (or will be) dispatched locally is what gets bid into Piclo.
    public class PicloBidRequest
    {
        public DayAheadResult DayAhead { get; set; }
        public PicloServiceWindowBid[] ServiceWindows { get; set; }

        // Restricts the resulting Ballot(s) to specific Flex/Planned Assets. If omitted,
        // Piclo defaults to all of the provider's assets qualifying for each competition.
        public PicloBallotAssets Assets { get; set; }
    }

    public class PicloBidWindowResult
    {
        public String CompetitionId { get; set; }
        public String ServiceWindowId { get; set; }
        public Double CapacityMW { get; set; }
        public Double RateValue { get; set; }
        // Non-null if this window was left out of its Ballot (e.g. no capacity available)
        public String Skipped { get; set; }
    }

    public class PicloBidResult
    {
        public PicloBidWindowResult[] Windows { get; set; }
        public PicloBallotSubmission[] Ballots { get; set; }
    }

    public class PicloBallotSubmission
    {
        public String CompetitionId { get; set; }
        public String BallotId { get; set; }
        public String Error { get; set; }
    }
}
