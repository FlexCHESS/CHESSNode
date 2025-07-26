namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class ValueListValueReferencePairRelationship : Relationship<ValueReferencePair>, IEquatable<ValueListValueReferencePairRelationship>
    {
        public ValueListValueReferencePairRelationship()
        {
            Name = "valueReferencePair";
        }

        public ValueListValueReferencePairRelationship(ValueList source, ValueReferencePair target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ValueListValueReferencePairRelationship);
        }

        public bool Equals(ValueListValueReferencePairRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(ValueListValueReferencePairRelationship? left, ValueListValueReferencePairRelationship? right)
        {
            return EqualityComparer<ValueListValueReferencePairRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(ValueListValueReferencePairRelationship? left, ValueListValueReferencePairRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as ValueListValueReferencePairRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}