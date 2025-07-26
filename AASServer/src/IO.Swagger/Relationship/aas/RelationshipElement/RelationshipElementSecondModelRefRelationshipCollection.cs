namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class RelationshipElementSecondModelRefRelationshipCollection : RelationshipCollection<RelationshipElementSecondModelRefRelationship, Referable>
    {
        public RelationshipElementSecondModelRefRelationshipCollection(IEnumerable<RelationshipElementSecondModelRefRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<RelationshipElementSecondModelRefRelationship>())
        {
        }
    }
}