namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EntityGlobalAssetIdRelationship : Relationship<Reference>, IEquatable<EntityGlobalAssetIdRelationship>
    {
        public EntityGlobalAssetIdRelationship()
        {
            Name = "globalAssetId";
        }

        public EntityGlobalAssetIdRelationship(Entity source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EntityGlobalAssetIdRelationship);
        }

        public bool Equals(EntityGlobalAssetIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EntityGlobalAssetIdRelationship? left, EntityGlobalAssetIdRelationship? right)
        {
            return EqualityComparer<EntityGlobalAssetIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EntityGlobalAssetIdRelationship? left, EntityGlobalAssetIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EntityGlobalAssetIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}