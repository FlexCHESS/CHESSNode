namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class SubmodelSupplementalSemanticIdRelationshipCollection : RelationshipCollection<SubmodelSupplementalSemanticIdRelationship, Reference>
    {
        public SubmodelSupplementalSemanticIdRelationshipCollection(IEnumerable<SubmodelSupplementalSemanticIdRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<SubmodelSupplementalSemanticIdRelationship>())
        {
        }
    }
}