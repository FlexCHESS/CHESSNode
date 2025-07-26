namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AssetKind : BasicDigitalTwin, IEquatable<AssetKind>, IEquatable<BasicDigitalTwin>
    {
        public AssetKind()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:AssetKind;1";
        [JsonPropertyName("assetKind")]
        public AssetKind? AssetKindValue { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as AssetKind);
        }

        public bool Equals(AssetKind? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && AssetKindValue == other.AssetKindValue;
        }

        public static bool operator ==(AssetKind? left, AssetKind? right)
        {
            return EqualityComparer<AssetKind?>.Default.Equals(left, right);
        }

        public static bool operator !=(AssetKind? left, AssetKind? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), AssetKindValue?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as AssetKind) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}