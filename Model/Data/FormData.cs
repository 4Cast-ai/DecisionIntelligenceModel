using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class FormData
    {
        public FormDetails form_details { get; set; }
        public ActivityDetails activity_details { get; set; }
        public List<FormGroupData> form_items { get; set; }
        public FormData()
        {
            this.form_details = new FormDetails();
            this.activity_details = new ActivityDetails();
            this.form_items = new List<FormGroupData>();
        }

        public FormData(FormDetails form_details, ActivityDetails activity_details, List<FormGroupData> form_items)
        {
            this.form_details = form_details;
            this.activity_details = activity_details;
            this.form_items = form_items;
        }
    }
}
