namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class EnvironmentSubmodelRelationship : Relationship<Submodel>, IEquatable<EnvironmentSubmodelRelationship>
    {
        public EnvironmentSubmodelRelationship()
        {
            Name = "submodel";
        }

        public EnvironmentSubmodelRelationship(Environment source, Submodel target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EnvironmentSubmodelRelationship);
        }

        public bool Equals(EnvironmentSubmodelRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(EnvironmentSubmodelRelationship? left, EnvironmentSubmodelRelationship? right)
        {
            return EqualityComparer<EnvironmentSubmodelRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(EnvironmentSubmodelRelationship? left, EnvironmentSubmodelRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as EnvironmentSubmodelRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}