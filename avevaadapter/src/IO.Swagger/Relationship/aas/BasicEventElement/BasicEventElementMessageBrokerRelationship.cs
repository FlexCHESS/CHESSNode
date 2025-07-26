namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class BasicEventElementMessageBrokerRelationship : Relationship<Referable>, IEquatable<BasicEventElementMessageBrokerRelationship>
    {
        public BasicEventElementMessageBrokerRelationship()
        {
            Name = "messageBroker";
        }

        public BasicEventElementMessageBrokerRelationship(BasicEventElement source, Referable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as BasicEventElementMessageBrokerRelationship);
        }

        public bool Equals(BasicEventElementMessageBrokerRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(BasicEventElementMessageBrokerRelationship? left, BasicEventElementMessageBrokerRelationship? right)
        {
            return EqualityComparer<BasicEventElementMessageBrokerRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(BasicEventElementMessageBrokerRelationship? left, BasicEventElementMessageBrokerRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as BasicEventElementMessageBrokerRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}