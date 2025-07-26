namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelElementListSemanticIdListElementRelationship : Relationship<Reference>, IEquatable<SubmodelElementListSemanticIdListElementRelationship>
    {
        public SubmodelElementListSemanticIdListElementRelationship()
        {
            Name = "semanticIdListElement";
        }

        public SubmodelElementListSemanticIdListElementRelationship(SubmodelElementList source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelElementListSemanticIdListElementRelationship);
        }

        public bool Equals(SubmodelElementListSemanticIdListElementRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SubmodelElementListSemanticIdListElementRelationship? left, SubmodelElementListSemanticIdListElementRelationship? right)
        {
            return EqualityComparer<SubmodelElementListSemanticIdListElementRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelElementListSemanticIdListElementRelationship? left, SubmodelElementListSemanticIdListElementRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubmodelElementListSemanticIdListElementRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}