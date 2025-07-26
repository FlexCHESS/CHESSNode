namespace IoT.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BasicEventElementState
    {
        [EnumMember(Value = "on"), ]
        on,
        [EnumMember(Value = "off"), ]
        off
    }
}