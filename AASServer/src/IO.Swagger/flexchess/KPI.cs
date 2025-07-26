namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class KPI : BasicDigitalTwin, IEquatable<KPI>, IEquatable<BasicDigitalTwin>
    {
        public KPI()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:KPI;1";

        [JsonPropertyName("Name")]
        public String Name { get; set; }
        [JsonPropertyName("Unit")]
        public String Unit { get; set; }
        [JsonPropertyName("Value")]
        public Double Value { get; set; }
     

        public override bool Equals(object? obj)
        {
            return Equals(obj as KPI);
        }

        public bool Equals(KPI? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(KPI? left, KPI? right)
        {
            return EqualityComparer<KPI?>.Default.Equals(left, right);
        }

        public static bool operator !=(KPI? left, KPI? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as KPI) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}