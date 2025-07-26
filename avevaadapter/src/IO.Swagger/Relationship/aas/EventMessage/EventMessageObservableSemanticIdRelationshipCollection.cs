namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class EventMessageObservableSemanticIdRelationshipCollection : RelationshipCollection<EventMessageObservableSemanticIdRelationship, Reference>
    {
        public EventMessageObservableSemanticIdRelationshipCollection(IEnumerable<EventMessageObservableSemanticIdRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<EventMessageObservableSemanticIdRelationship>())
        {
        }
    }
}