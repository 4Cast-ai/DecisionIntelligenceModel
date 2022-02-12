using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    //[Serializable]
    
    public class Evaluator
    {
        public int EvaluatorType { get; set; }
        public string EvaluatorGuid { get; set; }
        public string EvaluatorFirstName { get; set; }
        public string EvaluatorLastName { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string EvaluatorUnitGuid { get; set; }
        public List<Evaluated> EvaluatedList { get; set; }
    }

}
