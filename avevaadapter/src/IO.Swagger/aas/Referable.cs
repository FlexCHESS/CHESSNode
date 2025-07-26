namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Referable : HasExtensions, IEquatable<Referable>
    {
        public Referable()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Referable;1";
        [JsonPropertyName("idShort")]
        public string? IdShort { get; set; }
        [JsonPropertyName("category")]
        public string? Category { get; set; }
        [JsonPropertyName("checksum")]
        public string? Checksum { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Referable);
        }

        public bool Equals(Referable? other)
        {
            return other is not null && base.Equals(other) && IdShort == other.IdShort && Category == other.Category && Checksum == other.Checksum;
        }

        public static bool operator ==(Referable? left, Referable? right)
        {
            return EqualityComparer<Referable?>.Default.Equals(left, right);
        }

        public static bool operator !=(Referable? left, Referable? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), IdShort?.GetHashCode(), Category?.GetHashCode(), Checksum?.GetHashCode());
        }
    }
}