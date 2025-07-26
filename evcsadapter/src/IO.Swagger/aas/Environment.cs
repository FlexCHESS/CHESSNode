namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Environment : Reference, IEquatable<Environment>
    {
        public Environment()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Environment;1";
        [JsonIgnore]
        public EnvironmentAssetAdministrationShellRelationshipCollection AssetAdministrationShell { get; set; } = new EnvironmentAssetAdministrationShellRelationshipCollection();
        [JsonIgnore]
        public EnvironmentSubmodelRelationshipCollection Submodel { get; set; } = new EnvironmentSubmodelRelationshipCollection();
        [JsonIgnore]
        public EnvironmentConceptDescriptionRelationshipCollection ConceptDescription { get; set; } = new EnvironmentConceptDescriptionRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Environment);
        }

        public bool Equals(Environment? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(Environment? left, Environment? right)
        {
            return EqualityComparer<Environment?>.Default.Equals(left, right);
        }

        public static bool operator !=(Environment? left, Environment? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}