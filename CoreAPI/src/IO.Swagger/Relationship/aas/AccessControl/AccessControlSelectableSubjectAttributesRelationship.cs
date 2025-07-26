namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AccessControlSelectableSubjectAttributesRelationship : Relationship<Submodel>, IEquatable<AccessControlSelectableSubjectAttributesRelationship>
    {
        public AccessControlSelectableSubjectAttributesRelationship()
        {
            Name = "selectableSubjectAttributes";
        }

        public AccessControlSelectableSubjectAttributesRelationship(AccessControl source, Submodel target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AccessControlSelectableSubjectAttributesRelationship);
        }

        public bool Equals(AccessControlSelectableSubjectAttributesRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AccessControlSelectableSubjectAttributesRelationship? left, AccessControlSelectableSubjectAttributesRelationship? right)
        {
            return EqualityComparer<AccessControlSelectableSubjectAttributesRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AccessControlSelectableSubjectAttributesRelationship? left, AccessControlSelectableSubjectAttributesRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AccessControlSelectableSubjectAttributesRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}