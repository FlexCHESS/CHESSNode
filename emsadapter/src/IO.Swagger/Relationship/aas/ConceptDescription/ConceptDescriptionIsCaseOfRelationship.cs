namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ConceptDescriptionIsCaseOfRelationship : Relationship<Reference>, IEquatable<ConceptDescriptionIsCaseOfRelationship>
    {
        public ConceptDescriptionIsCaseOfRelationship()
        {
            Name = "isCaseOf";
        }

        public ConceptDescriptionIsCaseOfRelationship(ConceptDescription source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ConceptDescriptionIsCaseOfRelationship);
        }

        public bool Equals(ConceptDescriptionIsCaseOfRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(ConceptDescriptionIsCaseOfRelationship? left, ConceptDescriptionIsCaseOfRelationship? right)
        {
            return EqualityComparer<ConceptDescriptionIsCaseOfRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(ConceptDescriptionIsCaseOfRelationship? left, ConceptDescriptionIsCaseOfRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as ConceptDescriptionIsCaseOfRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}