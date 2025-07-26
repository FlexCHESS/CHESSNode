namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class HasDataSpecificationDataSpecificationRelationshipCollection : RelationshipCollection<HasDataSpecificationDataSpecificationRelationship, Reference>
    {
        public HasDataSpecificationDataSpecificationRelationshipCollection(IEnumerable<HasDataSpecificationDataSpecificationRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<HasDataSpecificationDataSpecificationRelationship>())
        {
        }
    }
}