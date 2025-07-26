namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class measurement : Storage, IEquatable<measurement>
    {
        public measurement()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:com:flexchess:der:storage:battery;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as measurement);
        }

        public bool Equals(measurement? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(measurement? left, measurement? right)
        {
            return EqualityComparer<measurement?>.Default.Equals(left, right);
        }

        public static bool operator !=(measurement? left, measurement? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}