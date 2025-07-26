namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EventPayloadSourceRelationship : Relationship<Referable>, IEquatable<EventPayloadSourceRelationship>
    {
        public EventPayloadSourceRelationship()
        {
            Name = "source";
        }

        public EventPayloadSourceRelationship(EventPayload source, Referable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EventPayloadSourceRelationship);
        }

        public bool Equals(EventPayloadSourceRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EventPayloadSourceRelationship? left, EventPayloadSourceRelationship? right)
        {
            return EqualityComparer<EventPayloadSourceRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EventPayloadSourceRelationship? left, EventPayloadSourceRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EventPayloadSourceRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}