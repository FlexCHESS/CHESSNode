namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelElementCollectionValueRelationship : Relationship<SubmodelElement>, IEquatable<SubmodelElementCollectionValueRelationship>
    {
        public SubmodelElementCollectionValueRelationship()
        {
            Name = "value";
        }

        public SubmodelElementCollectionValueRelationship(SubmodelElementCollection source, SubmodelElement target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelElementCollectionValueRelationship);
        }

        public bool Equals(SubmodelElementCollectionValueRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SubmodelElementCollectionValueRelationship? left, SubmodelElementCollectionValueRelationship? right)
        {
            return EqualityComparer<SubmodelElementCollectionValueRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelElementCollectionValueRelationship? left, SubmodelElementCollectionValueRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubmodelElementCollectionValueRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}