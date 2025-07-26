namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EventPayload : BasicDigitalTwin, IEquatable<EventPayload>, IEquatable<BasicDigitalTwin>
    {
        public EventPayload()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:EventPayload;1";
        [JsonPropertyName("topic")]
        public string? Topic { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }
        [JsonPropertyName("payload")]
        public string? Payload { get; set; }
        [JsonIgnore]
        public EventPayloadSourceRelationshipCollection Source { get; set; } = new EventPayloadSourceRelationshipCollection();
        [JsonIgnore]
        public EventPayloadSourceSemanticIdRelationshipCollection SourceSemanticId { get; set; } = new EventPayloadSourceSemanticIdRelationshipCollection();
        [JsonIgnore]
        public EventPayloadObservableReferenceRelationshipCollection ObservableReference { get; set; } = new EventPayloadObservableReferenceRelationshipCollection();
        [JsonIgnore]
        public EventPayloadObservableSemanticIdRelationshipCollection ObservableSemanticId { get; set; } = new EventPayloadObservableSemanticIdRelationshipCollection();
        [JsonIgnore]
        public EventPayloadSubjectIdRelationshipCollection SubjectId { get; set; } = new EventPayloadSubjectIdRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as EventPayload);
        }

        public bool Equals(EventPayload? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Topic == other.Topic && Timestamp == other.Timestamp && Payload == other.Payload;
        }

        public static bool operator ==(EventPayload? left, EventPayload? right)
        {
            return EqualityComparer<EventPayload?>.Default.Equals(left, right);
        }

        public static bool operator !=(EventPayload? left, EventPayload? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Topic?.GetHashCode(), Timestamp?.GetHashCode(), Payload?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as EventPayload) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}