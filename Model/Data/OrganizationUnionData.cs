using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class OrganizationUnionData
    {
        public int OrganizationUnionId { get; set; }
        public string OrganizationUnionGuid { get; set; }
        public string OrgObjGuid { get; set; }
        public string ParentOrgObjGuid { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
