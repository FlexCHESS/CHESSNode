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
          /// Gets or Sets AssetKind
          /// </summary>
          [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
          public enum AssetKind
          {
              /// <summary>
              /// Enum InstanceEnum for Instance
              /// </summary>
              [EnumMember(Value = "Instance")]
              InstanceEnum = 0,
              /// <summary>
              /// Enum NotApplicableEnum for NotApplicable
              /// </summary>
              [EnumMember(Value = "NotApplicable")]
              NotApplicableEnum = 1,
              /// <summary>
              /// Enum TypeEnum for Type
              /// </summary>
              [EnumMember(Value = "Type")]
              TypeEnum = 2          }
}
