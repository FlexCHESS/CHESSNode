namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Tags : BasicDigitalTwin, IEquatable<Tags>, IEquatable<BasicDigitalTwin>
    {
        public Tags()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:ext:Tags;1";
        [JsonPropertyName("markers")]
        public IDictionary<string, bool>? Markers { get; set; }
        [JsonPropertyName("values")]
        public IDictionary<string, string>? Values { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Tags);
        }

        public bool Equals(Tags? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Markers == other.Markers && Values == other.Values;
        }

        public static bool operator ==(Tags? left, Tags? right)
        {
            return EqualityComparer<Tags?>.Default.Equals(left, right);
        }

        public static bool operator !=(Tags? left, Tags? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Markers?.GetHashCode(), Values?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Tags) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}