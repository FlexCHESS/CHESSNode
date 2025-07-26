namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class HasDataSpecificationDataSpecificationRefRelationship : Relationship<DataSpecification>, IEquatable<HasDataSpecificationDataSpecificationRefRelationship>
    {
        public HasDataSpecificationDataSpecificationRefRelationship()
        {
            Name = "dataSpecificationRef";
        }

        public HasDataSpecificationDataSpecificationRefRelationship(HasDataSpecification source, DataSpecification target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as HasDataSpecificationDataSpecificationRefRelationship);
        }

        public bool Equals(HasDataSpecificationDataSpecificationRefRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(HasDataSpecificationDataSpecificationRefRelationship? left, HasDataSpecificationDataSpecificationRefRelationship? right)
        {
            return EqualityComparer<HasDataSpecificationDataSpecificationRefRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(HasDataSpecificationDataSpecificationRefRelationship? left, HasDataSpecificationDataSpecificationRefRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as HasDataSpecificationDataSpecificationRefRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}