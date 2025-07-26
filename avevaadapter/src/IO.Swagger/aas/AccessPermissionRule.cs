namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AccessPermissionRule : BasicDigitalTwin, IEquatable<AccessPermissionRule>, IEquatable<BasicDigitalTwin>
    {
        public AccessPermissionRule()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:AccessPermissionRule;1";
        [JsonIgnore]
        public AccessPermissionRuleTargetSubjectAttributesRelationshipCollection TargetSubjectAttributes { get; set; } = new AccessPermissionRuleTargetSubjectAttributesRelationshipCollection();
        [JsonIgnore]
        public AccessPermissionRulePermissionsPerObjectRelationshipCollection PermissionsPerObject { get; set; } = new AccessPermissionRulePermissionsPerObjectRelationshipCollection();
        [JsonIgnore]
        public AccessPermissionRuleConstraintRelationshipCollection Constraint { get; set; } = new AccessPermissionRuleConstraintRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as AccessPermissionRule);
        }

        public bool Equals(AccessPermissionRule? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(AccessPermissionRule? left, AccessPermissionRule? right)
        {
            return EqualityComparer<AccessPermissionRule?>.Default.Equals(left, right);
        }

        public static bool operator !=(AccessPermissionRule? left, AccessPermissionRule? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as AccessPermissionRule) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}