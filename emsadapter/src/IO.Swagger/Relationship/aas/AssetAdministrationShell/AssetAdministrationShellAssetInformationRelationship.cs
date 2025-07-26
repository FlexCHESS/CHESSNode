namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AssetAdministrationShellAssetInformationRelationship : Relationship<AssetInformation>, IEquatable<AssetAdministrationShellAssetInformationRelationship>
    {
        public AssetAdministrationShellAssetInformationRelationship()
        {
            Name = "assetInformation";
        }

        public AssetAdministrationShellAssetInformationRelationship(AssetAdministrationShell source, AssetInformation target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AssetAdministrationShellAssetInformationRelationship);
        }

        public bool Equals(AssetAdministrationShellAssetInformationRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AssetAdministrationShellAssetInformationRelationship? left, AssetAdministrationShellAssetInformationRelationship? right)
        {
            return EqualityComparer<AssetAdministrationShellAssetInformationRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AssetAdministrationShellAssetInformationRelationship? left, AssetAdministrationShellAssetInformationRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AssetAdministrationShellAssetInformationRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}