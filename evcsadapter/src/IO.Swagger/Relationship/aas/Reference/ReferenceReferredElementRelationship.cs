namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ReferenceReferredElementRelationship : Relationship<Referable>, IEquatable<ReferenceReferredElementRelationship>
    {
        public ReferenceReferredElementRelationship()
        {
            Name = "referredElement";
        }

        public ReferenceReferredElementRelationship(Reference source, Referable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ReferenceReferredElementRelationship);
        }

        public bool Equals(ReferenceReferredElementRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(ReferenceReferredElementRelationship? left, ReferenceReferredElementRelationship? right)
        {
            return EqualityComparer<ReferenceReferredElementRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(ReferenceReferredElementRelationship? left, ReferenceReferredElementRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as ReferenceReferredElementRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}