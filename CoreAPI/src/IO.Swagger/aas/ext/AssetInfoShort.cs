namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AssetInfoShort : BasicDigitalTwin, IEquatable<AssetInfoShort>, IEquatable<BasicDigitalTwin>
    {
        public AssetInfoShort()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:ext:AssetInfoShort;1";
        [JsonPropertyName("assetKind")]
        public AssetInfoShortAssetKind? AssetKind { get; set; }
        [JsonPropertyName("globalAssetId")]
        public string? GlobalAssetId { get; set; }
        [JsonPropertyName("specificAssetId")]
        public string? SpecificAssetId { get; set; }
        [JsonPropertyName("defaultThumbnailpath")]
        public string? DefaultThumbnailpath { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as AssetInfoShort);
        }

        public bool Equals(AssetInfoShort? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && AssetKind == other.AssetKind && GlobalAssetId == other.GlobalAssetId && SpecificAssetId == other.SpecificAssetId && DefaultThumbnailpath == other.DefaultThumbnailpath;
        }

        public static bool operator ==(AssetInfoShort? left, AssetInfoShort? right)
        {
            return EqualityComparer<AssetInfoShort?>.Default.Equals(left, right);
        }

        public static bool operator !=(AssetInfoShort? left, AssetInfoShort? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), AssetKind?.GetHashCode(), GlobalAssetId?.GetHashCode(), SpecificAssetId?.GetHashCode(), DefaultThumbnailpath?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as AssetInfoShort) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}