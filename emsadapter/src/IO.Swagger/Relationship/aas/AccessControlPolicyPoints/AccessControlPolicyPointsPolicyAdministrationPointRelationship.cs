namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AccessControlPolicyPointsPolicyAdministrationPointRelationship : Relationship<AccessControl>, IEquatable<AccessControlPolicyPointsPolicyAdministrationPointRelationship>
    {
        public AccessControlPolicyPointsPolicyAdministrationPointRelationship()
        {
            Name = "policyAdministrationPoint";
        }

        public AccessControlPolicyPointsPolicyAdministrationPointRelationship(AccessControlPolicyPoints source, AccessControl target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AccessControlPolicyPointsPolicyAdministrationPointRelationship);
        }

        public bool Equals(AccessControlPolicyPointsPolicyAdministrationPointRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AccessControlPolicyPointsPolicyAdministrationPointRelationship? left, AccessControlPolicyPointsPolicyAdministrationPointRelationship? right)
        {
            return EqualityComparer<AccessControlPolicyPointsPolicyAdministrationPointRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AccessControlPolicyPointsPolicyAdministrationPointRelationship? left, AccessControlPolicyPointsPolicyAdministrationPointRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AccessControlPolicyPointsPolicyAdministrationPointRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}