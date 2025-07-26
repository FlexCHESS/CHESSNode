namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class FloatProperty : Property, IEquatable<FloatProperty>
    {
        public FloatProperty()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ext:FloatProperty;1";
        [JsonPropertyName("floatValue")]
        public float? FloatValue { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as FloatProperty);
        }

        public bool Equals(FloatProperty? other)
        {
            return other is not null && base.Equals(other) && FloatValue == other.FloatValue;
        }

        public static bool operator ==(FloatProperty? left, FloatProperty? right)
        {
            return EqualityComparer<FloatProperty?>.Default.Equals(left, right);
        }

        public static bool operator !=(FloatProperty? left, FloatProperty? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), FloatValue?.GetHashCode());
        }
    }
}