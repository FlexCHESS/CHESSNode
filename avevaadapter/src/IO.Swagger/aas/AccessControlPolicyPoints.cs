namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class AccessControlPolicyPoints : BasicDigitalTwin, IEquatable<AccessControlPolicyPoints>, IEquatable<BasicDigitalTwin>
    {
        public AccessControlPolicyPoints()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:AccessControlPolicyPoints;1";
        [JsonIgnore]
        public AccessControlPolicyPointsPolicyAdministrationPointRelationshipCollection PolicyAdministrationPoint { get; set; } = new AccessControlPolicyPointsPolicyAdministrationPointRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as AccessControlPolicyPoints);
        }

        public bool Equals(AccessControlPolicyPoints? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(AccessControlPolicyPoints? left, AccessControlPolicyPoints? right)
        {
            return EqualityComparer<AccessControlPolicyPoints?>.Default.Equals(left, right);
        }

        public static bool operator !=(AccessControlPolicyPoints? left, AccessControlPolicyPoints? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as AccessControlPolicyPoints) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}