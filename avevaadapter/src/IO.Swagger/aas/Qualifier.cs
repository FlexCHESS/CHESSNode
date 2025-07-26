namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Qualifier : HasSemantics, IEquatable<Qualifier>
    {
        public Qualifier()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Qualifier;1";
        [JsonPropertyName("kind")]
        public QualifierKind? Kind { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("valueType")]
        public string? ValueType { get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; }
        [JsonIgnore]
        public QualifierValueIdRelationshipCollection ValueId { get; set; } = new QualifierValueIdRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Qualifier);
        }

        public bool Equals(Qualifier? other)
        {
            return other is not null && base.Equals(other) && Kind == other.Kind && Type == other.Type && ValueType == other.ValueType && Value == other.Value;
        }

        public static bool operator ==(Qualifier? left, Qualifier? right)
        {
            return EqualityComparer<Qualifier?>.Default.Equals(left, right);
        }

        public static bool operator !=(Qualifier? left, Qualifier? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), Kind?.GetHashCode(), Type?.GetHashCode(), ValueType?.GetHashCode(), Value?.GetHashCode());
        }
    }
}