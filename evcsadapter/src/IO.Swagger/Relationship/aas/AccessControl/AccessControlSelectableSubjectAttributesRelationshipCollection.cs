namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class AccessControlSelectableSubjectAttributesRelationshipCollection : RelationshipCollection<AccessControlSelectableSubjectAttributesRelationship, Submodel>
    {
        public AccessControlSelectableSubjectAttributesRelationshipCollection(IEnumerable<AccessControlSelectableSubjectAttributesRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<AccessControlSelectableSubjectAttributesRelationship>())
        {
        }
    }
}