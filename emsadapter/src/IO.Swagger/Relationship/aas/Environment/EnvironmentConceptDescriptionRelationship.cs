namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EnvironmentConceptDescriptionRelationship : Relationship<ConceptDescription>, IEquatable<EnvironmentConceptDescriptionRelationship>
    {
        public EnvironmentConceptDescriptionRelationship()
        {
            Name = "conceptDescription";
        }

        public EnvironmentConceptDescriptionRelationship(Environment source, ConceptDescription target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EnvironmentConceptDescriptionRelationship);
        }

        public bool Equals(EnvironmentConceptDescriptionRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EnvironmentConceptDescriptionRelationship? left, EnvironmentConceptDescriptionRelationship? right)
        {
            return EqualityComparer<EnvironmentConceptDescriptionRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EnvironmentConceptDescriptionRelationship? left, EnvironmentConceptDescriptionRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EnvironmentConceptDescriptionRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}