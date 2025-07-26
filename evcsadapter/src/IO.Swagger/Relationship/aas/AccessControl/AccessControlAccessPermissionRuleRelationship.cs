namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AccessControlAccessPermissionRuleRelationship : Relationship<AccessPermissionRule>, IEquatable<AccessControlAccessPermissionRuleRelationship>
    {
        public AccessControlAccessPermissionRuleRelationship()
        {
            Name = "accessPermissionRule";
        }

        public AccessControlAccessPermissionRuleRelationship(AccessControl source, AccessPermissionRule target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AccessControlAccessPermissionRuleRelationship);
        }

        public bool Equals(AccessControlAccessPermissionRuleRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AccessControlAccessPermissionRuleRelationship? left, AccessControlAccessPermissionRuleRelationship? right)
        {
            return EqualityComparer<AccessControlAccessPermissionRuleRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AccessControlAccessPermissionRuleRelationship? left, AccessControlAccessPermissionRuleRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AccessControlAccessPermissionRuleRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}