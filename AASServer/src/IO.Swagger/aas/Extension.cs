namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Extension : HasSemantics, IEquatable<Extension>
    {
        public Extension()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:Extension;1";
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("valueType")]
        public string? ValueType { get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; }
        [JsonIgnore]
        public ExtensionRefersToRelationshipCollection RefersTo { get; set; } = new ExtensionRefersToRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as Extension);
        }

        public bool Equals(Extension? other)
        {
            return other is not null && base.Equals(other) && Name == other.Name && ValueType == other.ValueType && Value == other.Value;
        }

        public static bool operator ==(Extension? left, Extension? right)
        {
            return EqualityComparer<Extension?>.Default.Equals(left, right);
        }

        public static bool operator !=(Extension? left, Extension? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), Name?.GetHashCode(), ValueType?.GetHashCode(), Value?.GetHashCode());
        }
    }
}