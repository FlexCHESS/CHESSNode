namespace IoT.Services
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum KeyType
    {
        [EnumMember(Value = "Referable"), Display(Name = "Referable", Description = " Referable is abstract, i.e. if a key uses “Referable” the reference may be an Asset Administration Shell, a Property etc."), SourceValue(Value = "AAS Key type class: AasReferables")]
        Referable,
        [EnumMember(Value = "FragmentReference"), Display(Name = "Fragment Reference", Description = "Bookmark or a similar local identifier of a subordinate part of a primary resource"), SourceValue(Value = "AAS Key type class: GenericFragmentKeys")]
        FragmentReference,
        [EnumMember(Value = "GlobalReference"), Display(Name = "Global Reference", Description = "Global reference"), SourceValue(Value = "AAS Key type class: GenericGloballyIdentifiables")]
        GlobalReference,
        [EnumMember(Value = "AssetAdministrationShell"), Display(Name = "Asset Administration Shell", Description = "Asset administration shell"), SourceValue(Value = "AAS Key type class: AasIdentifiables")]
        AssetAdministrationShell,
        [EnumMember(Value = "ConceptDescription"), Display(Name = "Concept Description", Description = "Concept description"), SourceValue(Value = "AAS Key type class: AasIdentifiables")]
        ConceptDescription,
        [EnumMember(Value = "Identifiable"), Display(Name = "Identifiable", Description = "Identifiable"), SourceValue(Value = "AAS Key type class: AasIdentifiables")]
        Identifiable,
        [EnumMember(Value = "Submodel"), Display(Name = "Submodel", Description = "Submodel"), SourceValue(Value = "AAS Key type class: AasIdentifiables")]
        Submodel,
        [EnumMember(Value = "AnnotatedRelationshipElement"), Display(Name = "Annotated Relationship Element", Description = "Annotated relationship element"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        AnnotatedRelationshipElement,
        [EnumMember(Value = "BasicEventElement"), Display(Name = "Basic Event Element", Description = "Basic Event Element"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        BasicEventElement,
        [EnumMember(Value = "Blob"), Display(Name = "Blob", Description = "Blob"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        Blob,
        [EnumMember(Value = "Capability"), Display(Name = "Capability", Description = "Capability"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        Capability,
        [EnumMember(Value = "DataElement"), Display(Name = "Data Element", Description = "Data element"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        DataElement,
        [EnumMember(Value = "Entity"), Display(Name = "Entity", Description = "Entity"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        Entity,
        [EnumMember(Value = "EventElement"), Display(Name = "Event Element", Description = "Event element"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        EventElement,
        [EnumMember(Value = "File"), Display(Name = "File", Description = "File"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        File,
        [EnumMember(Value = "MultiLanguageProperty"), Display(Name = "Multi Language Property", Description = "Property with a value that can be provided in multiple languages"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        MultiLanguageProperty,
        [EnumMember(Value = "Operation"), Display(Name = "Operation", Description = "Operation"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        Operation,
        [EnumMember(Value = "Property"), Display(Name = "Property", Description = "Property"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        Property,
        [EnumMember(Value = "Range"), Display(Name = "Range", Description = "Range with min and max"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        Range,
        [EnumMember(Value = "ReferenceElement"), Display(Name = "Reference Element", Description = "Reference"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        ReferenceElement,
        [EnumMember(Value = "RelationshipElement"), Display(Name = "Relationship Element", Description = "Relationship"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        RelationshipElement,
        [EnumMember(Value = "SubmodelElement"), Display(Name = "Submodel Element", Description = "Submodel element"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        SubmodelElement,
        [EnumMember(Value = "SubmodelElementCollection"), Display(Name = "Submodel Element Collection", Description = "Struct of submodel elements"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        SubmodelElementCollection,
        [EnumMember(Value = "SubmodelElementList"), Display(Name = "Submodel Element List", Description = "list of submodel elements"), SourceValue(Value = "AAS Key type class: AasSubmodelElements")]
        SubmodelElementList
    }
}