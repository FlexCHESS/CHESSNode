namespace IoT.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QualifierKind
    {
        [EnumMember(Value = "ValueQualifier"), ]
        ValueQualifier,
        [EnumMember(Value = "ConceptQualifier"), ]
        ConceptQualifier,
        [EnumMember(Value = "TemplateQualifier"), ]
        TemplateQualifier
    }
}