namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelElementDataSpecificationRefRelationship : Relationship<DataSpecification>, IEquatable<SubmodelElementDataSpecificationRefRelationship>
    {
        public SubmodelElementDataSpecificationRefRelationship()
        {
            Name = "dataSpecificationRef";
        }

        public SubmodelElementDataSpecificationRefRelationship(SubmodelElement source, DataSpecification target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelElementDataSpecificationRefRelationship);
        }

        public bool Equals(SubmodelElementDataSpecificationRefRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SubmodelElementDataSpecificationRefRelationship? left, SubmodelElementDataSpecificationRefRelationship? right)
        {
            return EqualityComparer<SubmodelElementDataSpecificationRefRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelElementDataSpecificationRefRelationship? left, SubmodelElementDataSpecificationRefRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubmodelElementDataSpecificationRefRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}