namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Chess : BasicDigitalTwin, IEquatable<Chess>, IEquatable<BasicDigitalTwin>
    {
        public Chess()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:chess;1";
        [JsonPropertyName("identifier")]
        public string? identifier { get; set; }
        [JsonPropertyName("location")]
        public string? location { get; set; }
        [JsonPropertyName("standard")]
        public string? standard { get; set; }
        [JsonPropertyName("version")]
        public string? version { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Chess);
        }

        public bool Equals(Chess? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(Chess? left, Chess? right)
        {
            return EqualityComparer<Chess?>.Default.Equals(left, right);
        }

        public static bool operator !=(Chess? left, Chess? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Chess) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}