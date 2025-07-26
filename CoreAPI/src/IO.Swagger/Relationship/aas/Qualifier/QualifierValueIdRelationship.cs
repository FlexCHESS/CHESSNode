namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class QualifierValueIdRelationship : Relationship<Reference>, IEquatable<QualifierValueIdRelationship>
    {
        public QualifierValueIdRelationship()
        {
            Name = "valueId";
        }

        public QualifierValueIdRelationship(Qualifier source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as QualifierValueIdRelationship);
        }

        public bool Equals(QualifierValueIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(QualifierValueIdRelationship? left, QualifierValueIdRelationship? right)
        {
            return EqualityComparer<QualifierValueIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(QualifierValueIdRelationship? left, QualifierValueIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as QualifierValueIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}