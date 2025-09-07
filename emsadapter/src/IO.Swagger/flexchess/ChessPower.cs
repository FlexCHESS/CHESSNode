namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ChessPower : BasicDigitalTwin, IEquatable<ChessPower>, IEquatable<BasicDigitalTwin>
    {
        public ChessPower()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:chessspower;1";

        [JsonPropertyName("powerActiveImport")]
        public Double powerActiveImport { get; set; }
       

        public override bool Equals(object? obj)
        {
            return Equals(obj as ChessPower);
        }

        public bool Equals(ChessPower? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(ChessPower? left, ChessPower? right)
        {
            return EqualityComparer<ChessPower?>.Default.Equals(left, right);
        }

        public static bool operator !=(ChessPower? left, ChessPower? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as ChessPower) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}
