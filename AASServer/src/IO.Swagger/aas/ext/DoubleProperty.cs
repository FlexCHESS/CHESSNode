namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class DoubleProperty : Property, IEquatable<DoubleProperty>
    {
        public DoubleProperty()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ext:DoubleProperty;1";
        [JsonPropertyName("doubleValue")]
        public double? DoubleValue { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as DoubleProperty);
        }

        public bool Equals(DoubleProperty? other)
        {
            return other is not null && base.Equals(other) && DoubleValue == other.DoubleValue;
        }

        public static bool operator ==(DoubleProperty? left, DoubleProperty? right)
        {
            return EqualityComparer<DoubleProperty?>.Default.Equals(left, right);
        }

        public static bool operator !=(DoubleProperty? left, DoubleProperty? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), DoubleValue?.GetHashCode());
        }
    }
}