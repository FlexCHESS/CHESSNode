namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class OptimiserIn : BasicDigitalTwin, IEquatable<OptimiserIn>, IEquatable<BasicDigitalTwin>
    {
        public OptimiserIn()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:OptimiserIn;1";

        [JsonPropertyName("Limits")]
        public Limit[] Limits { get; set; }
        [JsonPropertyName("Options")]
        public OptionIn[] Options { get; set; }
       

        public override bool Equals(object? obj)
        {
            return Equals(obj as OptimiserIn);
        }

        public bool Equals(OptimiserIn? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(OptimiserIn? left, OptimiserIn? right)
        {
            return EqualityComparer<OptimiserIn?>.Default.Equals(left, right);
        }

        public static bool operator !=(OptimiserIn? left, OptimiserIn? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as OptimiserIn) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}