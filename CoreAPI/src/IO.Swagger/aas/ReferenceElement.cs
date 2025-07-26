namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ReferenceElement : DataElement, IEquatable<ReferenceElement>
    {
        public ReferenceElement()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:ReferenceElement;1";
        [JsonIgnore]
        public ReferenceElementReferredElementRelationshipCollection ReferredElement { get; set; } = new ReferenceElementReferredElementRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as ReferenceElement);
        }

        public bool Equals(ReferenceElement? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(ReferenceElement? left, ReferenceElement? right)
        {
            return EqualityComparer<ReferenceElement?>.Default.Equals(left, right);
        }

        public static bool operator !=(ReferenceElement? left, ReferenceElement? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}