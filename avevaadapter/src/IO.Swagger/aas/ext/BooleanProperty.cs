namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class BooleanProperty : Property, IEquatable<BooleanProperty>
    {
        public BooleanProperty()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ext:BooleanProperty;1";
        [JsonPropertyName("booleanValue")]
        public bool? BooleanValue { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as BooleanProperty);
        }

        public bool Equals(BooleanProperty? other)
        {
            return other is not null && base.Equals(other) && BooleanValue == other.BooleanValue;
        }

        public static bool operator ==(BooleanProperty? left, BooleanProperty? right)
        {
            return EqualityComparer<BooleanProperty?>.Default.Equals(left, right);
        }

        public static bool operator !=(BooleanProperty? left, BooleanProperty? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), BooleanValue?.GetHashCode());
        }
    }
}