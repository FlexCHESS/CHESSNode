namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class AssetInformationSpecificAssetIdRelationshipCollection : RelationshipCollection<AssetInformationSpecificAssetIdRelationship, SpecificAssetId>
    {
        public AssetInformationSpecificAssetIdRelationshipCollection(IEnumerable<AssetInformationSpecificAssetIdRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<AssetInformationSpecificAssetIdRelationship>())
        {
        }
    }
}