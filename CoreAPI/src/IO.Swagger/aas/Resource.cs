namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Resource : BasicDigitalTwin, IEquatable<Resource>, IEquatable<BasicDigitalTwin>
    {
        public Resource()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:Resource;1";
        [JsonPropertyName("path")]
        public string? Path { get; set; }
        [JsonPropertyName("contentType")]
        public string? ContentType { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as Resource);
        }

        public bool Equals(Resource? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Path == other.Path && ContentType == other.ContentType;
        }

        public static bool operator ==(Resource? left, Resource? right)
        {
            return EqualityComparer<Resource?>.Default.Equals(left, right);
        }

        public static bool operator !=(Resource? left, Resource? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Path?.GetHashCode(), ContentType?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Resource) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}