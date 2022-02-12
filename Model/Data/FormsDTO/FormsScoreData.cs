using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    //[Serializable]
    public class FormsScoreData
    {
        public string ActivityGuid { get; set; }

        public string ModelComponentGuid { get; set; }

        public string FormElementGuid { get; set; }

        public string EvaluatedGuid { get; set; }

        public string EvaluatedType { get; set; }
        public string EvaluatorGuid { get; set; }

        public string EvaluatorType { get; set; }

        public double? OriginalScore { get; set; }

        public string ModelComponentComment { get; set; }

        public string FormGuid { get; set; }
        
    }
}
