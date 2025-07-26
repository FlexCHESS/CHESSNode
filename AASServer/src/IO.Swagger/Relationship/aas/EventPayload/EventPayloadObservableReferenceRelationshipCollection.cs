namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class EventPayloadObservableReferenceRelationshipCollection : RelationshipCollection<EventPayloadObservableReferenceRelationship, Referable>
    {
        public EventPayloadObservableReferenceRelationshipCollection(IEnumerable<EventPayloadObservableReferenceRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<EventPayloadObservableReferenceRelationship>())
        {
        }
    }
}