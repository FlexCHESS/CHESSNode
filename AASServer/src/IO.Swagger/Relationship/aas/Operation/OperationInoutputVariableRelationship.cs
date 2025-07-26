namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class OperationInoutputVariableRelationship : Relationship<OperationVariable>, IEquatable<OperationInoutputVariableRelationship>
    {
        public OperationInoutputVariableRelationship()
        {
            Name = "inoutputVariable";
        }

        public OperationInoutputVariableRelationship(Operation source, OperationVariable target) : this()
        {
            InitializeFromTwins(source, target);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as OperationInoutputVariableRelationship);
        }

        public bool Equals(OperationInoutputVariableRelationship? other)
        {
            return other is not null && Id == other.Id && SourceId == other.SourceId && TargetId == other.TargetId && Target == other.Target && Name == other.Name;
        }

        public static bool operator ==(OperationInoutputVariableRelationship? left, OperationInoutputVariableRelationship? right)
        {
            return EqualityComparer<OperationInoutputVariableRelationship?>.Default.Equals(left, right);
        }

        public static bool operator !=(OperationInoutputVariableRelationship? left, OperationInoutputVariableRelationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), SourceId?.GetHashCode(), TargetId?.GetHashCode(), Target?.GetHashCode());
        }

        public override bool Equals(BasicRelationship? other)
        {
            return Equals(other as OperationInoutputVariableRelationship) || new RelationshipEqualityComparer().Equals(this, other);
        }
    }
}