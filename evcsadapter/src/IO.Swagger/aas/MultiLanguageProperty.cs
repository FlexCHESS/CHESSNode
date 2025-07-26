namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class MultiLanguageProperty : DataElement, IEquatable<MultiLanguageProperty>
    {
        public MultiLanguageProperty()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:MultiLanguageProperty;1";
        [JsonIgnore]
        public MultiLanguagePropertyValueIdRelationshipCollection ValueId { get; set; } = new MultiLanguagePropertyValueIdRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as MultiLanguageProperty);
        }

        public bool Equals(MultiLanguageProperty? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(MultiLanguageProperty? left, MultiLanguageProperty? right)
        {
            return EqualityComparer<MultiLanguageProperty?>.Default.Equals(left, right);
        }

        public static bool operator !=(MultiLanguageProperty? left, MultiLanguageProperty? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}