namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelSupplementalSemanticIdRelationship : Relationship<Reference>, IEquatable<SubmodelSupplementalSemanticIdRelationship>
    {
        public SubmodelSupplementalSemanticIdRelationship()
        {
            Name = "supplementalSemanticId";
        }

        public SubmodelSupplementalSemanticIdRelationship(Submodel source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelSupplementalSemanticIdRelationship);
        }

        public bool Equals(SubmodelSupplementalSemanticIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SubmodelSupplementalSemanticIdRelationship? left, SubmodelSupplementalSemanticIdRelationship? right)
        {
            return EqualityComparer<SubmodelSupplementalSemanticIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelSupplementalSemanticIdRelationship? left, SubmodelSupplementalSemanticIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubmodelSupplementalSemanticIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}