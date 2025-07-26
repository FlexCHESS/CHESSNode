namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Demand : BasicDigitalTwin, IEquatable<Demand>, IEquatable<BasicDigitalTwin>
    {
        public Demand()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:der:demand;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as Demand);
        }

        public bool Equals(Demand? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(Demand? left, Demand? right)
        {
            return EqualityComparer<Demand?>.Default.Equals(left, right);
        }

        public static bool operator !=(Demand? left, Demand? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Demand) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}