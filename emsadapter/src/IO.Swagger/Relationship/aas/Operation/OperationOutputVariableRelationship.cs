namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class OperationOutputVariableRelationship : Relationship<OperationVariable>, IEquatable<OperationOutputVariableRelationship>
    {
        public OperationOutputVariableRelationship()
        {
            Name = "outputVariable";
        }

        public OperationOutputVariableRelationship(Operation source, OperationVariable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as OperationOutputVariableRelationship);
        }

        public bool Equals(OperationOutputVariableRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(OperationOutputVariableRelationship? left, OperationOutputVariableRelationship? right)
        {
            return EqualityComparer<OperationOutputVariableRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(OperationOutputVariableRelationship? left, OperationOutputVariableRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as OperationOutputVariableRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}