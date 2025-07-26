namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class HasDataSpecification : BasicDigitalTwin, IEquatable<HasDataSpecification>, IEquatable<BasicDigitalTwin>
    {
        public HasDataSpecification()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:HasDataSpecification;1";
        [JsonPropertyName("dataSpecificationTemplateGlobalRefValue")]
        public string? DataSpecificationTemplateGlobalRefValue { get; set; }
        [JsonIgnore]
        public HasDataSpecificationDataSpecificationRelationshipCollection DataSpecification { get; set; } = new HasDataSpecificationDataSpecificationRelationshipCollection();
        [JsonIgnore]
        public HasDataSpecificationDataSpecificationRefRelationshipCollection DataSpecificationRef { get; set; } = new HasDataSpecificationDataSpecificationRefRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as HasDataSpecification);
        }

        public bool Equals(HasDataSpecification? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && DataSpecificationTemplateGlobalRefValue == other.DataSpecificationTemplateGlobalRefValue;
        }

        public static bool operator ==(HasDataSpecification? left, HasDataSpecification? right)
        {
            return EqualityComparer<HasDataSpecification?>.Default.Equals(left, right);
        }

        public static bool operator !=(HasDataSpecification? left, HasDataSpecification? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), DataSpecificationTemplateGlobalRefValue?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as HasDataSpecification) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}