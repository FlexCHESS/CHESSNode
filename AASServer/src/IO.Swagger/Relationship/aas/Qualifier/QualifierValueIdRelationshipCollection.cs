namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class QualifierValueIdRelationshipCollection : RelationshipCollection<QualifierValueIdRelationship, Reference>
    {
        public QualifierValueIdRelationshipCollection(IEnumerable<QualifierValueIdRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<QualifierValueIdRelationship>())
        {
        }
    }
}