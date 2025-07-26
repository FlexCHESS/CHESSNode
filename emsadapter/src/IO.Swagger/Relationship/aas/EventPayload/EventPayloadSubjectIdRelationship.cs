namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EventPayloadSubjectIdRelationship : Relationship<Reference>, IEquatable<EventPayloadSubjectIdRelationship>
    {
        public EventPayloadSubjectIdRelationship()
        {
            Name = "subjectId";
        }

        public EventPayloadSubjectIdRelationship(EventPayload source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EventPayloadSubjectIdRelationship);
        }

        public bool Equals(EventPayloadSubjectIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EventPayloadSubjectIdRelationship? left, EventPayloadSubjectIdRelationship? right)
        {
            return EqualityComparer<EventPayloadSubjectIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EventPayloadSubjectIdRelationship? left, EventPayloadSubjectIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EventPayloadSubjectIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}