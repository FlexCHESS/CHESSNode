namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EventMessage : BasicDigitalTwin, IEquatable<EventMessage>, IEquatable<BasicDigitalTwin>
    {
        public EventMessage()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:EventMessage;1";
        [JsonPropertyName("topic")]
        public string? Topic { get; set; }
        [JsonPropertyName("subject")]
        public string? Subject { get; set; }
        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }
        [JsonPropertyName("payload")]
        public string? Payload { get; set; }
        [JsonIgnore]
        public EventMessageSourceRelationshipCollection Source { get; set; } = new EventMessageSourceRelationshipCollection();
        [JsonIgnore]
        public EventMessageSourceSemanticIdRelationshipCollection SourceSemanticId { get; set; } = new EventMessageSourceSemanticIdRelationshipCollection();
        [JsonIgnore]
        public EventMessageObservableReferenceRelationshipCollection ObservableReference { get; set; } = new EventMessageObservableReferenceRelationshipCollection();
        [JsonIgnore]
        public EventMessageObservableSemanticIdRelationshipCollection ObservableSemanticId { get; set; } = new EventMessageObservableSemanticIdRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as EventMessage);
        }

        public bool Equals(EventMessage? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Topic == other.Topic && Subject == other.Subject && Timestamp == other.Timestamp && Payload == other.Payload;
        }

        public static bool operator ==(EventMessage? left, EventMessage? right)
        {
            return EqualityComparer<EventMessage?>.Default.Equals(left, right);
        }

        public static bool operator !=(EventMessage? left, EventMessage? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Topic?.GetHashCode(), Subject?.GetHashCode(), Timestamp?.GetHashCode(), Payload?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as EventMessage) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}