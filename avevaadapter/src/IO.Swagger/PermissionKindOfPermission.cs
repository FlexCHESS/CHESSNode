namespace IoT.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PermissionKindOfPermission
    {
        [EnumMember(Value = "Allow"), Display(Name = "Allow", Description = "Allow the permission given to the subject.")]
        Allow,
        [EnumMember(Value = "Deny"), Display(Name = "Deny", Description = "Explicitly deny the permission given to the subject.")]
        Deny,
        [EnumMember(Value = "NotApplicable"), Display(Name = "Not applicable", Description = "The permission is not applicable to the subject.")]
        NotApplicable,
        [EnumMember(Value = "Undefined"), Display(Name = "Deny", Description = "Explicitly deny the permission given to the subject.")]
        Undefined
    }
}