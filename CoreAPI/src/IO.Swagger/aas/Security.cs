namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Security : BasicDigitalTwin, IEquatable<Security>, IEquatable<BasicDigitalTwin>
    {
        public Security()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:Security;1";
        [JsonIgnore]
        public SecurityAccessControlPolicyPointsRelationshipCollection AccessControlPolicyPoints { get; set; } = new SecurityAccessControlPolicyPointsRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Security);
        }

        public bool Equals(Security? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(Security? left, Security? right)
        {
            return EqualityComparer<Security?>.Default.Equals(left, right);
        }

        public static bool operator !=(Security? left, Security? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Security) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}