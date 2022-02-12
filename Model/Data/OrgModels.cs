using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class OrgModels
    {
        public string OrgObjParentGuid { get; set; }
        public string OrgObjGuid { get; set; }
        public string OrgObjName { get; set; }
        public int OrgOrder { get; set; }
        public int? OrgObjType { get; set; }
        public List<string> ModelComponentList { get; set; }
    }
}
