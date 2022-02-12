using System;

namespace Model.Data
{
    [Serializable]
    public class UnitDataInfo
    {
        public string guid { get; set; }
        public string name { get; set; }
        public string remark { get; set; }
        public int? org_type { get; set; }
        public int? order { get; set; }
        public string parent_guid { get; set; }
        public string union_guid { get; set; }
    }
}
