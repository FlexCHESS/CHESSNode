namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ConceptDescription : Identifiable, IEquatable<ConceptDescription>
    {
        public ConceptDescription()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ConceptDescription;1";
        [JsonIgnore]
        public ConceptDescriptionIsCaseOfRelationshipCollection IsCaseOf { get; set; } = new ConceptDescriptionIsCaseOfRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as ConceptDescription);
        }

        public bool Equals(ConceptDescription? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(ConceptDescription? left, ConceptDescription? right)
        {
            return EqualityComparer<ConceptDescription?>.Default.Equals(left, right);
        }

        public static bool operator !=(ConceptDescription? left, ConceptDescription? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}