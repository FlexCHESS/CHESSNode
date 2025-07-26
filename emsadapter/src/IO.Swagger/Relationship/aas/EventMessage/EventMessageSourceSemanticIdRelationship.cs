namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EventMessageSourceSemanticIdRelationship : Relationship<Reference>, IEquatable<EventMessageSourceSemanticIdRelationship>
    {
        public EventMessageSourceSemanticIdRelationship()
        {
            Name = "sourceSemanticId";
        }

        public EventMessageSourceSemanticIdRelationship(EventMessage source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EventMessageSourceSemanticIdRelationship);
        }

        public bool Equals(EventMessageSourceSemanticIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EventMessageSourceSemanticIdRelationship? left, EventMessageSourceSemanticIdRelationship? right)
        {
            return EqualityComparer<EventMessageSourceSemanticIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EventMessageSourceSemanticIdRelationship? left, EventMessageSourceSemanticIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EventMessageSourceSemanticIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}