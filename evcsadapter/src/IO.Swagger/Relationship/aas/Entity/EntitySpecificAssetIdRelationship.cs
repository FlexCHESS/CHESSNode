namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EntitySpecificAssetIdRelationship : Relationship<SpecificAssetId>, IEquatable<EntitySpecificAssetIdRelationship>
    {
        public EntitySpecificAssetIdRelationship()
        {
            Name = "specificAssetId";
        }

        public EntitySpecificAssetIdRelationship(Entity source, SpecificAssetId target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EntitySpecificAssetIdRelationship);
        }

        public bool Equals(EntitySpecificAssetIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EntitySpecificAssetIdRelationship? left, EntitySpecificAssetIdRelationship? right)
        {
            return EqualityComparer<EntitySpecificAssetIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EntitySpecificAssetIdRelationship? left, EntitySpecificAssetIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EntitySpecificAssetIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}