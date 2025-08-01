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
          /// Gets or Sets ReferenceTypes
          /// </summary>
          [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
          public enum ReferenceTypes
          {
              /// <summary>
              /// Enum ExternalReferenceEnum for ExternalReference
              /// </summary>
              [EnumMember(Value = "ExternalReference")]
              ExternalReferenceEnum = 0,
              /// <summary>
              /// Enum ModelReferenceEnum for ModelReference
              /// </summary>
              [EnumMember(Value = "ModelReference")]
              ModelReferenceEnum = 1          }
}
