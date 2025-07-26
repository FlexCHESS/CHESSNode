namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class DataElement : SubmodelElement, IEquatable<DataElement>
    {
        public DataElement()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:DataElement;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as DataElement);
        }

        public bool Equals(DataElement? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(DataElement? left, DataElement? right)
        {
            return EqualityComparer<DataElement?>.Default.Equals(left, right);
        }

        public static bool operator !=(DataElement? left, DataElement? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}