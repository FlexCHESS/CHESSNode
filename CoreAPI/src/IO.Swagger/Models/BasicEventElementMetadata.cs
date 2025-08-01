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
    /// 
    /// </summary>
    [DataContract]
    public partial class BasicEventElementMetadata : SubmodelElementAttributes, IEquatable<BasicEventElementMetadata>, OneOfSubmodelElementMetadata 
    { 
        /// <summary>
        /// Gets or Sets Direction
        /// </summary>

        [DataMember(Name="direction")]
        public Direction Direction { get; set; }

        /// <summary>
        /// Gets or Sets State
        /// </summary>

        [DataMember(Name="state")]
        public StateOfEvent State { get; set; }

        /// <summary>
        /// Gets or Sets MessageTopic
        /// </summary>

        [MaxLength(255)]
        [DataMember(Name="messageTopic")]
        public string MessageTopic { get; set; }

        /// <summary>
        /// Gets or Sets MessageBroker
        /// </summary>

        [DataMember(Name="messageBroker")]
        public Reference MessageBroker { get; set; }

        /// <summary>
        /// Gets or Sets LastUpdate
        /// </summary>

        [DataMember(Name="lastUpdate")]
        public string LastUpdate { get; set; }

        /// <summary>
        /// Gets or Sets MinInterval
        /// </summary>

        [DataMember(Name="minInterval")]
        public string MinInterval { get; set; }

        /// <summary>
        /// Gets or Sets MaxInterval
        /// </summary>

        [DataMember(Name="maxInterval")]
        public string MaxInterval { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BasicEventElementMetadata {\n");
            sb.Append("  Direction: ").Append(Direction).Append("\n");
            sb.Append("  State: ").Append(State).Append("\n");
            sb.Append("  MessageTopic: ").Append(MessageTopic).Append("\n");
            sb.Append("  MessageBroker: ").Append(MessageBroker).Append("\n");
            sb.Append("  LastUpdate: ").Append(LastUpdate).Append("\n");
            sb.Append("  MinInterval: ").Append(MinInterval).Append("\n");
            sb.Append("  MaxInterval: ").Append(MaxInterval).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public  new string ToJson()
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
            return obj.GetType() == GetType() && Equals((BasicEventElementMetadata)obj);
        }

        /// <summary>
        /// Returns true if BasicEventElementMetadata instances are equal
        /// </summary>
        /// <param name="other">Instance of BasicEventElementMetadata to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BasicEventElementMetadata other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Direction == other.Direction ||
                    Direction != null &&
                    Direction.Equals(other.Direction)
                ) && 
                (
                    State == other.State ||
                    State != null &&
                    State.Equals(other.State)
                ) && 
                (
                    MessageTopic == other.MessageTopic ||
                    MessageTopic != null &&
                    MessageTopic.Equals(other.MessageTopic)
                ) && 
                (
                    MessageBroker == other.MessageBroker ||
                    MessageBroker != null &&
                    MessageBroker.Equals(other.MessageBroker)
                ) && 
                (
                    LastUpdate == other.LastUpdate ||
                    LastUpdate != null &&
                    LastUpdate.Equals(other.LastUpdate)
                ) && 
                (
                    MinInterval == other.MinInterval ||
                    MinInterval != null &&
                    MinInterval.Equals(other.MinInterval)
                ) && 
                (
                    MaxInterval == other.MaxInterval ||
                    MaxInterval != null &&
                    MaxInterval.Equals(other.MaxInterval)
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
                    if (Direction != null)
                    hashCode = hashCode * 59 + Direction.GetHashCode();
                    if (State != null)
                    hashCode = hashCode * 59 + State.GetHashCode();
                    if (MessageTopic != null)
                    hashCode = hashCode * 59 + MessageTopic.GetHashCode();
                    if (MessageBroker != null)
                    hashCode = hashCode * 59 + MessageBroker.GetHashCode();
                    if (LastUpdate != null)
                    hashCode = hashCode * 59 + LastUpdate.GetHashCode();
                    if (MinInterval != null)
                    hashCode = hashCode * 59 + MinInterval.GetHashCode();
                    if (MaxInterval != null)
                    hashCode = hashCode * 59 + MaxInterval.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(BasicEventElementMetadata left, BasicEventElementMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BasicEventElementMetadata left, BasicEventElementMetadata right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
