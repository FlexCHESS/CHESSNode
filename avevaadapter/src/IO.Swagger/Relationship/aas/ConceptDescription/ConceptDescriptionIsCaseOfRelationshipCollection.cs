namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class ConceptDescriptionIsCaseOfRelationshipCollection : RelationshipCollection<ConceptDescriptionIsCaseOfRelationship, Reference>
    {
        public ConceptDescriptionIsCaseOfRelationshipCollection(IEnumerable<ConceptDescriptionIsCaseOfRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<ConceptDescriptionIsCaseOfRelationship>())
        {
        }
    }
}