namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelElementList : SubmodelElement, IEquatable<SubmodelElementList>
    {
        public SubmodelElementList()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:SubmodelElementList;1";
        [JsonPropertyName("orderRelevant")]
        public bool? OrderRelevant { get; set; }
        [JsonPropertyName("semanticIdListElementValue")]
        public string? SemanticIdListElementValue { get; set; }
        [JsonPropertyName("typeValueListElement")]
        public SubmodelElementListTypeValueListElement? TypeValueListElement { get; set; }
        [JsonPropertyName("valueTypeListElement")]
        public string? ValueTypeListElement { get; set; }
        [JsonIgnore]
        public SubmodelElementListValueRelationshipCollection Value { get; set; } = new SubmodelElementListValueRelationshipCollection();
        [JsonIgnore]
        public SubmodelElementListSemanticIdListElementRelationshipCollection SemanticIdListElement { get; set; } = new SubmodelElementListSemanticIdListElementRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelElementList);
        }

        public bool Equals(SubmodelElementList? other)
        {
            return other is not null && base.Equals(other) && OrderRelevant == other.OrderRelevant && SemanticIdListElementValue == other.SemanticIdListElementValue && TypeValueListElement == other.TypeValueListElement && ValueTypeListElement == other.ValueTypeListElement;
        }

        public static bool operator ==(SubmodelElementList? left, SubmodelElementList? right)
        {
            return EqualityComparer<SubmodelElementList?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelElementList? left, SubmodelElementList? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), OrderRelevant?.GetHashCode(), SemanticIdListElementValue?.GetHashCode(), TypeValueListElement?.GetHashCode(), ValueTypeListElement?.GetHashCode());
        }
    }
}