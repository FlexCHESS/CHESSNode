namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class PermissionsPerObjectTargetObjectAttributesRelationship : Relationship<ObjectAttributes>, IEquatable<PermissionsPerObjectTargetObjectAttributesRelationship>
    {
        public PermissionsPerObjectTargetObjectAttributesRelationship()
        {
            Name = "targetObjectAttributes";
        }

        public PermissionsPerObjectTargetObjectAttributesRelationship(PermissionsPerObject source, ObjectAttributes target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PermissionsPerObjectTargetObjectAttributesRelationship);
        }

        public bool Equals(PermissionsPerObjectTargetObjectAttributesRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(PermissionsPerObjectTargetObjectAttributesRelationship? left, PermissionsPerObjectTargetObjectAttributesRelationship? right)
        {
            return EqualityComparer<PermissionsPerObjectTargetObjectAttributesRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(PermissionsPerObjectTargetObjectAttributesRelationship? left, PermissionsPerObjectTargetObjectAttributesRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as PermissionsPerObjectTargetObjectAttributesRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}