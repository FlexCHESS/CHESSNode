namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class TimeProperty : Property, IEquatable<TimeProperty>
    {
        public TimeProperty()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ext:TimeProperty;1";
        [JsonPropertyName("timeValue")]
        public object? TimeValue { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as TimeProperty);
        }

        public bool Equals(TimeProperty? other)
        {
            return other is not null && base.Equals(other) && TimeValue == other.TimeValue;
        }

        public static bool operator ==(TimeProperty? left, TimeProperty? right)
        {
            return EqualityComparer<TimeProperty?>.Default.Equals(left, right);
        }

        public static bool operator !=(TimeProperty? left, TimeProperty? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), TimeValue?.GetHashCode());
        }
    }
}