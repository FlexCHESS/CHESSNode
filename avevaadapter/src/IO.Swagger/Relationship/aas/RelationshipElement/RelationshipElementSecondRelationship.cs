namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class RelationshipElementSecondRelationship : Relationship<Reference>, IEquatable<RelationshipElementSecondRelationship>
    {
        public RelationshipElementSecondRelationship()
        {
            Name = "second";
        }

        public RelationshipElementSecondRelationship(RelationshipElement source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as RelationshipElementSecondRelationship);
        }

        public bool Equals(RelationshipElementSecondRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(RelationshipElementSecondRelationship? left, RelationshipElementSecondRelationship? right)
        {
            return EqualityComparer<RelationshipElementSecondRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(RelationshipElementSecondRelationship? left, RelationshipElementSecondRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as RelationshipElementSecondRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}