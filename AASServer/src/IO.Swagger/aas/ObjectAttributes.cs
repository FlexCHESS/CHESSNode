namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ObjectAttributes : BasicDigitalTwin, IEquatable<ObjectAttributes>, IEquatable<BasicDigitalTwin>
    {
        public ObjectAttributes()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:ObjectAttributes;1";
        [JsonIgnore]
        public ObjectAttributesObjectAttributeRelationshipCollection ObjectAttribute { get; set; } = new ObjectAttributesObjectAttributeRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as ObjectAttributes);
        }

        public bool Equals(ObjectAttributes? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(ObjectAttributes? left, ObjectAttributes? right)
        {
            return EqualityComparer<ObjectAttributes?>.Default.Equals(left, right);
        }

        public static bool operator !=(ObjectAttributes? left, ObjectAttributes? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as ObjectAttributes) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}