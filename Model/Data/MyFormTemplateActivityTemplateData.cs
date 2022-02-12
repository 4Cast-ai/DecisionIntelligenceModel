using System;

namespace Model.Data
{
    [Serializable]
    public class MyFormTemplateActivityTemplateData
    {
        public string activity_template_guid { get; set; }
        public string form_template_guid { get; set; }
        public string Form_template_in_activity_template_delete_date { get; set; }
        public string Form_template_in_activity_template_create_date { get; set; }
        public bool IsSelected { get; set; }
        public string activity_template_name { get; set; }
        public string activity_type_guid { get; set; }
        public string activity_type_name { get; set; }


    }
}
