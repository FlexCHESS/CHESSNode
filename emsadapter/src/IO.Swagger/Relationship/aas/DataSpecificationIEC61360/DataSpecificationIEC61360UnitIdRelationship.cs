namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class DataSpecificationIEC61360UnitIdRelationship : Relationship<Reference>, IEquatable<DataSpecificationIEC61360UnitIdRelationship>
    {
        public DataSpecificationIEC61360UnitIdRelationship()
        {
            Name = "unitId";
        }

        public DataSpecificationIEC61360UnitIdRelationship(DataSpecificationIEC61360 source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DataSpecificationIEC61360UnitIdRelationship);
        }

        public bool Equals(DataSpecificationIEC61360UnitIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(DataSpecificationIEC61360UnitIdRelationship? left, DataSpecificationIEC61360UnitIdRelationship? right)
        {
            return EqualityComparer<DataSpecificationIEC61360UnitIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(DataSpecificationIEC61360UnitIdRelationship? left, DataSpecificationIEC61360UnitIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as DataSpecificationIEC61360UnitIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}