namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EnvironmentAssetAdministrationShellRelationship : Relationship<AssetAdministrationShell>, IEquatable<EnvironmentAssetAdministrationShellRelationship>
    {
        public EnvironmentAssetAdministrationShellRelationship()
        {
            Name = "assetAdministrationShell";
        }

        public EnvironmentAssetAdministrationShellRelationship(Environment source, AssetAdministrationShell target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EnvironmentAssetAdministrationShellRelationship);
        }

        public bool Equals(EnvironmentAssetAdministrationShellRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EnvironmentAssetAdministrationShellRelationship? left, EnvironmentAssetAdministrationShellRelationship? right)
        {
            return EqualityComparer<EnvironmentAssetAdministrationShellRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EnvironmentAssetAdministrationShellRelationship? left, EnvironmentAssetAdministrationShellRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EnvironmentAssetAdministrationShellRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}