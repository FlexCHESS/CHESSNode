namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class DateTimeProperty : Property, IEquatable<DateTimeProperty>
    {
        public DateTimeProperty()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ext:DateTimeProperty;1";
        [JsonPropertyName("dateTimeValue")]
        public DateTime? DateTimeValue { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as DateTimeProperty);
        }

        public bool Equals(DateTimeProperty? other)
        {
            return other is not null && base.Equals(other) && DateTimeValue == other.DateTimeValue;
        }

        public static bool operator ==(DateTimeProperty? left, DateTimeProperty? right)
        {
            return EqualityComparer<DateTimeProperty?>.Default.Equals(left, right);
        }

        public static bool operator !=(DateTimeProperty? left, DateTimeProperty? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), DateTimeValue?.GetHashCode());
        }
    }
}