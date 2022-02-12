using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public class FormsFormData
    {
        public string FormGuid { get; set; } 
        public int FormStatus { get; set; }
        public string ActivityGuid { get; set; }
        public string ActivityName { get; set; }
        public string FormTemplateGuid { get; set; }
        public string EvaluatorGuid { get; set; }
        public string EvaluatorName { get; set; }
        public int EvaluatorType { get; set; }
        public string EvaluatedGuid { get; set; }
        public string EvaluatedName { get; set; }
        public int Evaluatedtype { get; set; }
        public string CreationDate { get; set; } 
        public string UpdateDate { get; set; } 
        public string LastUpdateGuid { get; set; } 
    }
}
