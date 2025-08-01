/*
 * Charging Station Monitoring API
 *
 * Single endpoint to receive bulk updates from a Charging Station Management System (CSMS). The server is expected to update its internal database based on the differential changes in the data and respond with a list of load curves (one per charging station). A call to this endpoint is made as soon as new data is available or if not data is available on a set interval. 
 *
 * OpenAPI spec version: 1.0.0
 * 
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
    /// A load-curve object corresponding to a single Charging Station. The server may return either &#x60;station_limit&#x60; or &#x60;evse_limit&#x60;. 
    /// </summary>
    [DataContract]
    public partial class LoadCurve : IEquatable<LoadCurve>, OneOfLoadCurve 
    { 
        /// <summary>
        /// Reference to the Charging Station ID
        /// </summary>
        /// <value>Reference to the Charging Station ID</value>
        [Required]

        [DataMember(Name="csid")]
        public string Csid { get; set; }

        /// <summary>
        /// Array of station-wide limit points (if the station-level limits apply). The station itself will apply local load balancing only when activated and available. 
        /// </summary>
        /// <value>Array of station-wide limit points (if the station-level limits apply). The station itself will apply local load balancing only when activated and available. </value>

      //  [DataMember(Name="station_limit")]
      //  public List<LoadPoint> StationLimit { get; set; }

        /// <summary>
        /// Array of per-EVSE limits, each with its own set of points (if EVSE-level limits apply). The station itself will NOT apply local load balancing only when not activated or not available. 
        /// </summary>
        /// <value>Array of per-EVSE limits, each with its own set of points (if EVSE-level limits apply). The station itself will NOT apply local load balancing only when not activated or not available. </value>

        [DataMember(Name="evse_limit")]
        public List<EvseLimit> EvseLimit { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class LoadCurve {\n");
            sb.Append("  Csid: ").Append(Csid).Append("\n");
            //sb.Append("  StationLimit: ").Append(StationLimit).Append("\n");
            sb.Append("  EvseLimit: ").Append(EvseLimit).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((LoadCurve)obj);
        }

        /// <summary>
        /// Returns true if LoadCurve instances are equal
        /// </summary>
        /// <param name="other">Instance of LoadCurve to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(LoadCurve other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Csid == other.Csid ||
                    Csid != null &&
                    Csid.Equals(other.Csid)
                ) && 
               // (
               //     StationLimit == other.StationLimit ||
               //     StationLimit != null &&
               //     StationLimit.SequenceEqual(other.StationLimit)
               // ) && 
                (
                    EvseLimit == other.EvseLimit ||
                    EvseLimit != null &&
                    EvseLimit.SequenceEqual(other.EvseLimit)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Csid != null)
                    hashCode = hashCode * 59 + Csid.GetHashCode();
                 //   if (StationLimit != null)
                 //   hashCode = hashCode * 59 + StationLimit.GetHashCode();
                    if (EvseLimit != null)
                    hashCode = hashCode * 59 + EvseLimit.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(LoadCurve left, LoadCurve right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LoadCurve left, LoadCurve right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
