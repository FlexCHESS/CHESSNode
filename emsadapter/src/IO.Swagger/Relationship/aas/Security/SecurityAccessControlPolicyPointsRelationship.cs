namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SecurityAccessControlPolicyPointsRelationship : Relationship<AccessControlPolicyPoints>, IEquatable<SecurityAccessControlPolicyPointsRelationship>
    {
        public SecurityAccessControlPolicyPointsRelationship()
        {
            Name = "accessControlPolicyPoints";
        }

        public SecurityAccessControlPolicyPointsRelationship(Security source, AccessControlPolicyPoints target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SecurityAccessControlPolicyPointsRelationship);
        }

        public bool Equals(SecurityAccessControlPolicyPointsRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SecurityAccessControlPolicyPointsRelationship? left, SecurityAccessControlPolicyPointsRelationship? right)
        {
            return EqualityComparer<SecurityAccessControlPolicyPointsRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SecurityAccessControlPolicyPointsRelationship? left, SecurityAccessControlPolicyPointsRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SecurityAccessControlPolicyPointsRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}