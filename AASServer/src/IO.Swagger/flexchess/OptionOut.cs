namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class OptionOut : BasicDigitalTwin, IEquatable<OptionOut>, IEquatable<BasicDigitalTwin>
    {
        public OptionOut()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:OptionOut;1";

        [JsonPropertyName("Objective")]
        public String objective { get; set; }
        [JsonPropertyName("Option")]
        public String option { get; set; }
        [JsonPropertyName("Status")]
        public ChessStatus[] status { get; set; }
        [JsonPropertyName("KPI")]
        public KPI[] kpi { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as OptionOut);
        }

        public bool Equals(OptionOut? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(OptionOut? left, OptionOut? right)
        {
            return EqualityComparer<OptionOut?>.Default.Equals(left, right);
        }

        public static bool operator !=(OptionOut? left, OptionOut? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as OptionOut) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}
