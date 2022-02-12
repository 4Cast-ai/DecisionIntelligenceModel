using System;
using System.Collections.Generic;

namespace Model.Data
{
    //[Serializable]
    //public partial class OrgObjTree : OrgObj
    //{
    //    public List<OrgObjTree> children { get; set; }
    //}

    //[Serializable]
    //public class OrgObj
    //{
    //    public string guid { get; set; }
    //    public string parent_guid { get; set; }
    //    public string name { get; set; }
    //    public string remark { get; set; }
    //    public int? order { get; set; }
    //    public int? org_type { get; set; }
    //    public Guid[] permission_units { get; set; }
    //}

    [Serializable]
    public class EntityConnection
    {
        public string entity_guid { get; set; }
        public ActivityTemplateDataInfo[] activities { get; set; }
        public DescriptionsData[] descriptions { get; set; }
        //public ModelData[] models { get; set; }
    }
}
