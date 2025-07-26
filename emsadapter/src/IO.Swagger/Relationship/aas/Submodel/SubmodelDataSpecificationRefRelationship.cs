namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelDataSpecificationRefRelationship : Relationship<DataSpecification>, IEquatable<SubmodelDataSpecificationRefRelationship>
    {
        public SubmodelDataSpecificationRefRelationship()
        {
            Name = "dataSpecificationRef";
        }

        public SubmodelDataSpecificationRefRelationship(Submodel source, DataSpecification target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelDataSpecificationRefRelationship);
        }

        public bool Equals(SubmodelDataSpecificationRefRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SubmodelDataSpecificationRefRelationship? left, SubmodelDataSpecificationRefRelationship? right)
        {
            return EqualityComparer<SubmodelDataSpecificationRefRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelDataSpecificationRefRelationship? left, SubmodelDataSpecificationRefRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubmodelDataSpecificationRefRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}