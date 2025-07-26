namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SpecificAssetIdExternalSubjectIdRelationship : Relationship<Reference>, IEquatable<SpecificAssetIdExternalSubjectIdRelationship>
    {
        public SpecificAssetIdExternalSubjectIdRelationship()
        {
            Name = "externalSubjectId";
        }

        public SpecificAssetIdExternalSubjectIdRelationship(SpecificAssetId source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SpecificAssetIdExternalSubjectIdRelationship);
        }

        public bool Equals(SpecificAssetIdExternalSubjectIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SpecificAssetIdExternalSubjectIdRelationship? left, SpecificAssetIdExternalSubjectIdRelationship? right)
        {
            return EqualityComparer<SpecificAssetIdExternalSubjectIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SpecificAssetIdExternalSubjectIdRelationship? left, SpecificAssetIdExternalSubjectIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SpecificAssetIdExternalSubjectIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}