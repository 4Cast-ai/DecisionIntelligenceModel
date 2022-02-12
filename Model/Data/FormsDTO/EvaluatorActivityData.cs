using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    //[Serializable]

    public class EvaluatorActivityData
    {
        public string ActivityGuid { get; set; }
        public string ActivityName { get; set; }
        public string EndDate { get; set; }
        public List<EvaluatorFormTemplate> EvaluatorFormTemplateList { get; set; }
    }

}
