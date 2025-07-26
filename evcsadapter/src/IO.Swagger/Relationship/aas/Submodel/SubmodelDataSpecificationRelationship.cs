namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelDataSpecificationRelationship : Relationship<Reference>, IEquatable<SubmodelDataSpecificationRelationship>
    {
        public SubmodelDataSpecificationRelationship()
        {
            Name = "dataSpecification";
        }

        public SubmodelDataSpecificationRelationship(Submodel source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelDataSpecificationRelationship);
        }

        public bool Equals(SubmodelDataSpecificationRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SubmodelDataSpecificationRelationship? left, SubmodelDataSpecificationRelationship? right)
        {
            return EqualityComparer<SubmodelDataSpecificationRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelDataSpecificationRelationship? left, SubmodelDataSpecificationRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubmodelDataSpecificationRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}