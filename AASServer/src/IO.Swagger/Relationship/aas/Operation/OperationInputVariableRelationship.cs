namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class OperationInputVariableRelationship : Relationship<OperationVariable>, IEquatable<OperationInputVariableRelationship>
    {
        public OperationInputVariableRelationship()
        {
            Name = "inputVariable";
        }

        public OperationInputVariableRelationship(Operation source, OperationVariable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as OperationInputVariableRelationship);
        }

        public bool Equals(OperationInputVariableRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(OperationInputVariableRelationship? left, OperationInputVariableRelationship? right)
        {
            return EqualityComparer<OperationInputVariableRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(OperationInputVariableRelationship? left, OperationInputVariableRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as OperationInputVariableRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}