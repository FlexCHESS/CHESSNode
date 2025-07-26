namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ValueList : BasicDigitalTwin, IEquatable<ValueList>, IEquatable<BasicDigitalTwin>
    {
        public ValueList()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:ValueList;1";
        [JsonIgnore]
        public ValueListValueReferencePairRelationshipCollection ValueReferencePair { get; set; } = new ValueListValueReferencePairRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as ValueList);
        }

        public bool Equals(ValueList? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(ValueList? left, ValueList? right)
        {
            return EqualityComparer<ValueList?>.Default.Equals(left, right);
        }

        public static bool operator !=(ValueList? left, ValueList? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as ValueList) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}