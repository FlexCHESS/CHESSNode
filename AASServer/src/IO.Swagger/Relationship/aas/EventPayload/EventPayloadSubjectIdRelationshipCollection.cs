namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class EventPayloadSubjectIdRelationshipCollection : RelationshipCollection<EventPayloadSubjectIdRelationship, Reference>
    {
        public EventPayloadSubjectIdRelationshipCollection(IEnumerable<EventPayloadSubjectIdRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<EventPayloadSubjectIdRelationship>())
        {
        }
    }
}