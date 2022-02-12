//using Model.Data.FormsDTO;
using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    //[Serializable]
   
    public class FormsFormTemplate
    {
        public string FormTemplateGuid { get; set; }
        public string FormTemplateName { get; set; }
        public List<FormsFormItemData> FormsItemDataList { get; set; }

    }
}
