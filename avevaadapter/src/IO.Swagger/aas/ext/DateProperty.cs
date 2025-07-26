namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class DateProperty : Property, IEquatable<DateProperty>
    {
        public DateProperty()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ext:DateProperty;1";
        [JsonConverter(typeof(DateOnlyConverter))]
        [JsonPropertyName("dateValue")]
        public DateOnly? DateValue { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as DateProperty);
        }

        public bool Equals(DateProperty? other)
        {
            return other is not null && base.Equals(other) && DateValue == other.DateValue;
        }

        public static bool operator ==(DateProperty? left, DateProperty? right)
        {
            return EqualityComparer<DateProperty?>.Default.Equals(left, right);
        }

        public static bool operator !=(DateProperty? left, DateProperty? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), DateValue?.GetHashCode());
        }
    }
}