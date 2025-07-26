namespace IoT.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DataSpecificationIEC61360LevelType
    {
        [EnumMember(Value = "Min"), ]
        Min,
        [EnumMember(Value = "Max"), ]
        Max,
        [EnumMember(Value = "Nom"), ]
        Nom,
        [EnumMember(Value = "Typ"), ]
        Typ
    }
}