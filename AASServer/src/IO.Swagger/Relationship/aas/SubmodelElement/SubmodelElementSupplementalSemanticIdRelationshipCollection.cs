namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class SubmodelElementSupplementalSemanticIdRelationshipCollection : RelationshipCollection<SubmodelElementSupplementalSemanticIdRelationship, Reference>
    {
        public SubmodelElementSupplementalSemanticIdRelationshipCollection(IEnumerable<SubmodelElementSupplementalSemanticIdRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<SubmodelElementSupplementalSemanticIdRelationship>())
        {
        }
    }
}