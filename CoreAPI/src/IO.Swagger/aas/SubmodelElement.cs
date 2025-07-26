namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelElement : BasicDigitalTwin, IEquatable<SubmodelElement>, IEquatable<BasicDigitalTwin>
    {
        public SubmodelElement()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:SubmodelElement;1";
        [JsonPropertyName("semanticIdValue")]
        public string? SemanticIdValue { get; set; }
        [JsonPropertyName("dataSpecificationTemplateGlobalRefValue")]
        public string? DataSpecificationTemplateGlobalRefValue { get; set; }
        [JsonIgnore]
        public SubmodelElementSemanticIdRelationshipCollection SemanticId { get; set; } = new SubmodelElementSemanticIdRelationshipCollection();
        [JsonIgnore]
        public SubmodelElementSupplementalSemanticIdRelationshipCollection SupplementalSemanticId { get; set; } = new SubmodelElementSupplementalSemanticIdRelationshipCollection();
        [JsonIgnore]
        public SubmodelElementDataSpecificationRelationshipCollection DataSpecification { get; set; } = new SubmodelElementDataSpecificationRelationshipCollection();
        [JsonIgnore]
        public SubmodelElementDataSpecificationRefRelationshipCollection DataSpecificationRef { get; set; } = new SubmodelElementDataSpecificationRefRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelElement);
        }

        public bool Equals(SubmodelElement? other)
        {
            return other is not null && base.Equals(other) && SemanticIdValue == other.SemanticIdValue && DataSpecificationTemplateGlobalRefValue == other.DataSpecificationTemplateGlobalRefValue;
        }

        public static bool operator ==(SubmodelElement? left, SubmodelElement? right)
        {
            return EqualityComparer<SubmodelElement?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelElement? left, SubmodelElement? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), SemanticIdValue?.GetHashCode(), DataSpecificationTemplateGlobalRefValue?.GetHashCode());
        }

        bool IEquatable<BasicDigitalTwin>.Equals(BasicDigitalTwin? other)
        {
            throw new NotImplementedException();
        }
    }
}