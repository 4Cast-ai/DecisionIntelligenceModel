using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class ActivityTemplateData
    {
        public ActivityTemplateDataInfo activity_template { get; set; }
        public List<int> connected_descriptions { get; set; }
        public List<string> connected_orgs { get; set; }
        public List<string> connected_form_templates { get; set; }

    }
}
