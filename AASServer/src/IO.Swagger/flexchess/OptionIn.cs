namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class OptionIn : BasicDigitalTwin, IEquatable<OptionIn>, IEquatable<BasicDigitalTwin>
    {
        public OptionIn()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:OptionIn;1";

        [JsonPropertyName("Objective")]
        public String objective { get; set; }
        [JsonPropertyName("Option")]
        public String option { get; set; }
        [JsonPropertyName("Status")]
        public ChessStatus[] status { get; set; }
        

        public override bool Equals(object? obj)
        {
            return Equals(obj as OptionIn);
        }

        public bool Equals(OptionIn? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(OptionIn? left, OptionIn? right)
        {
            return EqualityComparer<OptionIn?>.Default.Equals(left, right);
        }

        public static bool operator !=(OptionIn? left, OptionIn? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as OptionIn) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}
