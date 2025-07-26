namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelSemanticIdRelationship : Relationship<Reference>, IEquatable<SubmodelSemanticIdRelationship>
    {
        public SubmodelSemanticIdRelationship()
        {
            Name = "semanticId";
        }

        public SubmodelSemanticIdRelationship(Submodel source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelSemanticIdRelationship);
        }

        public bool Equals(SubmodelSemanticIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SubmodelSemanticIdRelationship? left, SubmodelSemanticIdRelationship? right)
        {
            return EqualityComparer<SubmodelSemanticIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelSemanticIdRelationship? left, SubmodelSemanticIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubmodelSemanticIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}