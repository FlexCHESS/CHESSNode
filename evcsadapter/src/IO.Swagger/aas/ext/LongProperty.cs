namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class LongProperty : Property, IEquatable<LongProperty>
    {
        public LongProperty()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ext:LongProperty;1";
        [JsonPropertyName("longValue")]
        public long? LongValue { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as LongProperty);
        }

        public bool Equals(LongProperty? other)
        {
            return other is not null && base.Equals(other) && LongValue == other.LongValue;
        }

        public static bool operator ==(LongProperty? left, LongProperty? right)
        {
            return EqualityComparer<LongProperty?>.Default.Equals(left, right);
        }

        public static bool operator !=(LongProperty? left, LongProperty? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), LongValue?.GetHashCode());
        }
    }
}