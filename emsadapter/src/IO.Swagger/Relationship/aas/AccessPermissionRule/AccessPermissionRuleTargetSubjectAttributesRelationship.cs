namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AccessPermissionRuleTargetSubjectAttributesRelationship : Relationship<SubjectAttributes>, IEquatable<AccessPermissionRuleTargetSubjectAttributesRelationship>
    {
        public AccessPermissionRuleTargetSubjectAttributesRelationship()
        {
            Name = "targetSubjectAttributes";
        }

        public AccessPermissionRuleTargetSubjectAttributesRelationship(AccessPermissionRule source, SubjectAttributes target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AccessPermissionRuleTargetSubjectAttributesRelationship);
        }

        public bool Equals(AccessPermissionRuleTargetSubjectAttributesRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AccessPermissionRuleTargetSubjectAttributesRelationship? left, AccessPermissionRuleTargetSubjectAttributesRelationship? right)
        {
            return EqualityComparer<AccessPermissionRuleTargetSubjectAttributesRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AccessPermissionRuleTargetSubjectAttributesRelationship? left, AccessPermissionRuleTargetSubjectAttributesRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AccessPermissionRuleTargetSubjectAttributesRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}