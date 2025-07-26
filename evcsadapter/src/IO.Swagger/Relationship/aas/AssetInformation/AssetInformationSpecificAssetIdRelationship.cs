namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AssetInformationSpecificAssetIdRelationship : Relationship<SpecificAssetId>, IEquatable<AssetInformationSpecificAssetIdRelationship>
    {
        public AssetInformationSpecificAssetIdRelationship()
        {
            Name = "specificAssetId";
        }

        public AssetInformationSpecificAssetIdRelationship(AssetInformation source, SpecificAssetId target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AssetInformationSpecificAssetIdRelationship);
        }

        public bool Equals(AssetInformationSpecificAssetIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AssetInformationSpecificAssetIdRelationship? left, AssetInformationSpecificAssetIdRelationship? right)
        {
            return EqualityComparer<AssetInformationSpecificAssetIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AssetInformationSpecificAssetIdRelationship? left, AssetInformationSpecificAssetIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AssetInformationSpecificAssetIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}