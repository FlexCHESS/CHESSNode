namespace IoT.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BasicEventElementDirection
    {
        [EnumMember(Value = "input"), ]
        input,
        [EnumMember(Value = "output"), ]
        output
    }
}