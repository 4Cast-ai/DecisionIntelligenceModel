using System;

namespace Model.Data
{
    [Serializable]
    public class EntityDescriptionData
    {
        public string name { get; set; }
        public string remark { get; set; }
        public DateTime creator { get; set; }
        public DateTime modify { get; set; }
        public string symbol { get; set; }
        public int? description_guid_1 { get; set; }
        public int? description_guid_2 { get; set; }
        public int? description_guid_3 { get; set; }
        public int? description_guid_4 { get; set; }
        public int? description_guid_5 { get; set; }
        public int? description_guid_6 { get; set; }
        public int? description_guid_7 { get; set; }
        public int? description_guid_8 { get; set; }
        public int? description_guid_9 { get; set; }
        public int? description_guid_10 { get; set; }
        public string creator_user_guid { get; set; }
        public string modify_user_guid { get; set; }
    }
}
