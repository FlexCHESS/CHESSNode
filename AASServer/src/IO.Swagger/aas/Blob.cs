namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Blob : DataElement, IEquatable<Blob>
    {
        public Blob()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Blob;1";
        [JsonPropertyName("value")]
        public string? Value { get; set; }
        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Blob);
        }

        public bool Equals(Blob? other)
        {
            return other is not null && base.Equals(other) && Value == other.Value && MimeType == other.MimeType;
        }

        public static bool operator ==(Blob? left, Blob? right)
        {
            return EqualityComparer<Blob?>.Default.Equals(left, right);
        }

        public static bool operator !=(Blob? left, Blob? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), Value?.GetHashCode(), MimeType?.GetHashCode());
        }
    }
}