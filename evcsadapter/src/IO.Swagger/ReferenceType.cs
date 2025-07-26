namespace IoT.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReferenceType
    {
        [EnumMember(Value = "GlobalReference"),]
        GlobalReference,
        [EnumMember(Value = "ModelReference"),]
        ModelReference
    }
}