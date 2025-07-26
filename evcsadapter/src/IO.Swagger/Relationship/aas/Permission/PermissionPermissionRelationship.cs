namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class PermissionPermissionRelationship : Relationship<Property>, IEquatable<PermissionPermissionRelationship>
    {
        public PermissionPermissionRelationship()
        {
            Name = "permission";
        }

        public PermissionPermissionRelationship(Permission source, Property target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PermissionPermissionRelationship);
        }

        public bool Equals(PermissionPermissionRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(PermissionPermissionRelationship? left, PermissionPermissionRelationship? right)
        {
            return EqualityComparer<PermissionPermissionRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(PermissionPermissionRelationship? left, PermissionPermissionRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as PermissionPermissionRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}