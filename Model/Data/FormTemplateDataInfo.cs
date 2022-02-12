using Model.Entities;
using System;

namespace Model.Data
{
    [Serializable]
    public class FormTemplateDataInfo
    {
        public string form_template_guid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string modified_date { get; set; }
        public string create_date { get; set; }
        public string creator_user_guid { get; set; }

        public FormTemplateDataInfo()
        {

        }

        public FormTemplateDataInfo(FormTemplate form_template)
        {
            this.form_template_guid = form_template.FormTemplateGuid;
            this.name = form_template.Name;
            this.description = form_template.Description;
            this.modified_date = form_template.ModifiedDate;
            this.create_date = form_template.CreateDate;
            this.creator_user_guid = form_template.CreatorUserGuid;
        }

    }
}
