namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AssetAdministrationShell : Identifiable, IEquatable<AssetAdministrationShell>
    {
        public AssetAdministrationShell()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:AssetAdministrationShell;1";
        [JsonIgnore]
        public AssetAdministrationShellDerivedFromRelationshipCollection DerivedFrom { get; set; } = new AssetAdministrationShellDerivedFromRelationshipCollection();
        [JsonIgnore]
        public AssetAdministrationShellAssetInformationRelationshipCollection AssetInformation { get; set; } = new AssetAdministrationShellAssetInformationRelationshipCollection();
        [JsonIgnore]
        public AssetAdministrationShellSubmodelRelationshipCollection Submodel { get; set; } = new AssetAdministrationShellSubmodelRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as AssetAdministrationShell);
        }

        public bool Equals(AssetAdministrationShell? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(AssetAdministrationShell? left, AssetAdministrationShell? right)
        {
            return EqualityComparer<AssetAdministrationShell?>.Default.Equals(left, right);
        }

        public static bool operator !=(AssetAdministrationShell? left, AssetAdministrationShell? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}