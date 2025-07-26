namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AssetInformationDefaultThumbnailRelationship : Relationship<Resource>, IEquatable<AssetInformationDefaultThumbnailRelationship>
    {
        public AssetInformationDefaultThumbnailRelationship()
        {
            Name = "defaultThumbnail";
        }

        public AssetInformationDefaultThumbnailRelationship(AssetInformation source, Resource target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AssetInformationDefaultThumbnailRelationship);
        }

        public bool Equals(AssetInformationDefaultThumbnailRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AssetInformationDefaultThumbnailRelationship? left, AssetInformationDefaultThumbnailRelationship? right)
        {
            return EqualityComparer<AssetInformationDefaultThumbnailRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AssetInformationDefaultThumbnailRelationship? left, AssetInformationDefaultThumbnailRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AssetInformationDefaultThumbnailRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}