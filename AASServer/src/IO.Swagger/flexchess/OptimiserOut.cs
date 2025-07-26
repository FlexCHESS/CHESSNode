namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class OptimiserOut : BasicDigitalTwin, IEquatable<OptimiserOut>, IEquatable<BasicDigitalTwin>
    {
        public OptimiserOut()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:OptimiserOut;1";

      
        [JsonPropertyName("Options")]
        public OptionOut[] Options { get; set; }
       

        public override bool Equals(object? obj)
        {
            return Equals(obj as OptimiserOut);
        }

        public bool Equals(OptimiserOut? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(OptimiserOut? left, OptimiserOut? right)
        {
            return EqualityComparer<OptimiserOut?>.Default.Equals(left, right);
        }

        public static bool operator !=(OptimiserOut? left, OptimiserOut? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as OptimiserOut) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}