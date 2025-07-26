namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class DataSpecificationIEC61360UnitIdRelationshipCollection : RelationshipCollection<DataSpecificationIEC61360UnitIdRelationship, Reference>
    {
        public DataSpecificationIEC61360UnitIdRelationshipCollection(IEnumerable<DataSpecificationIEC61360UnitIdRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<DataSpecificationIEC61360UnitIdRelationship>())
        {
        }
    }
}