namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Capability : SubmodelElement, IEquatable<Capability>
    {
        public Capability()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Capability;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as Capability);
        }

        public bool Equals(Capability? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(Capability? left, Capability? right)
        {
            return EqualityComparer<Capability?>.Default.Equals(left, right);
        }

        public static bool operator !=(Capability? left, Capability? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}