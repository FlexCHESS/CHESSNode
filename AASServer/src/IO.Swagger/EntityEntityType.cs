namespace IoT.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EntityEntityType
    {
        [EnumMember(Value = "CoManagedEntity"), Display(Name = "Co managed entity", Description = "For co-managed entities there is no separate AAS. Co-managed entities need to be part of a self-managed entity.")]
        CoManagedEntity,
        [EnumMember(Value = "SelfManagedEntity"), Display(Name = "self managed entity", Description = "Self-Managed Entities have their own AAS but can be part of the bill of material of a composite self-managed entity.")]
        SelfManagedEntity
    }
}