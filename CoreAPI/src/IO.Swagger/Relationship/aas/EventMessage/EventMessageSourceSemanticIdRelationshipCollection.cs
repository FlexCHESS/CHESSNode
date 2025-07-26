namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class EventMessageSourceSemanticIdRelationshipCollection : RelationshipCollection<EventMessageSourceSemanticIdRelationship, Reference>
    {
        public EventMessageSourceSemanticIdRelationshipCollection(IEnumerable<EventMessageSourceSemanticIdRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<EventMessageSourceSemanticIdRelationship>())
        {
        }
    }
}