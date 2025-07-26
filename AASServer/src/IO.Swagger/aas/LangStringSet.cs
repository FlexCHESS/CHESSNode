namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class LangStringSet : BasicDigitalTwin, IEquatable<LangStringSet>, IEquatable<BasicDigitalTwin>
    {
        public LangStringSet()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:LangStringSet;1";
        [JsonPropertyName("langString")]
        public IDictionary<string, string>? LangString { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as LangStringSet);
        }

        public bool Equals(LangStringSet? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && LangString == other.LangString;
        }

        public static bool operator ==(LangStringSet? left, LangStringSet? right)
        {
            return EqualityComparer<LangStringSet?>.Default.Equals(left, right);
        }

        public static bool operator !=(LangStringSet? left, LangStringSet? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), LangString?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as LangStringSet) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}