namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class RelationshipElementSecondModelRefRelationship : Relationship<Referable>, IEquatable<RelationshipElementSecondModelRefRelationship>
    {
        public RelationshipElementSecondModelRefRelationship()
        {
            Name = "secondModelRef";
        }

        public RelationshipElementSecondModelRefRelationship(RelationshipElement source, Referable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as RelationshipElementSecondModelRefRelationship);
        }

        public bool Equals(RelationshipElementSecondModelRefRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(RelationshipElementSecondModelRefRelationship? left, RelationshipElementSecondModelRefRelationship? right)
        {
            return EqualityComparer<RelationshipElementSecondModelRefRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(RelationshipElementSecondModelRefRelationship? left, RelationshipElementSecondModelRefRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as RelationshipElementSecondModelRefRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}