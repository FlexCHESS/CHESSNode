namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class HasSemanticsSemanticIdRelationship : Relationship<Reference>, IEquatable<HasSemanticsSemanticIdRelationship>
    {
        public HasSemanticsSemanticIdRelationship()
        {
            Name = "semanticId";
        }

        public HasSemanticsSemanticIdRelationship(HasSemantics source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as HasSemanticsSemanticIdRelationship);
        }

        public bool Equals(HasSemanticsSemanticIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(HasSemanticsSemanticIdRelationship? left, HasSemanticsSemanticIdRelationship? right)
        {
            return EqualityComparer<HasSemanticsSemanticIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(HasSemanticsSemanticIdRelationship? left, HasSemanticsSemanticIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as HasSemanticsSemanticIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}