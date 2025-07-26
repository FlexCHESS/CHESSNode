namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ChessAdapter : BasicDigitalTwin, IEquatable<ChessAdapter>, IEquatable<BasicDigitalTwin>
    {
        public ChessAdapter()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:chessadapter;1";
        [JsonPropertyName("identifier")]
        public string? identifier { get; set; }
        [JsonPropertyName("location")]
        public string? location { get; set; }
        [JsonPropertyName("standard")]
        public string? standard { get; set; }
        [JsonPropertyName("version")]
        public string? version { get; set; }
        [JsonPropertyName("container")]
        public string? container { get; set; }
        [JsonPropertyName("credentials")]
        public string? credentials { get; set; }
        [JsonPropertyName("wireless")]
        public string? wireless { get; set; }
        [JsonPropertyName("envconf")]
        public string? envconf { get; set; }
        [JsonPropertyName("volumeMount")]
        public string? volumeMount { get; set; }
        [JsonPropertyName("exposedPort")]
        public Int32? exposedPort { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ChessAdapter);
        }

        public bool Equals(ChessAdapter? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(ChessAdapter? left, ChessAdapter? right)
        {
            return EqualityComparer<ChessAdapter?>.Default.Equals(left, right);
        }

        public static bool operator !=(ChessAdapter? left, ChessAdapter? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as ChessAdapter) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}