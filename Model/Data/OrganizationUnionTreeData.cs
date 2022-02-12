using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class OrganizationUnionTreeData
    {
        public OrganizationUnionData data { get; set; }
        public List<OrganizationUnionTreeData> children { get; set; }
    }
}
