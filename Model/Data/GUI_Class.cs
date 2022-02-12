using Model.Data;
using System.Collections.Generic;

namespace Model.Data
{
    public class ModelDataUnitTypeField
    {
        public string Unit_Type_Guid
        {
            get;
            set;
        }
    }

    public class MetricRefData
    {
        private string _metric_name;
        private List<string> _Models_for_metric = new List<string>();

        public string metric_name { get { return _metric_name; } set { _metric_name = value; } }
        public List<string> Models_for_metric { get { return _Models_for_metric; } set { _Models_for_metric = value; } }
    }

    public class MyForm_templates
    {

        public string form_template_guid { get; set; }
        public double? form_template_comment_obligation_level { get; set; }
        public string form_template_name { get; set; }
        public string form_template_description { get; set; }
        public string form_template_modified_date { get; set; }
        public string form_template_create_date { get; set; }
        public string form_template_creator_note { get; set; }

        public string form_template_status { get; set; }
        public string form_template_creator_user_guid { get; set; }
        public int form_template_type { get; set; }
    }

    public class FormTemplatePermissionData
    {
        public string JobGuid { get; set; }
        public string JobDesc { get; set; }
        public string form_template_guid { get; set; }
        public int permission_type { get; set; }
        public string permission_desc { get; set; }

        public string unit_guid { get; set; }
        public string unit_name { get; set; }

        public string unit_type_guid { get; set; }
        public string unit_type_name { get; set; }


    }

    public class FormTemplateActivityTemplates
    {
        public string activity_template_guid { get; set; }
        public string activity_template_name { get; set; }
    }
}