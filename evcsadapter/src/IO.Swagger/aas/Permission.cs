namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Permission : BasicDigitalTwin, IEquatable<Permission>, IEquatable<BasicDigitalTwin>
    {
        public Permission()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:Permission;1";
        [JsonPropertyName("kindOfPermission")]
        public PermissionKindOfPermission? KindOfPermission { get; set; }
        [JsonIgnore]
        public PermissionPermissionRelationshipCollection PermissionCollection { get; set; } = new PermissionPermissionRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Permission);
        }

        public bool Equals(Permission? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && KindOfPermission == other.KindOfPermission;
        }

        public static bool operator ==(Permission? left, Permission? right)
        {
            return EqualityComparer<Permission?>.Default.Equals(left, right);
        }

        public static bool operator !=(Permission? left, Permission? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), KindOfPermission?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Permission) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}