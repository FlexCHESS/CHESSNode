namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EventMessageSourceRelationship : Relationship<Reference>, IEquatable<EventMessageSourceRelationship>
    {
        public EventMessageSourceRelationship()
        {
            Name = "source";
        }

        public EventMessageSourceRelationship(EventMessage source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EventMessageSourceRelationship);
        }

        public bool Equals(EventMessageSourceRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EventMessageSourceRelationship? left, EventMessageSourceRelationship? right)
        {
            return EqualityComparer<EventMessageSourceRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EventMessageSourceRelationship? left, EventMessageSourceRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EventMessageSourceRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}