using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public class EvaluatorFormTemplate
    {
        public string FormTemplateGuid { get; set; }
        public string FormTemplateName { get; set; }//TODO 
        public List<FormsFormData> FormData { get; set; }
    }
}
