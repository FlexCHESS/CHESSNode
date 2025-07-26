namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class PropertyValueIdRelationship : Relationship<Reference>, IEquatable<PropertyValueIdRelationship>
    {
        public PropertyValueIdRelationship()
        {
            Name = "valueId";
        }

        public PropertyValueIdRelationship(Property source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PropertyValueIdRelationship);
        }

        public bool Equals(PropertyValueIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(PropertyValueIdRelationship? left, PropertyValueIdRelationship? right)
        {
            return EqualityComparer<PropertyValueIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(PropertyValueIdRelationship? left, PropertyValueIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as PropertyValueIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}