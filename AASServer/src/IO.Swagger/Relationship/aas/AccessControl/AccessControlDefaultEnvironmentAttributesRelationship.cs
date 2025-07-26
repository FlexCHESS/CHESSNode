namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AccessControlDefaultEnvironmentAttributesRelationship : Relationship<Submodel>, IEquatable<AccessControlDefaultEnvironmentAttributesRelationship>
    {
        public AccessControlDefaultEnvironmentAttributesRelationship()
        {
            Name = "defaultEnvironmentAttributes";
        }

        public AccessControlDefaultEnvironmentAttributesRelationship(AccessControl source, Submodel target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AccessControlDefaultEnvironmentAttributesRelationship);
        }

        public bool Equals(AccessControlDefaultEnvironmentAttributesRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AccessControlDefaultEnvironmentAttributesRelationship? left, AccessControlDefaultEnvironmentAttributesRelationship? right)
        {
            return EqualityComparer<AccessControlDefaultEnvironmentAttributesRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AccessControlDefaultEnvironmentAttributesRelationship? left, AccessControlDefaultEnvironmentAttributesRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AccessControlDefaultEnvironmentAttributesRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}