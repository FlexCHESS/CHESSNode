namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class BasicEventElementObservedRelationship : Relationship<Referable>, IEquatable<BasicEventElementObservedRelationship>
    {
        public BasicEventElementObservedRelationship()
        {
            Name = "observed";
        }

        public BasicEventElementObservedRelationship(BasicEventElement source, Referable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BasicEventElementObservedRelationship);
        }

        public bool Equals(BasicEventElementObservedRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(BasicEventElementObservedRelationship? left, BasicEventElementObservedRelationship? right)
        {
            return EqualityComparer<BasicEventElementObservedRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(BasicEventElementObservedRelationship? left, BasicEventElementObservedRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as BasicEventElementObservedRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}