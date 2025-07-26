namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AccessPermissionRuleConstraintRelationship : Relationship<Formula>, IEquatable<AccessPermissionRuleConstraintRelationship>
    {
        public AccessPermissionRuleConstraintRelationship()
        {
            Name = "constraint";
        }

        public AccessPermissionRuleConstraintRelationship(AccessPermissionRule source, Formula target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AccessPermissionRuleConstraintRelationship);
        }

        public bool Equals(AccessPermissionRuleConstraintRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AccessPermissionRuleConstraintRelationship? left, AccessPermissionRuleConstraintRelationship? right)
        {
            return EqualityComparer<AccessPermissionRuleConstraintRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AccessPermissionRuleConstraintRelationship? left, AccessPermissionRuleConstraintRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AccessPermissionRuleConstraintRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}