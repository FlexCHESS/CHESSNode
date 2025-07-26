namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ExtensionRefersToRelationship : Relationship<Referable>, IEquatable<ExtensionRefersToRelationship>
    {
        public ExtensionRefersToRelationship()
        {
            Name = "refersTo";
        }

        public ExtensionRefersToRelationship(Extension source, Referable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ExtensionRefersToRelationship);
        }

        public bool Equals(ExtensionRefersToRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(ExtensionRefersToRelationship? left, ExtensionRefersToRelationship? right)
        {
            return EqualityComparer<ExtensionRefersToRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(ExtensionRefersToRelationship? left, ExtensionRefersToRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as ExtensionRefersToRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}