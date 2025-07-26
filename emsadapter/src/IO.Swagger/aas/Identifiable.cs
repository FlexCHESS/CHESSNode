namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Identifiable : Referable, IEquatable<Identifiable>
    {
        public Identifiable()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Identifiable;1";
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Identifiable);
        }

        public bool Equals(Identifiable? other)
        {
            return other is not null && base.Equals(other) && Id == other.Id;
        }

        public static bool operator ==(Identifiable? left, Identifiable? right)
        {
            return EqualityComparer<Identifiable?>.Default.Equals(left, right);
        }

        public static bool operator !=(Identifiable? left, Identifiable? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), Id?.GetHashCode());
        }
    }
}