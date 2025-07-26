namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Range : DataElement, IEquatable<Range>
    {
        public Range()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Range;1";
        [JsonPropertyName("valueType")]
        public string? ValueType { get; set; }
        [JsonPropertyName("min")]
        public string? Min { get; set; }
        [JsonPropertyName("max")]
        public string? Max { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Range);
        }

        public bool Equals(Range? other)
        {
            return other is not null && base.Equals(other) && ValueType == other.ValueType && Min == other.Min && Max == other.Max;
        }

        public static bool operator ==(Range? left, Range? right)
        {
            return EqualityComparer<Range?>.Default.Equals(left, right);
        }

        public static bool operator !=(Range? left, Range? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), ValueType?.GetHashCode(), Min?.GetHashCode(), Max?.GetHashCode());
        }
    }
}