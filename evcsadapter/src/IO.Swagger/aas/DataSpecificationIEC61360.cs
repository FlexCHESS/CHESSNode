namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class DataSpecificationIEC61360 : DataSpecificationContent, IEquatable<DataSpecificationIEC61360>
    {
        public DataSpecificationIEC61360()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:digitaltwins:aas:DataSpecificationIEC61360;1";
        [JsonPropertyName("unit")]
        public string? Unit { get; set; }
        [JsonPropertyName("unitIdValue")]
        public string? UnitIdValue { get; set; }
        [JsonPropertyName("sourceOfDefinition")]
        public string? SourceOfDefinition { get; set; }
        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
        [JsonPropertyName("dataType")]
        public DataSpecificationIEC61360DataType? DataType { get; set; }
        [JsonPropertyName("valueFormat")]
        public string? ValueFormat { get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; }
        [JsonPropertyName("levelType")]
        public DataSpecificationIEC61360LevelType? LevelType { get; set; }
        [JsonIgnore]
        public DataSpecificationIEC61360UnitIdRelationshipCollection UnitId { get; set; } = new DataSpecificationIEC61360UnitIdRelationshipCollection();
        [JsonIgnore]
        public DataSpecificationIEC61360ValueListRelationshipCollection ValueList { get; set; } = new DataSpecificationIEC61360ValueListRelationshipCollection();
        public override bool Equals(object? obj)
        {
            return Equals(obj as DataSpecificationIEC61360);
        }

        public bool Equals(DataSpecificationIEC61360? other)
        {
            return other is not null && base.Equals(other) && Unit == other.Unit && UnitIdValue == other.UnitIdValue && SourceOfDefinition == other.SourceOfDefinition && Symbol == other.Symbol && DataType == other.DataType && ValueFormat == other.ValueFormat && Value == other.Value && LevelType == other.LevelType;
        }

        public static bool operator ==(DataSpecificationIEC61360? left, DataSpecificationIEC61360? right)
        {
            return EqualityComparer<DataSpecificationIEC61360?>.Default.Equals(left, right);
        }

        public static bool operator !=(DataSpecificationIEC61360? left, DataSpecificationIEC61360? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode(), Unit?.GetHashCode(), UnitIdValue?.GetHashCode(), SourceOfDefinition?.GetHashCode(), Symbol?.GetHashCode(), DataType?.GetHashCode(), ValueFormat?.GetHashCode(), Value?.GetHashCode(), LevelType?.GetHashCode());
        }
    }
}