namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class RelationshipElement : SubmodelElement, IEquatable<RelationshipElement>
    {
        public RelationshipElement()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:RelationshipElement;1";
        [JsonIgnore]
        public RelationshipElementFirstRelationshipCollection First { get; set; } = new RelationshipElementFirstRelationshipCollection();
        [JsonIgnore]
        public RelationshipElementFirstModelRefRelationshipCollection FirstModelRef { get; set; } = new RelationshipElementFirstModelRefRelationshipCollection();
        [JsonIgnore]
        public RelationshipElementSecondRelationshipCollection Second { get; set; } = new RelationshipElementSecondRelationshipCollection();
        [JsonIgnore]
        public RelationshipElementSecondModelRefRelationshipCollection SecondModelRef { get; set; } = new RelationshipElementSecondModelRefRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as RelationshipElement);
        }

        public bool Equals(RelationshipElement? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(RelationshipElement? left, RelationshipElement? right)
        {
            return EqualityComparer<RelationshipElement?>.Default.Equals(left, right);
        }

        public static bool operator !=(RelationshipElement? left, RelationshipElement? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}