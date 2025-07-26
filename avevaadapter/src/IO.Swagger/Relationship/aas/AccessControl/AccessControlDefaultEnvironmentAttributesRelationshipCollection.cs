namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class AccessControlDefaultEnvironmentAttributesRelationshipCollection : RelationshipCollection<AccessControlDefaultEnvironmentAttributesRelationship, Submodel>
    {
        public AccessControlDefaultEnvironmentAttributesRelationshipCollection(IEnumerable<AccessControlDefaultEnvironmentAttributesRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<AccessControlDefaultEnvironmentAttributesRelationship>())
        {
        }
    }
}