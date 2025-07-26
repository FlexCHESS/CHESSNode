namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class DataSpecificationContent : BasicDigitalTwin, IEquatable<DataSpecificationContent>, IEquatable<BasicDigitalTwin>
    {
        public DataSpecificationContent()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:DataSpecificationContent;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as DataSpecificationContent);
        }

        public bool Equals(DataSpecificationContent? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(DataSpecificationContent? left, DataSpecificationContent? right)
        {
            return EqualityComparer<DataSpecificationContent?>.Default.Equals(left, right);
        }

        public static bool operator !=(DataSpecificationContent? left, DataSpecificationContent? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as DataSpecificationContent) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}