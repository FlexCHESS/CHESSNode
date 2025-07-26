namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Formula : BasicDigitalTwin, IEquatable<Formula>, IEquatable<BasicDigitalTwin>
    {
        public Formula()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:Formula;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as Formula);
        }

        public bool Equals(Formula? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(Formula? left, Formula? right)
        {
            return EqualityComparer<Formula?>.Default.Equals(left, right);
        }

        public static bool operator !=(Formula? left, Formula? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Formula) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}