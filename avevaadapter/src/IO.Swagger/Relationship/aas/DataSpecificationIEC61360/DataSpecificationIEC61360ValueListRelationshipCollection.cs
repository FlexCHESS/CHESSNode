namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class DataSpecificationIEC61360ValueListRelationshipCollection : RelationshipCollection<DataSpecificationIEC61360ValueListRelationship, ValueList>
    {
        public DataSpecificationIEC61360ValueListRelationshipCollection(IEnumerable<DataSpecificationIEC61360ValueListRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<DataSpecificationIEC61360ValueListRelationship>())
        {
        }
    }
}