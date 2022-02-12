using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Data
{
    [Serializable]
    public class FormTemplateData
    {
        public FormTemplateDataInfo form_template { get; set; }
        public List<FormItemData> form_items_list { get; set; }
        public List<string> activities { get; set; }

    }
}
