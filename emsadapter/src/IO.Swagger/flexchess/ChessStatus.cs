namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ChessStatus : BasicDigitalTwin, IEquatable<ChessStatus>, IEquatable<BasicDigitalTwin>
    {
        public ChessStatus()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:com:flexchess:chessstatus;1";

        [JsonPropertyName("status")]
        public String status { get; set; }
        [JsonPropertyName("service")]
        public String service { get; set; }
        [JsonPropertyName("starttime")]
        public String starttime { get; set; }
        [JsonPropertyName("endtime")]
        public String endtime { get; set; }
        [JsonPropertyName("capacity")]
        public String capacity { get; set; }
        [JsonPropertyName("recurrence")]
        public String recurrence { get; set; }
        [JsonPropertyName("efficiency")]
        public Double efficiency { get; set; }        
        [JsonPropertyName("priority")]
        public Int32 priority { get; set; }
        [JsonPropertyName("probability")]
        public Double probability { get; set; }
        [JsonPropertyName("capacityStart")]
        public Double capacityStart { get; set; }
        [JsonPropertyName("capacityEnd")]
        public Double capacityEnd { get; set; }
        [JsonPropertyName("capacityMax")]
        public Double capacityMax{ get; set; }
        [JsonPropertyName("cycleCost")]
        public Double cycleCost{ get; set; }      


        public override bool Equals(object? obj)
        {
            return Equals(obj as ChessStatus);
        }

        public bool Equals(ChessStatus? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(ChessStatus? left, ChessStatus? right)
        {
            return EqualityComparer<ChessStatus?>.Default.Equals(left, right);
        }

        public static bool operator !=(ChessStatus? left, ChessStatus? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as ChessStatus) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}