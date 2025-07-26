namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Qualifiable : BasicDigitalTwin, IEquatable<Qualifiable>, IEquatable<BasicDigitalTwin>
    {
        public Qualifiable()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static string ModelId { get; } = "dtmi:digitaltwins:aas:Qualifiable;1";
        [JsonIgnore]
        public QualifiableQualifierRelationshipCollection Qualifier { get; set; } = new QualifiableQualifierRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Qualifiable);
        }

        public bool Equals(Qualifiable? other)
        {
            return other is not null && Id == other.Id && Metadata.ModelId == other.Metadata.ModelId;
        }

        public static bool operator ==(Qualifiable? left, Qualifiable? right)
        {
            return EqualityComparer<Qualifiable?>.Default.Equals(left, right);
        }

        public static bool operator !=(Qualifiable? left, Qualifiable? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(Id?.GetHashCode(), Metadata?.ModelId?.GetHashCode());
        }

        public bool Equals(BasicDigitalTwin? other)
        {
            return Equals(other as Qualifiable) || new TwinEqualityComparer().Equals(this, other);
        }
    }
}