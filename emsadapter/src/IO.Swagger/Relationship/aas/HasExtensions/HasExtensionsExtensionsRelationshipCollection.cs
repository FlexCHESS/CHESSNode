namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class HasExtensionsExtensionsRelationshipCollection : RelationshipCollection<HasExtensionsExtensionsRelationship, Extension>
    {
        public HasExtensionsExtensionsRelationshipCollection(IEnumerable<HasExtensionsExtensionsRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<HasExtensionsExtensionsRelationship>())
        {
        }
    }
}