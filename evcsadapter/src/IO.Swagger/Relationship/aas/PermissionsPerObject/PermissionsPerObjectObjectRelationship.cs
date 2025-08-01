namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class PermissionsPerObjectObjectRelationship : Relationship<Referable>, IEquatable<PermissionsPerObjectObjectRelationship>
    {
        public PermissionsPerObjectObjectRelationship()
        {
            Name = "object";
        }

        public PermissionsPerObjectObjectRelationship(PermissionsPerObject source, Referable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PermissionsPerObjectObjectRelationship);
        }

        public bool Equals(PermissionsPerObjectObjectRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(PermissionsPerObjectObjectRelationship? left, PermissionsPerObjectObjectRelationship? right)
        {
            return EqualityComparer<PermissionsPerObjectObjectRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(PermissionsPerObjectObjectRelationship? left, PermissionsPerObjectObjectRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as PermissionsPerObjectObjectRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}