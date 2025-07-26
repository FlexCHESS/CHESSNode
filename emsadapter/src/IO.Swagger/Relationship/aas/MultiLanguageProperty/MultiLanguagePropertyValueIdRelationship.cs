namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class MultiLanguagePropertyValueIdRelationship : Relationship<Reference>, IEquatable<MultiLanguagePropertyValueIdRelationship>
    {
        public MultiLanguagePropertyValueIdRelationship()
        {
            Name = "valueId";
        }

        public MultiLanguagePropertyValueIdRelationship(MultiLanguageProperty source, Reference target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MultiLanguagePropertyValueIdRelationship);
        }

        public bool Equals(MultiLanguagePropertyValueIdRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(MultiLanguagePropertyValueIdRelationship? left, MultiLanguagePropertyValueIdRelationship? right)
        {
            return EqualityComparer<MultiLanguagePropertyValueIdRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(MultiLanguagePropertyValueIdRelationship? left, MultiLanguagePropertyValueIdRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as MultiLanguagePropertyValueIdRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}