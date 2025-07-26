namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Generation : BasicDigitalTwin, IEquatable<Generation>, IEquatable<BasicDigitalTwin>
    {
        public Generation()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:der:generation;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as Generation);
        }

        public bool Equals(Generation? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(Generation? left, Generation? right)
        {
            return EqualityComparer<Generation?>.Default.Equals(left, right);
        }

        public static bool operator !=(Generation? left, Generation? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Generation) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}