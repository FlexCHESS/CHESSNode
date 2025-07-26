namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class MultiLanguagePropertyValueIdRelationshipCollection : RelationshipCollection<MultiLanguagePropertyValueIdRelationship, Reference>
    {
        public MultiLanguagePropertyValueIdRelationshipCollection(IEnumerable<MultiLanguagePropertyValueIdRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<MultiLanguagePropertyValueIdRelationship>())
        {
        }
    }
}