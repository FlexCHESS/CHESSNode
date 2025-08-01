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
          /// Gets or Sets ExecutionState
          /// </summary>
          [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
          public enum ExecutionState
          {
              /// <summary>
              /// Enum InitiatedEnum for Initiated
              /// </summary>
              [EnumMember(Value = "Initiated")]
              InitiatedEnum = 0,
              /// <summary>
              /// Enum RunningEnum for Running
              /// </summary>
              [EnumMember(Value = "Running")]
              RunningEnum = 1,
              /// <summary>
              /// Enum CompletedEnum for Completed
              /// </summary>
              [EnumMember(Value = "Completed")]
              CompletedEnum = 2,
              /// <summary>
              /// Enum CanceledEnum for Canceled
              /// </summary>
              [EnumMember(Value = "Canceled")]
              CanceledEnum = 3,
              /// <summary>
              /// Enum FailedEnum for Failed
              /// </summary>
              [EnumMember(Value = "Failed")]
              FailedEnum = 4,
              /// <summary>
              /// Enum TimeoutEnum for Timeout
              /// </summary>
              [EnumMember(Value = "Timeout")]
              TimeoutEnum = 5          }
}
