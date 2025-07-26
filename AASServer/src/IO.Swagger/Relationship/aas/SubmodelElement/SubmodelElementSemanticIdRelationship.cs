namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelElementSemanticIdRelationship : Relationship<Reference>, IEquatable<SubmodelElementSemanticIdRelationship>
    {
        public SubmodelElementSemanticIdRelationship()
        {
            Name = "semanticId";
        }

        public SubmodelElementSemanticIdRelationship(SubmodelElement source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelElementSemanticIdRelationship);
        }

        public bool Equals(SubmodelElementSemanticIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SubmodelElementSemanticIdRelationship? left, SubmodelElementSemanticIdRelationship? right)
        {
            return EqualityComparer<SubmodelElementSemanticIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelElementSemanticIdRelationship? left, SubmodelElementSemanticIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubmodelElementSemanticIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}