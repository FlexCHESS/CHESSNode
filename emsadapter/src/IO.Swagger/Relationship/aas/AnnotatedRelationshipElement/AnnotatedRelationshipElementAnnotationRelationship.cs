namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AnnotatedRelationshipElementAnnotationRelationship : Relationship<DataElement>, IEquatable<AnnotatedRelationshipElementAnnotationRelationship>
    {
        public AnnotatedRelationshipElementAnnotationRelationship()
        {
            Name = "annotation";
        }

        public AnnotatedRelationshipElementAnnotationRelationship(AnnotatedRelationshipElement source, DataElement target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AnnotatedRelationshipElementAnnotationRelationship);
        }

        public bool Equals(AnnotatedRelationshipElementAnnotationRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(AnnotatedRelationshipElementAnnotationRelationship? left, AnnotatedRelationshipElementAnnotationRelationship? right)
        {
            return EqualityComparer<AnnotatedRelationshipElementAnnotationRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(AnnotatedRelationshipElementAnnotationRelationship? left, AnnotatedRelationshipElementAnnotationRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as AnnotatedRelationshipElementAnnotationRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}