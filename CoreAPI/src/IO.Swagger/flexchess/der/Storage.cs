namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Storage : BasicDigitalTwin, IEquatable<Storage>, IEquatable<BasicDigitalTwin>
    {
        public Storage()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:der:storage;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as Storage);
        }

        public bool Equals(Storage? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(Storage? left, Storage? right)
        {
            return EqualityComparer<Storage?>.Default.Equals(left, right);
        }

        public static bool operator !=(Storage? left, Storage? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Storage) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}