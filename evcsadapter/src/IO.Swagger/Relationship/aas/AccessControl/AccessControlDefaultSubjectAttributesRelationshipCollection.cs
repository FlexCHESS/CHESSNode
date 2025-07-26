namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class AccessControlDefaultSubjectAttributesRelationshipCollection : RelationshipCollection<AccessControlDefaultSubjectAttributesRelationship, Submodel>
    {
        public AccessControlDefaultSubjectAttributesRelationshipCollection(IEnumerable<AccessControlDefaultSubjectAttributesRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<AccessControlDefaultSubjectAttributesRelationship>())
        {
        }
    }
}