namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AnnotatedRelationshipElement : RelationshipElement, IEquatable<AnnotatedRelationshipElement>
    {
        public AnnotatedRelationshipElement()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:AnnotatedRelationshipElement;1";
        [JsonIgnore]
        public AnnotatedRelationshipElementAnnotationRelationshipCollection Annotation { get; set; } = new AnnotatedRelationshipElementAnnotationRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as AnnotatedRelationshipElement);
        }

        public bool Equals(AnnotatedRelationshipElement? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(AnnotatedRelationshipElement? left, AnnotatedRelationshipElement? right)
        {
            return EqualityComparer<AnnotatedRelationshipElement?>.Default.Equals(left, right);
        }

        public static bool operator !=(AnnotatedRelationshipElement? left, AnnotatedRelationshipElement? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}