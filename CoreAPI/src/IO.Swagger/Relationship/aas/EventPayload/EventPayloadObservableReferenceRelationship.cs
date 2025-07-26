namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EventPayloadObservableReferenceRelationship : Relationship<Referable>, IEquatable<EventPayloadObservableReferenceRelationship>
    {
        public EventPayloadObservableReferenceRelationship()
        {
            Name = "observableReference";
        }

        public EventPayloadObservableReferenceRelationship(EventPayload source, Referable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EventPayloadObservableReferenceRelationship);
        }

        public bool Equals(EventPayloadObservableReferenceRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EventPayloadObservableReferenceRelationship? left, EventPayloadObservableReferenceRelationship? right)
        {
            return EqualityComparer<EventPayloadObservableReferenceRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EventPayloadObservableReferenceRelationship? left, EventPayloadObservableReferenceRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EventPayloadObservableReferenceRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}