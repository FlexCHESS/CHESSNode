namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AdministrationInformation : BasicDigitalTwin, IEquatable<AdministrationInformation>, IEquatable<BasicDigitalTwin>
    {
        public AdministrationInformation()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:AdministrationInformation;1";
        [JsonPropertyName("version")]
        public string? Version { get; set; }
        [JsonPropertyName("revision")]
        public string? Revision { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as AdministrationInformation);
        }

        public bool Equals(AdministrationInformation? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Version == other.Version && Revision == other.Revision;
        }

        public static bool operator ==(AdministrationInformation? left, AdministrationInformation? right)
        {
            return EqualityComparer<AdministrationInformation?>.Default.Equals(left, right);
        }

        public static bool operator !=(AdministrationInformation? left, AdministrationInformation? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Version?.GetHashCode(), Revision?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as AdministrationInformation) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}