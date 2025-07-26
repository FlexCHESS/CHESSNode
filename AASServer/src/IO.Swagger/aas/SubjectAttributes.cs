namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubjectAttributes : BasicDigitalTwin, IEquatable<SubjectAttributes>, IEquatable<BasicDigitalTwin>
    {
        public SubjectAttributes()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:SubjectAttributes;1";
        [JsonIgnore]
        public SubjectAttributesSubjectAttributeRelationshipCollection SubjectAttribute { get; set; } = new SubjectAttributesSubjectAttributeRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as SubjectAttributes);
        }

        public bool Equals(SubjectAttributes? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(SubjectAttributes? left, SubjectAttributes? right)
        {
            return EqualityComparer<SubjectAttributes?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubjectAttributes? left, SubjectAttributes? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as SubjectAttributes) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}