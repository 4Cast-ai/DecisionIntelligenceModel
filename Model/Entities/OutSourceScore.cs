using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class OutSourceScore
    {
        public string UserGuid { get; set; }
        public string ModelComponentGuid { get; set; }
        public DateTime EventDate { get; set; }
        public string Score { get; set; }
        public int FormType { get; set; }

        public string AverageScore { get; set; }
        public int? EvaluatingCount { get; set; }
        public string CandidateUnit { get; set; }
        public string CandidateRole { get; set; }
        public string CandidateRank { get; set; }
        public string TextAnswerQuestion { get; set; }
        public string TextAnswerSummary { get; set; }

        public virtual ModelComponent ModelComponentGu { get; set; }
    }
}
