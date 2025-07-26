namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class HasKind : BasicDigitalTwin, IEquatable<HasKind>, IEquatable<BasicDigitalTwin>
    {
        public HasKind()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:HasKind;1";
        [JsonPropertyName("kind")]
        public HasKindKind? Kind { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as HasKind);
        }

        public bool Equals(HasKind? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Kind == other.Kind;
        }

        public static bool operator ==(HasKind? left, HasKind? right)
        {
            return EqualityComparer<HasKind?>.Default.Equals(left, right);
        }

        public static bool operator !=(HasKind? left, HasKind? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Kind?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as HasKind) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}