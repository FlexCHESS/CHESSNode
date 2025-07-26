namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Reference : BasicDigitalTwin, IEquatable<Reference>, IEquatable<BasicDigitalTwin>
    {
        public Reference()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:Reference;1";
        [JsonPropertyName("type")]
        public ReferenceType? Type { get; set; }
        [JsonIgnore]
        public ReferenceReferredSemanticIdRelationshipCollection ReferredSemanticId { get; set; } = new ReferenceReferredSemanticIdRelationshipCollection();
        [JsonIgnore]
        public ReferenceReferredElementRelationshipCollection ReferredElement { get; set; } = new ReferenceReferredElementRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Reference);
        }

        public bool Equals(Reference? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Type == other.Type;
        }

        public static bool operator ==(Reference? left, Reference? right)
        {
            return EqualityComparer<Reference?>.Default.Equals(left, right);
        }

        public static bool operator !=(Reference? left, Reference? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Type?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Reference) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}