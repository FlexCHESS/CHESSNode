namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class SubmodelElementListValueRelationship : Relationship<SubmodelElement>, IEquatable<SubmodelElementListValueRelationship>
    {
        public SubmodelElementListValueRelationship()
        {
            Name = "value";
        }

        public SubmodelElementListValueRelationship(SubmodelElementList source, SubmodelElement target) : this()
        {
            InitializeFromTwins(source, target);
        }

        [JsonPropertyName("index")]
        public int? Index { get; set; }
        public override bool Equals(object? obj)
        {
            return Equals(obj as SubmodelElementListValueRelationship);
        }

        public bool Equals(SubmodelElementListValueRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name && Index == other.Index;
        }

        public static bool operator ==(SubmodelElementListValueRelationship? left, SubmodelElementListValueRelationship? right)
        {
            return EqualityComparer<SubmodelElementListValueRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(SubmodelElementListValueRelationship? left, SubmodelElementListValueRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode(), Index?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as SubmodelElementListValueRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}