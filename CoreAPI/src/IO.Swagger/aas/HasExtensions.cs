namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class HasExtensions : BasicDigitalTwin, IEquatable<HasExtensions>, IEquatable<BasicDigitalTwin>
    {
        public HasExtensions()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:HasExtensions;1";
        [JsonIgnore]
        public HasExtensionsExtensionsRelationshipCollection Extensions { get; set; } = new HasExtensionsExtensionsRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as HasExtensions);
        }

        public bool Equals(HasExtensions? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(HasExtensions? left, HasExtensions? right)
        {
            return EqualityComparer<HasExtensions?>.Default.Equals(left, right);
        }

        public static bool operator !=(HasExtensions? left, HasExtensions? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as HasExtensions) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}