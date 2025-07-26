namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SpecificAssetId : HasSemantics, IEquatable<SpecificAssetId>
    {
        public SpecificAssetId()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:SpecificAssetId;1";
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; }
        [JsonIgnore]
        public SpecificAssetIdExternalSubjectIdRelationshipCollection ExternalSubjectId { get; set; } = new SpecificAssetIdExternalSubjectIdRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as SpecificAssetId);
        }

        public bool Equals(SpecificAssetId? other)
        {
            return other is not null && base.Equals(other) && Name == other.Name && Value == other.Value;
        }

        public static bool operator ==(SpecificAssetId? left, SpecificAssetId? right)
        {
            return EqualityComparer<SpecificAssetId?>.Default.Equals(left, right);
        }

        public static bool operator !=(SpecificAssetId? left, SpecificAssetId? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), Name?.GetHashCode(), Value?.GetHashCode());
        }
    }
}