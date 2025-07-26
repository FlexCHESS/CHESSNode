namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AccessControlSelectableEnvironmentAttributesRelationship : Relationship<Submodel>, IEquatable<AccessControlSelectableEnvironmentAttributesRelationship>
    {
        public AccessControlSelectableEnvironmentAttributesRelationship()
        {
            Name = "selectableEnvironmentAttributes";
        }

        public AccessControlSelectableEnvironmentAttributesRelationship(AccessControl source, Submodel target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AccessControlSelectableEnvironmentAttributesRelationship);
        }

        public bool Equals(AccessControlSelectableEnvironmentAttributesRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AccessControlSelectableEnvironmentAttributesRelationship? left, AccessControlSelectableEnvironmentAttributesRelationship? right)
        {
            return EqualityComparer<AccessControlSelectableEnvironmentAttributesRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AccessControlSelectableEnvironmentAttributesRelationship? left, AccessControlSelectableEnvironmentAttributesRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AccessControlSelectableEnvironmentAttributesRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}