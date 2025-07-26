namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class PermissionsPerObject : BasicDigitalTwin, IEquatable<PermissionsPerObject>, IEquatable<BasicDigitalTwin>
    {
        public PermissionsPerObject()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:PermissionsPerObject;1";
        [JsonIgnore]
        public PermissionsPerObjectObjectRelationshipCollection Object { get; set; } = new PermissionsPerObjectObjectRelationshipCollection();
        [JsonIgnore]
        public PermissionsPerObjectTargetObjectAttributesRelationshipCollection TargetObjectAttributes { get; set; } = new PermissionsPerObjectTargetObjectAttributesRelationshipCollection();
        [JsonIgnore]
        public PermissionsPerObjectPermissionRelationshipCollection Permission { get; set; } = new PermissionsPerObjectPermissionRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as PermissionsPerObject);
        }

        public bool Equals(PermissionsPerObject? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(PermissionsPerObject? left, PermissionsPerObject? right)
        {
            return EqualityComparer<PermissionsPerObject?>.Default.Equals(left, right);
        }

        public static bool operator !=(PermissionsPerObject? left, PermissionsPerObject? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as PermissionsPerObject) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}