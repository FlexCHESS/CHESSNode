namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EventElement : SubmodelElement, IEquatable<EventElement>
    {
        public EventElement()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:EventElement;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as EventElement);
        }

        public bool Equals(EventElement? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(EventElement? left, EventElement? right)
        {
            return EqualityComparer<EventElement?>.Default.Equals(left, right);
        }

        public static bool operator !=(EventElement? left, EventElement? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}