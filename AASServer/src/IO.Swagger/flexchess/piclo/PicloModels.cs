/*
*   FlexCHESS - Piclo Flex marketplace integration - wire models
*   Mirrors the subset of https://docs.picloflex.com used by PicloClient: authentication,
*   Flex Assets, Planned Assets, Bidding Opportunities and Bid ballots.
*   tim@toshiba-bril.com
*/
using System;
using Newtonsoft.Json;

namespace IO.Swagger.Piclo
{
    // POST /authtoken/v1/
    public class PicloLogin
    {
        [JsonProperty("client_id")]
        public String ClientId { get; set; }
        [JsonProperty("api_key")]
        public String ApiKey { get; set; }
    }

    public class PicloToken
    {
        [JsonProperty("token")]
        public String Token { get; set; }
    }

    // A Piclo Flex Asset - see /assets/v1/ (AssetUKPost / AssetUK / AssetNonGeo schemas).
    // Used for both create (POST) and update (PATCH) - fields left null are omitted from
    // the outgoing request (see PicloClient's NullValueHandling.Ignore) rather than sent
    // as an explicit reset, matching Piclo's partial-update semantics for PATCH.
    public class PicloAsset
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("provider")]
        public String Provider { get; set; }
        [JsonProperty("ref")]
        public String Ref { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("status")]
        public String Status { get; set; }
        [JsonProperty("asset_category")]
        public String AssetCategory { get; set; }
        [JsonProperty("asset_scale")]
        public String AssetScale { get; set; }
        [JsonProperty("asset_type")]
        public String AssetType { get; set; }
        [JsonProperty("voltage_level")]
        public String VoltageLevel { get; set; }
        [JsonProperty("country_code")]
        public String CountryCode { get; set; }
        [JsonProperty("postcode")]
        public String Postcode { get; set; }
        [JsonProperty("latitude")]
        public Double? Latitude { get; set; }
        [JsonProperty("longitude")]
        public Double? Longitude { get; set; }
        [JsonProperty("address")]
        public String Address { get; set; }
        [JsonProperty("export_meter_id")]
        public String ExportMeterId { get; set; }
        [JsonProperty("import_meter_id")]
        public String ImportMeterId { get; set; }
        [JsonProperty("active_export_capacity")]
        public String ActiveExportCapacity { get; set; }
        [JsonProperty("active_import_capacity")]
        public String ActiveImportCapacity { get; set; }
        [JsonProperty("reactive_export_capacity")]
        public String ReactiveExportCapacity { get; set; }
        [JsonProperty("reactive_import_capacity")]
        public String ReactiveImportCapacity { get; set; }
        [JsonProperty("max_import_capacity")]
        public String MaxImportCapacity { get; set; }
        [JsonProperty("max_export_capacity")]
        public String MaxExportCapacity { get; set; }
        [JsonProperty("response_time")]
        public String ResponseTime { get; set; }
        [JsonProperty("max_runtime")]
        public String MaxRuntime { get; set; }
        [JsonProperty("min_runtime")]
        public String MinRuntime { get; set; }
        [JsonProperty("recovery_time")]
        public String RecoveryTime { get; set; }
        [JsonProperty("connection_status")]
        public String ConnectionStatus { get; set; }
        [JsonProperty("connection_type")]
        public String ConnectionType { get; set; }
        [JsonProperty("connection_current")]
        public String ConnectionCurrent { get; set; }
        [JsonProperty("operational_date")]
        public String OperationalDate { get; set; }
        [JsonProperty("metering_point")]
        public String MeteringPoint { get; set; }
        [JsonProperty("meter_interval")]
        public String MeterInterval { get; set; }
        [JsonProperty("supplier")]
        public String Supplier { get; set; }
    }

    // A Piclo Planned Asset - see /planned-assets/v1/ (PlannedAssetPostUK / PlannedAssetUK /
    // PlannedAssetBase schemas). Planned Assets represent capacity not yet built/energised,
    // so (unlike PicloAsset) there is no live CHESS telemetry to enrich this from.
    public class PicloPlannedAsset
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("provider")]
        public String Provider { get; set; }
        [JsonProperty("reference")]
        public String Reference { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("voltage_level")]
        public String VoltageLevel { get; set; }
        [JsonProperty("asset_type")]
        public String AssetType { get; set; }
        [JsonProperty("capacity_type")]
        public String CapacityType { get; set; }
        [JsonProperty("capacity_value")]
        public String CapacityValue { get; set; }
        [JsonProperty("competition_reference")]
        public String CompetitionReference { get; set; }
        [JsonProperty("operator_name")]
        public String OperatorName { get; set; }
        [JsonProperty("estimated_asset_count")]
        public Int32? EstimatedAssetCount { get; set; }
        [JsonProperty("maximum_response_time")]
        public String MaximumResponseTime { get; set; }
        [JsonProperty("maximum_runtime")]
        public String MaximumRuntime { get; set; }
        [JsonProperty("minimum_runtime")]
        public String MinimumRuntime { get; set; }
        [JsonProperty("maximum_recovery_time")]
        public String MaximumRecoveryTime { get; set; }
    }

    // A single rate offered against a Bid - see OfferUK schema. "type" must match the
    // parent Competition's type (availability / utilisation / service_fee).
    public class PicloRate
    {
        [JsonProperty("type")]
        public String Type { get; set; }
        [JsonProperty("value")]
        public String Value { get; set; }
    }

    // A single Bid within a Ballot, targeting one Service Window - see FPBidPost schema.
    public class PicloBid
    {
        [JsonProperty("service_window_id")]
        public String ServiceWindowId { get; set; }
        [JsonProperty("capacity")]
        public String Capacity { get; set; }
        [JsonProperty("max_runtime")]
        public String MaxRuntime { get; set; }
        [JsonProperty("rates")]
        public PicloRate[] Rates { get; set; }
    }

    // Restricts a Ballot to specific Flex/Planned Assets - see FPBallotAssetsPost schema.
    // If omitted, Piclo defaults to all of the provider's qualifying assets.
    public class PicloBallotAssets
    {
        [JsonProperty("flex")]
        public String[] Flex { get; set; }
        [JsonProperty("planned")]
        public String[] Planned { get; set; }
    }

    // POST /bids/v1/competitions/{competitionId}/ballots/ - see FPBallotPostBids schema.
    public class PicloBallotRequest
    {
        [JsonProperty("competition_id")]
        public String CompetitionId { get; set; }
        [JsonProperty("bids")]
        public PicloBid[] Bids { get; set; }
        [JsonProperty("assets")]
        public PicloBallotAssets Assets { get; set; }
    }

    // Response to a submitted Ballot - see FPBallotResponse schema. Bids/Assets are left
    // as raw JSON since we only need the Ballot id/competition/created for confirmation.
    public class PicloBallotResponse
    {
        [JsonProperty("id")]
        public String Id { get; set; }
        [JsonProperty("competition_id")]
        public String CompetitionId { get; set; }
        [JsonProperty("created")]
        public String Created { get; set; }
        [JsonProperty("bids")]
        public Newtonsoft.Json.Linq.JToken Bids { get; set; }
        [JsonProperty("assets")]
        public Newtonsoft.Json.Linq.JToken Assets { get; set; }
    }
}
