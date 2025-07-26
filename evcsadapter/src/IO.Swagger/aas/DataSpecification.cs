namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class DataSpecification : BasicDigitalTwin, IEquatable<DataSpecification>, IEquatable<BasicDigitalTwin>
    {
        public DataSpecification()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:DataSpecification;1";
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonIgnore]
        public DataSpecificationHasContentRelationshipCollection HasContent { get; set; } = new DataSpecificationHasContentRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as DataSpecification);
        }

        public bool Equals(DataSpecification? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId && Id == other.Id;
        }

        public static bool operator ==(DataSpecification? left, DataSpecification? right)
        {
            return EqualityComparer<DataSpecification?>.Default.Equals(left, right);
        }

        public static bool operator !=(DataSpecification? left, DataSpecification? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode(), Id?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as DataSpecification) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}