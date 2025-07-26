namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelElementSupplementalSemanticIdRelationship : Relationship<Reference>, IEquatable<SubmodelElementSupplementalSemanticIdRelationship>
    {
        public SubmodelElementSupplementalSemanticIdRelationship()
        {
            Name = "supplementalSemanticId";
        }

        public SubmodelElementSupplementalSemanticIdRelationship(SubmodelElement source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelElementSupplementalSemanticIdRelationship);
        }

        public bool Equals(SubmodelElementSupplementalSemanticIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SubmodelElementSupplementalSemanticIdRelationship? left, SubmodelElementSupplementalSemanticIdRelationship? right)
        {
            return EqualityComparer<SubmodelElementSupplementalSemanticIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelElementSupplementalSemanticIdRelationship? left, SubmodelElementSupplementalSemanticIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubmodelElementSupplementalSemanticIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}