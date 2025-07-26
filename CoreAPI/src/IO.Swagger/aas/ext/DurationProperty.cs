namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class DurationProperty : Property, IEquatable<DurationProperty>
    {
        public DurationProperty()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ext:DurationProperty;1";
        [JsonConverter(typeof(DurationConverter))]
        [JsonPropertyName("durationValue")]
        public TimeSpan? DurationValue { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as DurationProperty);
        }

        public bool Equals(DurationProperty? other)
        {
            return other is not null && base.Equals(other) && DurationValue == other.DurationValue;
        }

        public static bool operator ==(DurationProperty? left, DurationProperty? right)
        {
            return EqualityComparer<DurationProperty?>.Default.Equals(left, right);
        }

        public static bool operator !=(DurationProperty? left, DurationProperty? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), DurationValue?.GetHashCode());
        }
    }
}