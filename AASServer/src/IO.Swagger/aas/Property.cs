namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Property : DataElement, IEquatable<Property>
    {
        public Property()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Property;1";
        [JsonPropertyName("valueType")]
        public string? ValueType { get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; }
        [JsonIgnore]
        public PropertyValueIdRelationshipCollection ValueId { get; set; } = new PropertyValueIdRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Property);
        }

        public bool Equals(Property? other)
        {
            return other is not null && base.Equals(other) && ValueType == other.ValueType && Value == other.Value;
        }

        public static bool operator ==(Property? left, Property? right)
        {
            return EqualityComparer<Property?>.Default.Equals(left, right);
        }

        public static bool operator !=(Property? left, Property? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), ValueType?.GetHashCode(), Value?.GetHashCode());
        }
    }
}