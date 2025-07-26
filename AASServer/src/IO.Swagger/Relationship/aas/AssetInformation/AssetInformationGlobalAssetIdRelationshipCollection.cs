namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class AssetInformationGlobalAssetIdRelationshipCollection : RelationshipCollection<AssetInformationGlobalAssetIdRelationship, Reference>
    {
        public AssetInformationGlobalAssetIdRelationshipCollection(IEnumerable<AssetInformationGlobalAssetIdRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<AssetInformationGlobalAssetIdRelationship>())
        {
        }
    }
}