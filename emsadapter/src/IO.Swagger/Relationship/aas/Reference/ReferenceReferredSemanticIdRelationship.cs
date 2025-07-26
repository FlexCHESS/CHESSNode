namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ReferenceReferredSemanticIdRelationship : Relationship<Reference>, IEquatable<ReferenceReferredSemanticIdRelationship>
    {
        public ReferenceReferredSemanticIdRelationship()
        {
            Name = "referredSemanticId";
        }

        public ReferenceReferredSemanticIdRelationship(Reference source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ReferenceReferredSemanticIdRelationship);
        }

        public bool Equals(ReferenceReferredSemanticIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(ReferenceReferredSemanticIdRelationship? left, ReferenceReferredSemanticIdRelationship? right)
        {
            return EqualityComparer<ReferenceReferredSemanticIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(ReferenceReferredSemanticIdRelationship? left, ReferenceReferredSemanticIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as ReferenceReferredSemanticIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}