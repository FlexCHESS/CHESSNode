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
    public partial class ConceptDescription : Identifiable, IEquatable<ConceptDescription>
    { 
        /// <summary>
        /// Gets or Sets EmbeddedDataSpecifications
        /// </summary>

        [DataMember(Name="embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; }

        /// <summary>
        /// Gets or Sets IsCaseOf
        /// </summary>

        [DataMember(Name="isCaseOf")]
        public List<Reference> IsCaseOf { get; set; }

        /// <summary>
        /// Gets or Sets ModelType
        /// </summary>
        [RegularExpression("/ConceptDescription/")]
        [DataMember(Name="modelType")]
        public string ModelType { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ConceptDescription {\n");
            sb.Append("  EmbeddedDataSpecifications: ").Append(EmbeddedDataSpecifications).Append("\n");
            sb.Append("  IsCaseOf: ").Append(IsCaseOf).Append("\n");
            sb.Append("  ModelType: ").Append(ModelType).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ConceptDescription)obj);
        }

        /// <summary>
        /// Returns true if ConceptDescription instances are equal
        /// </summary>
        /// <param name="other">Instance of ConceptDescription to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ConceptDescription other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    EmbeddedDataSpecifications == other.EmbeddedDataSpecifications ||
                    EmbeddedDataSpecifications != null &&
                    EmbeddedDataSpecifications.SequenceEqual(other.EmbeddedDataSpecifications)
                ) && 
                (
                    IsCaseOf == other.IsCaseOf ||
                    IsCaseOf != null &&
                    IsCaseOf.SequenceEqual(other.IsCaseOf)
                ) && 
                (
                    ModelType == other.ModelType ||
                    ModelType != null &&
                    ModelType.Equals(other.ModelType)
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
                    if (EmbeddedDataSpecifications != null)
                    hashCode = hashCode * 59 + EmbeddedDataSpecifications.GetHashCode();
                    if (IsCaseOf != null)
                    hashCode = hashCode * 59 + IsCaseOf.GetHashCode();
                    if (ModelType != null)
                    hashCode = hashCode * 59 + ModelType.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ConceptDescription left, ConceptDescription right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ConceptDescription left, ConceptDescription right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
