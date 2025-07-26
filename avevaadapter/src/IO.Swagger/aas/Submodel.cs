namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using IO.Swagger.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Submodel :   BasicDigitalTwin, IEquatable<Submodel>, IEquatable<BasicDigitalTwin>
    {
        public Submodel()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Submodel;1";

        [JsonPropertyName("semanticIdValue")]
        public string? SemanticIdValue { get; set; }
        [JsonPropertyName("displayName")]
        public LangStringElementType DisplayName { get; set; }
        [JsonPropertyName("description")]
        public LangStringElementType Description { get; set; }
        [JsonPropertyName("tags")]
        public LangStringElementType Tags { get; set; }
        [JsonPropertyName("administration")]
        public LangStringElementType Administration { get; set; }
        [JsonPropertyName("dataSpecificationTemplateGlobalRefValue")]
        public string? DataSpecificationTemplateGlobalRefValue { get; set; }

        [JsonIgnore]
        public SubmodelSubmodelElementRelationshipCollection SubmodelElement { get; set; } = new SubmodelSubmodelElementRelationshipCollection();
        [JsonIgnore]
        public SubmodelSemanticIdRelationshipCollection SemanticId { get; set; } = new SubmodelSemanticIdRelationshipCollection();
        [JsonIgnore]
        public SubmodelSupplementalSemanticIdRelationshipCollection SupplementalSemanticId { get; set; } = new SubmodelSupplementalSemanticIdRelationshipCollection();
        [JsonIgnore]
        public SubmodelDataSpecificationRelationshipCollection DataSpecification { get; set; } = new SubmodelDataSpecificationRelationshipCollection();
        [JsonIgnore]
        public SubmodelDataSpecificationRefRelationshipCollection DataSpecificationRef { get; set; } = new SubmodelDataSpecificationRefRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Submodel);
        }

        public bool Equals(Submodel? other)
        {
            return other is not null && base.Equals(other) && SemanticIdValue == other.SemanticIdValue && DataSpecificationTemplateGlobalRefValue == other.DataSpecificationTemplateGlobalRefValue;
        }

        public static bool operator ==(Submodel? left, Submodel? right)
        {
            return EqualityComparer<Submodel?>.Default.Equals(left, right);
        }

        public static bool operator !=(Submodel? left, Submodel? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), SemanticIdValue?.GetHashCode(), DataSpecificationTemplateGlobalRefValue?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin other)
        {
            throw new NotImplementedException();
        }

      
        /// <summary>
        /// Gets or Sets Kind
        /// </summary>
        [JsonPropertyName("kind")]
        public Kind Kind { get; set; }

       
        

    }
}