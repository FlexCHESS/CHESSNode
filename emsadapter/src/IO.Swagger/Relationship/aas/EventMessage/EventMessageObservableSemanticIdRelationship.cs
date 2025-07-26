namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EventMessageObservableSemanticIdRelationship : Relationship<Reference>, IEquatable<EventMessageObservableSemanticIdRelationship>
    {
        public EventMessageObservableSemanticIdRelationship()
        {
            Name = "observableSemanticId";
        }

        public EventMessageObservableSemanticIdRelationship(EventMessage source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EventMessageObservableSemanticIdRelationship);
        }

        public bool Equals(EventMessageObservableSemanticIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EventMessageObservableSemanticIdRelationship? left, EventMessageObservableSemanticIdRelationship? right)
        {
            return EqualityComparer<EventMessageObservableSemanticIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EventMessageObservableSemanticIdRelationship? left, EventMessageObservableSemanticIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EventMessageObservableSemanticIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}