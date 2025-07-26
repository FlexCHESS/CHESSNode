namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ObjectAttributesObjectAttributeRelationship : Relationship<DataElement>, IEquatable<ObjectAttributesObjectAttributeRelationship>
    {
        public ObjectAttributesObjectAttributeRelationship()
        {
            Name = "objectAttribute";
        }

        public ObjectAttributesObjectAttributeRelationship(ObjectAttributes source, DataElement target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ObjectAttributesObjectAttributeRelationship);
        }

        public bool Equals(ObjectAttributesObjectAttributeRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(ObjectAttributesObjectAttributeRelationship? left, ObjectAttributesObjectAttributeRelationship? right)
        {
            return EqualityComparer<ObjectAttributesObjectAttributeRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(ObjectAttributesObjectAttributeRelationship? left, ObjectAttributesObjectAttributeRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as ObjectAttributesObjectAttributeRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}