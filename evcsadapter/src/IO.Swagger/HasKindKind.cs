namespace IoT.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HasKindKind
    {
        [EnumMember(Value = "Template"), ]
        Template,
        [EnumMember(Value = "Instance"), ]
        Instance
    }
}