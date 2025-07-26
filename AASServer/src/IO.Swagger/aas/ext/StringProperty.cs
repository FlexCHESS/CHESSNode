namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class StringProperty : Property, IEquatable<StringProperty>
    {
        public StringProperty()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ext:StringProperty;1";
        public override bool Equals(object? obj)
        {
            return Equals(obj as StringProperty);
        }

        public bool Equals(StringProperty? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(StringProperty? left, StringProperty? right)
        {
            return EqualityComparer<StringProperty?>.Default.Equals(left, right);
        }

        public static bool operator !=(StringProperty? left, StringProperty? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}