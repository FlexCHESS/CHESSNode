namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubjectAttributesSubjectAttributeRelationship : Relationship<DataElement>, IEquatable<SubjectAttributesSubjectAttributeRelationship>
    {
        public SubjectAttributesSubjectAttributeRelationship()
        {
            Name = "subjectAttribute";
        }

        public SubjectAttributesSubjectAttributeRelationship(SubjectAttributes source, DataElement target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as SubjectAttributesSubjectAttributeRelationship);
        }

        public bool Equals(SubjectAttributesSubjectAttributeRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(SubjectAttributesSubjectAttributeRelationship? left, SubjectAttributesSubjectAttributeRelationship? right)
        {
            return EqualityComparer<SubjectAttributesSubjectAttributeRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubjectAttributesSubjectAttributeRelationship? left, SubjectAttributesSubjectAttributeRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubjectAttributesSubjectAttributeRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}