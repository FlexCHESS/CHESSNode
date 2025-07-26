namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EventPayloadObservableSemanticIdRelationship : Relationship<Reference>, IEquatable<EventPayloadObservableSemanticIdRelationship>
    {
        public EventPayloadObservableSemanticIdRelationship()
        {
            Name = "observableSemanticId";
        }

        public EventPayloadObservableSemanticIdRelationship(EventPayload source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EventPayloadObservableSemanticIdRelationship);
        }

        public bool Equals(EventPayloadObservableSemanticIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EventPayloadObservableSemanticIdRelationship? left, EventPayloadObservableSemanticIdRelationship? right)
        {
            return EqualityComparer<EventPayloadObservableSemanticIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EventPayloadObservableSemanticIdRelationship? left, EventPayloadObservableSemanticIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EventPayloadObservableSemanticIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}