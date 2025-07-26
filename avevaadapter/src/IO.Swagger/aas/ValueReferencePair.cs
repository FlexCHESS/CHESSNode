namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ValueReferencePair : BasicDigitalTwin, IEquatable<ValueReferencePair>, IEquatable<BasicDigitalTwin>
    {
        public ValueReferencePair()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:ValueReferencePair;1";
        [JsonPropertyName("value")]
        public string? Value { get; set; }
        [JsonPropertyName("valueIdValue")]
        public string? ValueIdValue { get; set; }
        [JsonIgnore]
        public ValueReferencePairValueIdRelationshipCollection ValueId { get; set; } = new ValueReferencePairValueIdRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as ValueReferencePair);
        }

        public bool Equals(ValueReferencePair? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Value == other.Value && ValueIdValue == other.ValueIdValue;
        }

        public static bool operator ==(ValueReferencePair? left, ValueReferencePair? right)
        {
            return EqualityComparer<ValueReferencePair?>.Default.Equals(left, right);
        }

        public static bool operator !=(ValueReferencePair? left, ValueReferencePair? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Value?.GetHashCode(), ValueIdValue?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as ValueReferencePair) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}