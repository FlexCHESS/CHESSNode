namespace IoT.Services
{
    using Azure;
    using Azure.DigitalTwins.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text.Json.Serialization;
    using System.Linq;

    public class AccessPermissionRuleTargetSubjectAttributesRelationshipCollection : RelationshipCollection<AccessPermissionRuleTargetSubjectAttributesRelationship, SubjectAttributes>
    {
        public AccessPermissionRuleTargetSubjectAttributesRelationshipCollection(IEnumerable<AccessPermissionRuleTargetSubjectAttributesRelationship>? relationships = default) : base(relationships ?? Enumerable.Empty<AccessPermissionRuleTargetSubjectAttributesRelationship>())
        {
        }
    }
}