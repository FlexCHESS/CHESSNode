namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class BasicEventElement : EventElement, IEquatable<BasicEventElement>
    {
        public BasicEventElement()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:BasicEventElement;1";
        [JsonPropertyName("direction")]
        public BasicEventElementDirection? Direction { get; set; }
        [JsonPropertyName("state")]
        public BasicEventElementState? State { get; set; }
        [JsonPropertyName("messageTopic")]
        public string? MessageTopic { get; set; }
        [JsonPropertyName("lastUpdate")]
        public DateTime? LastUpdate { get; set; }
        [JsonPropertyName("minInterval")]
        public DateTime? MinInterval { get; set; }
        [JsonPropertyName("maxInterval")]
        public DateTime? MaxInterval { get; set; }
        [JsonIgnore]
        public BasicEventElementObservedRelationshipCollection Observed { get; set; } = new BasicEventElementObservedRelationshipCollection();
        [JsonIgnore]
        public BasicEventElementMessageBrokerRelationshipCollection MessageBroker { get; set; } = new BasicEventElementMessageBrokerRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as BasicEventElement);
        }

        public bool Equals(BasicEventElement? other)
        {
            return other is not null && base.Equals(other) && Direction == other.Direction && State == other.State && MessageTopic == other.MessageTopic && LastUpdate == other.LastUpdate && MinInterval == other.MinInterval && MaxInterval == other.MaxInterval;
        }

        public static bool operator ==(BasicEventElement? left, BasicEventElement? right)
        {
            return EqualityComparer<BasicEventElement?>.Default.Equals(left, right);
        }

        public static bool operator !=(BasicEventElement? left, BasicEventElement? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), Direction?.GetHashCode(), State?.GetHashCode(), MessageTopic?.GetHashCode(), LastUpdate?.GetHashCode(), MinInterval?.GetHashCode(), MaxInterval?.GetHashCode());
        }
    }
}