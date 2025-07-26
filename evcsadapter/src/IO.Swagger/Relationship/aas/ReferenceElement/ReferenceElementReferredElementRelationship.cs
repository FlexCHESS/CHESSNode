namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ReferenceElementReferredElementRelationship : Relationship<Referable>, IEquatable<ReferenceElementReferredElementRelationship>
    {
        public ReferenceElementReferredElementRelationship()
        {
            Name = "referredElement";
        }

        public ReferenceElementReferredElementRelationship(ReferenceElement source, Referable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ReferenceElementReferredElementRelationship);
        }

        public bool Equals(ReferenceElementReferredElementRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(ReferenceElementReferredElementRelationship? left, ReferenceElementReferredElementRelationship? right)
        {
            return EqualityComparer<ReferenceElementReferredElementRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(ReferenceElementReferredElementRelationship? left, ReferenceElementReferredElementRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as ReferenceElementReferredElementRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}