namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class HasSemantics : BasicDigitalTwin, IEquatable<HasSemantics>, IEquatable<BasicDigitalTwin>
    {
        public HasSemantics()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:HasSemantics;1";
        [JsonPropertyName("semanticIdValue")]
        public string? SemanticIdValue { get; set; }
        [JsonIgnore]
        public HasSemanticsSemanticIdRelationshipCollection SemanticId { get; set; } = new HasSemanticsSemanticIdRelationshipCollection();
        [JsonIgnore]
        public HasSemanticsSupplementalSemanticIdRelationshipCollection SupplementalSemanticId { get; set; } = new HasSemanticsSupplementalSemanticIdRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as HasSemantics);
        }

        public bool Equals(HasSemantics? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && SemanticIdValue == other.SemanticIdValue;
        }

        public static bool operator ==(HasSemantics? left, HasSemantics? right)
        {
            return EqualityComparer<HasSemantics?>.Default.Equals(left, right);
        }

        public static bool operator !=(HasSemantics? left, HasSemantics? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), SemanticIdValue?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as HasSemantics) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}