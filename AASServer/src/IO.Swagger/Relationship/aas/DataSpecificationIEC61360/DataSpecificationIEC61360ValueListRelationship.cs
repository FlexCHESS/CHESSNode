namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class DataSpecificationIEC61360ValueListRelationship : Relationship<ValueList>, IEquatable<DataSpecificationIEC61360ValueListRelationship>
    {
        public DataSpecificationIEC61360ValueListRelationship()
        {
            Name = "valueList";
        }

        public DataSpecificationIEC61360ValueListRelationship(DataSpecificationIEC61360 source, ValueList target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DataSpecificationIEC61360ValueListRelationship);
        }

        public bool Equals(DataSpecificationIEC61360ValueListRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(DataSpecificationIEC61360ValueListRelationship? left, DataSpecificationIEC61360ValueListRelationship? right)
        {
            return EqualityComparer<DataSpecificationIEC61360ValueListRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(DataSpecificationIEC61360ValueListRelationship? left, DataSpecificationIEC61360ValueListRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as DataSpecificationIEC61360ValueListRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}