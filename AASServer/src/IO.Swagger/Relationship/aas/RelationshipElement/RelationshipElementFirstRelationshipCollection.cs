namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class RelationshipElementFirstRelationshipCollection : RelationshipCollection<RelationshipElementFirstRelationship, Reference>
    {
        public RelationshipElementFirstRelationshipCollection(IEnumerable<RelationshipElementFirstRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<RelationshipElementFirstRelationship>())
        {
        }
    }
}