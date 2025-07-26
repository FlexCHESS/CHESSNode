namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class IntegerProperty : Property, IEquatable<IntegerProperty>
    {
        public IntegerProperty()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ext:IntegerProperty;1";
        [JsonPropertyName("intValue")]
        public int? IntValue { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as IntegerProperty);
        }

        public bool Equals(IntegerProperty? other)
        {
            return other is not null && base.Equals(other) && IntValue == other.IntValue;
        }

        public static bool operator ==(IntegerProperty? left, IntegerProperty? right)
        {
            return EqualityComparer<IntegerProperty?>.Default.Equals(left, right);
        }

        public static bool operator !=(IntegerProperty? left, IntegerProperty? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), IntValue?.GetHashCode());
        }
    }
}