namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class OperationVariable : BasicDigitalTwin, IEquatable<OperationVariable>, IEquatable<BasicDigitalTwin>
    {
        public OperationVariable()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:OperationVariable;1";
        [JsonIgnore]
        public OperationVariableValueRelationshipCollection Value { get; set; } = new OperationVariableValueRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as OperationVariable);
        }

        public bool Equals(OperationVariable? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(OperationVariable? left, OperationVariable? right)
        {
            return EqualityComparer<OperationVariable?>.Default.Equals(left, right);
        }

        public static bool operator !=(OperationVariable? left, OperationVariable? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as OperationVariable) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}