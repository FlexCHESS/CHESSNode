namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class HasDataSpecificationDataSpecificationRelationship : Relationship<Reference>, IEquatable<HasDataSpecificationDataSpecificationRelationship>
    {
        public HasDataSpecificationDataSpecificationRelationship()
        {
            Name = "dataSpecification";
        }

        public HasDataSpecificationDataSpecificationRelationship(HasDataSpecification source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as HasDataSpecificationDataSpecificationRelationship);
        }

        public bool Equals(HasDataSpecificationDataSpecificationRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(HasDataSpecificationDataSpecificationRelationship? left, HasDataSpecificationDataSpecificationRelationship? right)
        {
            return EqualityComparer<HasDataSpecificationDataSpecificationRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(HasDataSpecificationDataSpecificationRelationship? left, HasDataSpecificationDataSpecificationRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as HasDataSpecificationDataSpecificationRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}