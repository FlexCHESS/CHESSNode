namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Battery : Storage, IEquatable<Battery>
    {
        public Battery()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:com:flexchess:der:storage:battery;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as Battery);
        }

        public bool Equals(Battery? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(Battery? left, Battery? right)
        {
            return EqualityComparer<Battery?>.Default.Equals(left, right);
        }

        public static bool operator !=(Battery? left, Battery? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}