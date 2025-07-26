namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EventMessageObservableReferenceRelationship : Relationship<Reference>, IEquatable<EventMessageObservableReferenceRelationship>
    {
        public EventMessageObservableReferenceRelationship()
        {
            Name = "observableReference";
        }

        public EventMessageObservableReferenceRelationship(EventMessage source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EventMessageObservableReferenceRelationship);
        }

        public bool Equals(EventMessageObservableReferenceRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EventMessageObservableReferenceRelationship? left, EventMessageObservableReferenceRelationship? right)
        {
            return EqualityComparer<EventMessageObservableReferenceRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EventMessageObservableReferenceRelationship? left, EventMessageObservableReferenceRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EventMessageObservableReferenceRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}