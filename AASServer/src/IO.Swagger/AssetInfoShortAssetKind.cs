namespace IoT.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AssetInfoShortAssetKind
    {
        [EnumMember(Value = "Type"), ]
        Type,
        [EnumMember(Value = "Instance"), ]
        Instance
    }
}