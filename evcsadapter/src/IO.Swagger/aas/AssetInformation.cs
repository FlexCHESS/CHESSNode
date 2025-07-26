namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AssetInformation : BasicDigitalTwin, IEquatable<AssetInformation>, IEquatable<BasicDigitalTwin>
    {
        public AssetInformation()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:AssetInformation;1";
        [JsonPropertyName("globalAssetIdValue")]
        public string? GlobalAssetIdValue { get; set; }
        [JsonPropertyName("specificAssetIdValues")]
        public string? SpecificAssetIdValues { get; set; }
        [JsonIgnore]
        public AssetInformationGlobalAssetIdRelationshipCollection GlobalAssetId { get; set; } = new AssetInformationGlobalAssetIdRelationshipCollection();
        [JsonIgnore]
        public AssetInformationSpecificAssetIdRelationshipCollection SpecificAssetId { get; set; } = new AssetInformationSpecificAssetIdRelationshipCollection();
        [JsonIgnore]
        public AssetInformationDefaultThumbnailRelationshipCollection DefaultThumbnail { get; set; } = new AssetInformationDefaultThumbnailRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as AssetInformation);
        }

        public bool Equals(AssetInformation? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && GlobalAssetIdValue == other.GlobalAssetIdValue && SpecificAssetIdValues == other.SpecificAssetIdValues;
        }

        public static bool operator ==(AssetInformation? left, AssetInformation? right)
        {
            return EqualityComparer<AssetInformation?>.Default.Equals(left, right);
        }

        public static bool operator !=(AssetInformation? left, AssetInformation? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), GlobalAssetIdValue?.GetHashCode(), SpecificAssetIdValues?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as AssetInformation) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}