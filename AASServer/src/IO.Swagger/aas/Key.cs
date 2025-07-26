namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Key : BasicDigitalTwin, IEquatable<Key>, IEquatable<BasicDigitalTwin>
    {
        public Key()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:Key;1";
        [JsonPropertyName("type")]
        public KeyType? Type { get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Key);
        }

        public bool Equals(Key? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Type == other.Type && Value == other.Value;
        }

        public static bool operator ==(Key? left, Key? right)
        {
            return EqualityComparer<Key?>.Default.Equals(left, right);
        }

        public static bool operator !=(Key? left, Key? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Type?.GetHashCode(), Value?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Key) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}