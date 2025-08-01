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
    public partial class AssetAdministrationShellDescriptor : Descriptor, IEquatable<AssetAdministrationShellDescriptor>
    { 
        /// <summary>
        /// Gets or Sets Administration
        /// </summary>

        [DataMember(Name="administration")]
        public AdministrativeInformation Administration { get; set; }

        /// <summary>
        /// Gets or Sets AssetKind
        /// </summary>

        [DataMember(Name="assetKind")]
        public AssetKind AssetKind { get; set; }

        /// <summary>
        /// Gets or Sets AssetType
        /// </summary>
        [RegularExpression("/^[\\x09\\x0A\\x0D\\x20-\\uD7FF\\uE000-\\uFFFD\\U00010000-\\U0010FFFF]*$/")]
        [StringLength(2000, MinimumLength=1)]
        [DataMember(Name="assetType")]
        public string AssetType { get; set; }

        /// <summary>
        /// Gets or Sets Endpoints
        /// </summary>

        [DataMember(Name="endpoints")]
        public List<Endpoint> Endpoints { get; set; }

        /// <summary>
        /// Gets or Sets GlobalAssetId
        /// </summary>
        [RegularExpression("/^[\\x09\\x0A\\x0D\\x20-\\uD7FF\\uE000-\\uFFFD\\U00010000-\\U0010FFFF]*$/")]
        [StringLength(2000, MinimumLength=1)]
        [DataMember(Name="globalAssetId")]
        public string GlobalAssetId { get; set; }

        /// <summary>
        /// Gets or Sets IdShort
        /// </summary>

        [MaxLength(128)]
        [DataMember(Name="idShort")]
        public string IdShort { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Required]
        [RegularExpression("/^[\\x09\\x0A\\x0D\\x20-\\uD7FF\\uE000-\\uFFFD\\U00010000-\\U0010FFFF]*$/")]
        [StringLength(2000, MinimumLength=1)]
        [DataMember(Name="id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets SpecificAssetIds
        /// </summary>

        [DataMember(Name="specificAssetIds")]
        public List<SpecificAssetId> SpecificAssetIds { get; set; }

        /// <summary>
        /// Gets or Sets SubmodelDescriptors
        /// </summary>

        [DataMember(Name="submodelDescriptors")]
        public List<SubmodelDescriptor> SubmodelDescriptors { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AssetAdministrationShellDescriptor {\n");
            sb.Append("  Administration: ").Append(Administration).Append("\n");
            sb.Append("  AssetKind: ").Append(AssetKind).Append("\n");
            sb.Append("  AssetType: ").Append(AssetType).Append("\n");
            sb.Append("  Endpoints: ").Append(Endpoints).Append("\n");
            sb.Append("  GlobalAssetId: ").Append(GlobalAssetId).Append("\n");
            sb.Append("  IdShort: ").Append(IdShort).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  SpecificAssetIds: ").Append(SpecificAssetIds).Append("\n");
            sb.Append("  SubmodelDescriptors: ").Append(SubmodelDescriptors).Append("\n");
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
            return obj.GetType() == GetType() && Equals((AssetAdministrationShellDescriptor)obj);
        }

        /// <summary>
        /// Returns true if AssetAdministrationShellDescriptor instances are equal
        /// </summary>
        /// <param name="other">Instance of AssetAdministrationShellDescriptor to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AssetAdministrationShellDescriptor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Administration == other.Administration ||
                    Administration != null &&
                    Administration.Equals(other.Administration)
                ) && 
                (
                    AssetKind == other.AssetKind ||
                    AssetKind != null &&
                    AssetKind.Equals(other.AssetKind)
                ) && 
                (
                    AssetType == other.AssetType ||
                    AssetType != null &&
                    AssetType.Equals(other.AssetType)
                ) && 
                (
                    Endpoints == other.Endpoints ||
                    Endpoints != null &&
                    Endpoints.SequenceEqual(other.Endpoints)
                ) && 
                (
                    GlobalAssetId == other.GlobalAssetId ||
                    GlobalAssetId != null &&
                    GlobalAssetId.Equals(other.GlobalAssetId)
                ) && 
                (
                    IdShort == other.IdShort ||
                    IdShort != null &&
                    IdShort.Equals(other.IdShort)
                ) && 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) && 
                (
                    SpecificAssetIds == other.SpecificAssetIds ||
                    SpecificAssetIds != null &&
                    SpecificAssetIds.SequenceEqual(other.SpecificAssetIds)
                ) && 
                (
                    SubmodelDescriptors == other.SubmodelDescriptors ||
                    SubmodelDescriptors != null &&
                    SubmodelDescriptors.SequenceEqual(other.SubmodelDescriptors)
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
                    if (Administration != null)
                    hashCode = hashCode * 59 + Administration.GetHashCode();
                    if (AssetKind != null)
                    hashCode = hashCode * 59 + AssetKind.GetHashCode();
                    if (AssetType != null)
                    hashCode = hashCode * 59 + AssetType.GetHashCode();
                    if (Endpoints != null)
                    hashCode = hashCode * 59 + Endpoints.GetHashCode();
                    if (GlobalAssetId != null)
                    hashCode = hashCode * 59 + GlobalAssetId.GetHashCode();
                    if (IdShort != null)
                    hashCode = hashCode * 59 + IdShort.GetHashCode();
                    if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                    if (SpecificAssetIds != null)
                    hashCode = hashCode * 59 + SpecificAssetIds.GetHashCode();
                    if (SubmodelDescriptors != null)
                    hashCode = hashCode * 59 + SubmodelDescriptors.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(AssetAdministrationShellDescriptor left, AssetAdministrationShellDescriptor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AssetAdministrationShellDescriptor left, AssetAdministrationShellDescriptor right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
