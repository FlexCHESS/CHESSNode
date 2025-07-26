namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class RelationshipElementFirstModelRefRelationship : Relationship<Referable>, IEquatable<RelationshipElementFirstModelRefRelationship>
    {
        public RelationshipElementFirstModelRefRelationship()
        {
            Name = "firstModelRef";
        }

        public RelationshipElementFirstModelRefRelationship(RelationshipElement source, Referable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as RelationshipElementFirstModelRefRelationship);
        }

        public bool Equals(RelationshipElementFirstModelRefRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(RelationshipElementFirstModelRefRelationship? left, RelationshipElementFirstModelRefRelationship? right)
        {
            return EqualityComparer<RelationshipElementFirstModelRefRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(RelationshipElementFirstModelRefRelationship? left, RelationshipElementFirstModelRefRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as RelationshipElementFirstModelRefRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}