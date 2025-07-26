namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AccessControl : BasicDigitalTwin, IEquatable<AccessControl>, IEquatable<BasicDigitalTwin>
    {
        public AccessControl()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:AccessControl;1";
        [JsonIgnore]
        public AccessControlAccessPermissionRuleRelationshipCollection AccessPermissionRule { get; set; } = new AccessControlAccessPermissionRuleRelationshipCollection();
        [JsonIgnore]
        public AccessControlSelectableSubjectAttributesRelationshipCollection SelectableSubjectAttributes { get; set; } = new AccessControlSelectableSubjectAttributesRelationshipCollection();
        [JsonIgnore]
        public AccessControlDefaultSubjectAttributesRelationshipCollection DefaultSubjectAttributes { get; set; } = new AccessControlDefaultSubjectAttributesRelationshipCollection();
        [JsonIgnore]
        public AccessControlSelectablePermissionsRelationshipCollection SelectablePermissions { get; set; } = new AccessControlSelectablePermissionsRelationshipCollection();
        [JsonIgnore]
        public AccessControlDefaultPermissionsRelationshipCollection DefaultPermissions { get; set; } = new AccessControlDefaultPermissionsRelationshipCollection();
        [JsonIgnore]
        public AccessControlSelectableEnvironmentAttributesRelationshipCollection SelectableEnvironmentAttributes { get; set; } = new AccessControlSelectableEnvironmentAttributesRelationshipCollection();
        [JsonIgnore]
        public AccessControlDefaultEnvironmentAttributesRelationshipCollection DefaultEnvironmentAttributes { get; set; } = new AccessControlDefaultEnvironmentAttributesRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as AccessControl);
        }

        public bool Equals(AccessControl? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(AccessControl? left, AccessControl? right)
        {
            return EqualityComparer<AccessControl?>.Default.Equals(left, right);
        }

        public static bool operator !=(AccessControl? left, AccessControl? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as AccessControl) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}