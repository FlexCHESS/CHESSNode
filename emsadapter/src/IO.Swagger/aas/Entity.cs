namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Entity : SubmodelElement, IEquatable<Entity>
    {
        public Entity()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Entity;1";
        [JsonPropertyName("entityType")]
        public EntityEntityType? EntityType { get; set; }
        [JsonIgnore]
        public EntityStatementRelationshipCollection Statement { get; set; } = new EntityStatementRelationshipCollection();
        [JsonIgnore]
        public EntityGlobalAssetIdRelationshipCollection GlobalAssetId { get; set; } = new EntityGlobalAssetIdRelationshipCollection();
        [JsonIgnore]
        public EntitySpecificAssetIdRelationshipCollection SpecificAssetId { get; set; } = new EntitySpecificAssetIdRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Entity);
        }

        public bool Equals(Entity? other)
        {
            return other is not null && base.Equals(other) && EntityType == other.EntityType;
        }

        public static bool operator ==(Entity? left, Entity? right)
        {
            return EqualityComparer<Entity?>.Default.Equals(left, right);
        }

        public static bool operator !=(Entity? left, Entity? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), EntityType?.GetHashCode());
        }
    }
}