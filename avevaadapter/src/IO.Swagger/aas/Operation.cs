namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Operation : SubmodelElement, IEquatable<Operation>
    {
        public Operation()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Operation;1";
        [JsonIgnore]
        public OperationInputVariableRelationshipCollection InputVariable { get; set; } = new OperationInputVariableRelationshipCollection();
        [JsonIgnore]
        public OperationOutputVariableRelationshipCollection OutputVariable { get; set; } = new OperationOutputVariableRelationshipCollection();
        [JsonIgnore]
        public OperationInoutputVariableRelationshipCollection InoutputVariable { get; set; } = new OperationInoutputVariableRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Operation);
        }

        public bool Equals(Operation? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(Operation? left, Operation? right)
        {
            return EqualityComparer<Operation?>.Default.Equals(left, right);
        }

        public static bool operator !=(Operation? left, Operation? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}