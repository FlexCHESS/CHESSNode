/*
 * DotAAS Part 2 | HTTP/REST | Entire API Collection
 *
 * All APIs of the Specification of the [Specification of the Asset Administration Shell: Part 2](http://industrialdigitaltwin.org/en/content-hub) in one collection. Please not that this API is not intended to generate productive code but only for overview purposes.   Publisher: Industrial Digital Twin Association (IDTA) 2023\"
 *
 * OpenAPI spec version: V3.0.1
 * Contact: info@idtwin.org
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Models
{
  
          /// <summary>
          /// Gets or Sets AasSubmodelElements
          /// </summary>
          [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
          public enum AasSubmodelElements
          {
              /// <summary>
              /// Enum AnnotatedRelationshipElementEnum for AnnotatedRelationshipElement
              /// </summary>
              [EnumMember(Value = "AnnotatedRelationshipElement")]
              AnnotatedRelationshipElementEnum = 0,
              /// <summary>
              /// Enum BasicEventElementEnum for BasicEventElement
              /// </summary>
              [EnumMember(Value = "BasicEventElement")]
              BasicEventElementEnum = 1,
              /// <summary>
              /// Enum BlobEnum for Blob
              /// </summary>
              [EnumMember(Value = "Blob")]
              BlobEnum = 2,
              /// <summary>
              /// Enum CapabilityEnum for Capability
              /// </summary>
              [EnumMember(Value = "Capability")]
              CapabilityEnum = 3,
              /// <summary>
              /// Enum DataElementEnum for DataElement
              /// </summary>
              [EnumMember(Value = "DataElement")]
              DataElementEnum = 4,
              /// <summary>
              /// Enum EntityEnum for Entity
              /// </summary>
              [EnumMember(Value = "Entity")]
              EntityEnum = 5,
              /// <summary>
              /// Enum EventElementEnum for EventElement
              /// </summary>
              [EnumMember(Value = "EventElement")]
              EventElementEnum = 6,
              /// <summary>
              /// Enum FileEnum for File
              /// </summary>
              [EnumMember(Value = "File")]
              FileEnum = 7,
              /// <summary>
              /// Enum MultiLanguagePropertyEnum for MultiLanguageProperty
              /// </summary>
              [EnumMember(Value = "MultiLanguageProperty")]
              MultiLanguagePropertyEnum = 8,
              /// <summary>
              /// Enum OperationEnum for Operation
              /// </summary>
              [EnumMember(Value = "Operation")]
              OperationEnum = 9,
              /// <summary>
              /// Enum PropertyEnum for Property
              /// </summary>
              [EnumMember(Value = "Property")]
              PropertyEnum = 10,
              /// <summary>
              /// Enum RangeEnum for Range
              /// </summary>
              [EnumMember(Value = "Range")]
              RangeEnum = 11,
              /// <summary>
              /// Enum ReferenceElementEnum for ReferenceElement
              /// </summary>
              [EnumMember(Value = "ReferenceElement")]
              ReferenceElementEnum = 12,
              /// <summary>
              /// Enum RelationshipElementEnum for RelationshipElement
              /// </summary>
              [EnumMember(Value = "RelationshipElement")]
              RelationshipElementEnum = 13,
              /// <summary>
              /// Enum SubmodelElementEnum for SubmodelElement
              /// </summary>
              [EnumMember(Value = "SubmodelElement")]
              SubmodelElementEnum = 14,
              /// <summary>
              /// Enum SubmodelElementCollectionEnum for SubmodelElementCollection
              /// </summary>
              [EnumMember(Value = "SubmodelElementCollection")]
              SubmodelElementCollectionEnum = 15,
              /// <summary>
              /// Enum SubmodelElementListEnum for SubmodelElementList
              /// </summary>
              [EnumMember(Value = "SubmodelElementList")]
              SubmodelElementListEnum = 16          }
}
