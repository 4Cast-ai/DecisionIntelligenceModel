using System;

namespace Model.Data
{
    [Serializable]
    public class DescriptionsData
    {
        public int description_guid { get; set; }
        public string name { get; set; }
        public string remark { get; set; }
        public DateTime? creator { get; set; }
        public DateTime? modify { get; set; }
        public string creator_user_guid { get; set; }
        public string modify_user_guid { get; set; }
    }
}
