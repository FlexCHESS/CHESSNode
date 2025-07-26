namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class QualifiableQualifierRelationship : Relationship<Qualifier>, IEquatable<QualifiableQualifierRelationship>
    {
        public QualifiableQualifierRelationship()
        {
            Name = "qualifier";
        }

        public QualifiableQualifierRelationship(Qualifiable source, Qualifier target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as QualifiableQualifierRelationship);
        }

        public bool Equals(QualifiableQualifierRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(QualifiableQualifierRelationship? left, QualifiableQualifierRelationship? right)
        {
            return EqualityComparer<QualifiableQualifierRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(QualifiableQualifierRelationship? left, QualifiableQualifierRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as QualifiableQualifierRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}