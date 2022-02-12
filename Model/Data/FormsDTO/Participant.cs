using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    //[Serializable]
   
    public class Participant
    {
        public int EvaluatedType { get; set; }
        public string EvaluatedGuid { get; set; }
        public string EvaluatedName { get; set; }
        public List<Evaluator> EvaluatorList { get; set; }
    }
  
}
