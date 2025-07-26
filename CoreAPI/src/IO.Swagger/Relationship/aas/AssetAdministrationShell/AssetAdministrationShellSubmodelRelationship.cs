namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AssetAdministrationShellSubmodelRelationship : Relationship<Submodel>, IEquatable<AssetAdministrationShellSubmodelRelationship>
    {
        public AssetAdministrationShellSubmodelRelationship()
        {
            Name = "submodel";
        }

        public AssetAdministrationShellSubmodelRelationship(AssetAdministrationShell source, Submodel target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AssetAdministrationShellSubmodelRelationship);
        }

        public bool Equals(AssetAdministrationShellSubmodelRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AssetAdministrationShellSubmodelRelationship? left, AssetAdministrationShellSubmodelRelationship? right)
        {
            return EqualityComparer<AssetAdministrationShellSubmodelRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AssetAdministrationShellSubmodelRelationship? left, AssetAdministrationShellSubmodelRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AssetAdministrationShellSubmodelRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}