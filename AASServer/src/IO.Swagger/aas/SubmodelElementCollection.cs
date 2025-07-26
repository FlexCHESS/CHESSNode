namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelElementCollection : SubmodelElement, IEquatable<SubmodelElementCollection>
    {
        public SubmodelElementCollection()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:SubmodelElementCollection;1";
        [JsonIgnore]
        public SubmodelElementCollectionValueRelationshipCollection Value { get; set; } = new SubmodelElementCollectionValueRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelElementCollection);
        }

        public bool Equals(SubmodelElementCollection? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(SubmodelElementCollection? left, SubmodelElementCollection? right)
        {
            return EqualityComparer<SubmodelElementCollection?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelElementCollection? left, SubmodelElementCollection? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}