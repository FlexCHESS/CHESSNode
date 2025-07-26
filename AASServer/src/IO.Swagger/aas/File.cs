namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class File : DataElement, IEquatable<File>
    {
        public File()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:File;1";
        [JsonPropertyName("value")]
        public string? Value { get; set; }
        [JsonPropertyName("contentType")]
        public string? ContentType { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as File);
        }

        public bool Equals(File? other)
        {
            return other is not null && base.Equals(other) && Value == other.Value && ContentType == other.ContentType;
        }

        public static bool operator ==(File? left, File? right)
        {
            return EqualityComparer<File?>.Default.Equals(left, right);
        }

        public static bool operator !=(File? left, File? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), Value?.GetHashCode(), ContentType?.GetHashCode());
        }
    }
}