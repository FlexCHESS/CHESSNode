namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EntityStatementRelationship : Relationship<SubmodelElement>, IEquatable<EntityStatementRelationship>
    {
        public EntityStatementRelationship()
        {
            Name = "statement";
        }

        public EntityStatementRelationship(Entity source, SubmodelElement target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EntityStatementRelationship);
        }

        public bool Equals(EntityStatementRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EntityStatementRelationship? left, EntityStatementRelationship? right)
        {
            return EqualityComparer<EntityStatementRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EntityStatementRelationship? left, EntityStatementRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EntityStatementRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}