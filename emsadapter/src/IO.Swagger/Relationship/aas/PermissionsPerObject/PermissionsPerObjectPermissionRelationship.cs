namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class PermissionsPerObjectPermissionRelationship : Relationship<Permission>, IEquatable<PermissionsPerObjectPermissionRelationship>
    {
        public PermissionsPerObjectPermissionRelationship()
        {
            Name = "permission";
        }

        public PermissionsPerObjectPermissionRelationship(PermissionsPerObject source, Permission target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PermissionsPerObjectPermissionRelationship);
        }

        public bool Equals(PermissionsPerObjectPermissionRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(PermissionsPerObjectPermissionRelationship? left, PermissionsPerObjectPermissionRelationship? right)
        {
            return EqualityComparer<PermissionsPerObjectPermissionRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(PermissionsPerObjectPermissionRelationship? left, PermissionsPerObjectPermissionRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as PermissionsPerObjectPermissionRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}