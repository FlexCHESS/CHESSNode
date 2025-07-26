namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelElementDataSpecificationRelationship : Relationship<Reference>, IEquatable<SubmodelElementDataSpecificationRelationship>
    {
        public SubmodelElementDataSpecificationRelationship()
        {
            Name = "dataSpecification";
        }

        public SubmodelElementDataSpecificationRelationship(SubmodelElement source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelElementDataSpecificationRelationship);
        }

        public bool Equals(SubmodelElementDataSpecificationRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SubmodelElementDataSpecificationRelationship? left, SubmodelElementDataSpecificationRelationship? right)
        {
            return EqualityComparer<SubmodelElementDataSpecificationRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelElementDataSpecificationRelationship? left, SubmodelElementDataSpecificationRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubmodelElementDataSpecificationRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}