namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ChessDayahead : BasicDigitalTwin, IEquatable<ChessDayahead>, IEquatable<BasicDigitalTwin>
    {
        public ChessDayahead()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:chess:dayahead:forecast;1";

        [JsonPropertyName("dayahead")]
        public Double[] dayahead { get; set; }
        

        public override bool Equals(object? obj)
        {
            return Equals(obj as ChessDayahead);
        }

        public bool Equals(ChessDayahead? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(ChessDayahead? left, ChessDayahead? right)
        {
            return EqualityComparer<ChessDayahead?>.Default.Equals(left, right);
        }

        public static bool operator !=(ChessDayahead? left, ChessDayahead? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as ChessDayahead) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}
