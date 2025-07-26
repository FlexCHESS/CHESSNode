namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    public class Measurement : Storage, IEquatable<Measurement>
    {
        public Measurement()
        {
            Metadata.ModelId = ModelId;
        }
        [JsonIgnore]
        public static new string ModelId { get; } = "dtmi:com:flexchess:der:meter:Measurement;1";
        [JsonPropertyName("Id")]
        public string? Id { get; set; }
        [JsonPropertyName("timestamp")]
        public string? timestamp { get; set; }

        [JsonPropertyName("clockAligned")]
        public bool? clockAligned { get; set; }
        [JsonPropertyName("currentOffered")]
        public double? currentOffered { get; set; }
        [JsonPropertyName("powerOffered")]
        public double? powerOffered { get; set; }
        [JsonPropertyName("currentImport")]
        public double? currentImport { get; set; }
        [JsonPropertyName("currentImportN")]
        public double? currentImportN { get; set; }
        [JsonPropertyName("currentImportL1")]
        public double? currentImportL1 { get; set; }
        [JsonPropertyName("currentImportL2")]
        public double? currentImportL2 { get; set; }
        [JsonPropertyName("currentImportL3")]
        public double? currentImportL3 { get; set; }
        [JsonPropertyName("powerActiveImport")]
        public double? powerActiveImport { get; set; }
        [JsonPropertyName("powerActiveImportL1")]
        public double? powerActiceImportL1 { get; set; }
        [JsonPropertyName("powerActiveImportL2")]
        public double? powerActiceImportL2 { get; set; }
        [JsonPropertyName("powerActiveImportL3")]
        public double? powerActiceImportL3 { get; set; }
        [JsonPropertyName("powerReactiveImport")]
        public double? powerReacticeImport { get; set; }
        [JsonPropertyName("powerReactiveImportL1")]
        public double? powerReacticeImportL1 { get; set; }

        [JsonPropertyName("powerReactiveImportL2")]
        public double? powerReacticeImportL2 { get; set; }

        [JsonPropertyName("powerReactiveImportL3")]
        public double? powerReacticeImportL3 { get; set; }

        [JsonPropertyName("energyActiveImportRegister")]
        public double? energyActiceImportRegister { get; set; }

        [JsonPropertyName("energyActiveImportRegisterL1")]
        public double? energyActiceImportRegisterL1 { get; set; }

        [JsonPropertyName("energyActiveImportRegisterL2")]
        public double? energyActiceImportRegisterL2 { get; set; }

        [JsonPropertyName("energyActiveImportRegisterL3")]
        public double? energyActiceImportRegisterL3 { get; set; }

        [JsonPropertyName("energyActiveImportInterval")]
        public double? energyActiceImportInterval { get; set; }

        [JsonPropertyName("energyActiveImportIntervalL1")]
        public double? energyActiceImportIntervalL1 { get; set; }
        [JsonPropertyName("energyActiveImportIntervalL2")]
        public double? energyActiceImportIntervalL2 { get; set; }
        [JsonPropertyName("energyActiveImportIntervalL3")]
        public double? energyActiceImportIntervalL3 { get; set; }
        [JsonPropertyName("energyReactiveImportRegister")]
        public double? energyReacticeImportRegister { get; set; }
        [JsonPropertyName("energyReactiveImportRegisterL1")]
        public double? energyReacticeImportRegisterL1 { get; set; }
        [JsonPropertyName("energyReactiveImportRegisterL2")]
        public double? energyReacticeImportRegisterL2 { get; set; }
        [JsonPropertyName("energyReactiveImportRegisterL3")]
        public double? energyReacticeImportRegisterL3 { get; set; }
        [JsonPropertyName("energyReactiveImportInterval")]
        public double? energyReacticeImportInterval { get; set; }
        [JsonPropertyName("energyReactiveImportIntervalL1")]
        public double? energyReacticeImportIntervalL1 { get; set; }
        [JsonPropertyName("energyReactiveImportIntervalL2")]
        public double? energyReacticeImportIntervalL2 { get; set; }
        [JsonPropertyName("energyReactiveImportIntervalL3")]
        public double? energyReacticeImportIntervalL3 { get; set; }
        [JsonPropertyName("currentExport")]
        public double? currentExport { get; set; }
        [JsonPropertyName("currentExportN")]
        public double? currentExportN { get; set; }
        [JsonPropertyName("currentExportL1")]
        public double? currentExportL1 { get; set; }
        [JsonPropertyName("currentExportL2")]
        public double? currentExportL2 { get; set; }
        [JsonPropertyName("currentExportL3")]
        public double? currentExportL3 { get; set; }
        [JsonPropertyName("powerActiveExport")]
        public double? powerActiveExport { get; set; }
        [JsonPropertyName("powerActiveExportL1")]
        public double? powerActiveExportL1 { get; set; }
        [JsonPropertyName("powerActiveExportL2")]
        public double? powerActiveExportL2 { get; set; }
        [JsonPropertyName("powerActiveExportL3")]
        public double? powerActiveExportL3 { get; set; }
        [JsonPropertyName("powerReactiveExport")]
        public double? powerReactiveExport { get; set; }
        [JsonPropertyName("powerReactiveExportL1")]
        public double? powerReactiveExportL1 { get; set; }
        [JsonPropertyName("powerReactiveExportL2")]
        public double? powerReactiveExportL2 { get; set; }
        [JsonPropertyName("powerReactiveExportL3")]
        public double? powerReactiveExportL3 { get; set; }
        [JsonPropertyName("energyActiveExportRegister")]
        public double? energyActiveExportRegister { get; set; }
        [JsonPropertyName("energyActiveExportRegisterL1")]
        public double? energyActiveExportRegisterL1 { get; set; }
        [JsonPropertyName("energyActiveExportRegisterL2")]
        public double? energyActiveExportRegisterL2 { get; set; }
        [JsonPropertyName("energyActiveExportRegisterL3")]
        public double? energyActiveExportRegisterL3 { get; set; }
        [JsonPropertyName("energyActiveExportInterval")]
        public double? energyActiveExportInterval { get; set; }
        [JsonPropertyName("energyActiveExportIntervalL1")]
        public double? energyActiveExportIntervalL1 { get; set; }
        [JsonPropertyName("energyActiveExportIntervalL2")]
        public double? energyActiveExportIntervalL2 { get; set; }
        [JsonPropertyName("energyActiveExportIntervalL3")]
        public double? energyActiveExportIntervalL3 { get; set; }
        [JsonPropertyName("energyReactiveExportRegister")]
        public double? energyReactiveExportRegister { get; set; }
        [JsonPropertyName("energyReactiveExportRegisterL1")]
        public double? energyReactiveExportRegisterL1 { get; set; }
        [JsonPropertyName("energyReactiveExportRegisterL2")]
        public double? energyReactiveExportRegisterL2 { get; set; }
        [JsonPropertyName("energyReactiveExportRegisterL3")]
        public double? energyReactiveExportRegisterL3 { get; set; }
        [JsonPropertyName("energyReactiveExportInterval")]
        public double? energyReactiveExportInterval { get; set; }
        [JsonPropertyName("energyReactiveExportIntervalL1")]
        public double? energyReactiveExportIntervalL1 { get; set; }
        [JsonPropertyName("energyReactiveExportIntervalL2")]
        public double? energyReactiveExportIntervalL2 { get; set; }
        [JsonPropertyName("energyReactiveExportIntervalL3")]
        public double? energyReactiveExportIntervalL3 { get; set; }
        [JsonPropertyName("voltage")]
        public double? voltage { get; set; }
        [JsonPropertyName("voltageL1N")]
        public double? voltageL1N { get; set; }
        [JsonPropertyName("voltageL2N")]
        public double? voltageL2N { get; set; }
        [JsonPropertyName("voltageL3N")]
        public double? voltageL3N { get; set; }
        [JsonPropertyName("voltageL1L2")]
        public double? voltageL1L2 { get; set; }
        [JsonPropertyName("voltageL2L3")]
        public double? voltageL2L3 { get; set; }
        [JsonPropertyName("voltageL3L1")]
        public double? voltageL3L1 { get; set; }
        [JsonPropertyName("frequency")]
        public double? frequency { get; set; }
        [JsonPropertyName("powerFactor")]
        public double? powerFactor { get; set; }
        [JsonPropertyName("powerFactorL1")]
        public double? powerFactorL1 { get; set; }
        [JsonPropertyName("powerFactorL2")]
        public double? powerFactorL2 { get; set; }
        [JsonPropertyName("powerFactorL3")]
        public double? powerFactorL3 { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Measurement);
        }

        public bool Equals(Measurement? other)
        {
            return other is not null && base.Equals(other);
        }

        public static bool operator ==(Measurement? left, Measurement? right)
        {
            return EqualityComparer<Measurement?>.Default.Equals(left, right);
        }

        public static bool operator !=(Measurement? left, Measurement? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.CustomHash(base.GetHashCode());
        }
    }
}