namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Limit : BasicDigitalTwin, IEquatable<Limit>, IEquatable<BasicDigitalTwin>
    {
        public Limit()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:Limit;1";

        [JsonPropertyName("Name")]
        public String Name { get; set; }
        [JsonPropertyName("Unit")]
        public String Unit { get; set; }
        [JsonPropertyName("Value")]
        public Double Value { get; set; }
     

        public override bool Equals(object? obj)
        {
            return Equals(obj as Limit);
        }

        public bool Equals(Limit? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(Limit? left, Limit? right)
        {
            return EqualityComparer<Limit?>.Default.Equals(left, right);
        }

        public static bool operator !=(Limit? left, Limit? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Limit) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}