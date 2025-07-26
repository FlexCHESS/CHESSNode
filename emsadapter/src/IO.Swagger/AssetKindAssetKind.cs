namespace Generator.Tests.Generated
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AssetKindAssetKind
    {
        [EnumMember(Value = "Type"), ]
        Type,
        [EnumMember(Value = "Instance"), ]
        Instance
    }
}