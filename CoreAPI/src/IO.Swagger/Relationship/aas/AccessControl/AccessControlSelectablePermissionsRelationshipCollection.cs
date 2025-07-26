namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class AccessControlSelectablePermissionsRelationshipCollection : RelationshipCollection<AccessControlSelectablePermissionsRelationship, Submodel>
    {
        public AccessControlSelectablePermissionsRelationshipCollection(IEnumerable<AccessControlSelectablePermissionsRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<AccessControlSelectablePermissionsRelationship>())
        {
        }
    }
}