using System;

namespace Model.Data
{
    [Serializable]
    public class MyActivityTemplate
    {
        public string activity_template_guid { get; set; }
        public string activity_template_name { get; set; }
        public string activity_template_description { get; set; }
        public string activity_type_Professional_recommendations
        { get; set; }

        public string activity_template_status { get; set; }
        public string activity_template_create_date { get; set; }
        public string activity_template_delete_date { get; set; }
        public string activity_type_guid { get; set; }
        public string activity_type_name { get; set; }
    }
}
