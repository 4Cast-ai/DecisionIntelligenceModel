using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormsActivityTrace
    {
        public decimal ActivityTraceId { get; set; }
        public string ActivityGuid { get; set; } = null!;
        public string ActivityName { get; set; } = null!;
        public string ActivityStartDate { get; set; } = null!;
        public string ActivityEndDate { get; set; } = null!;
        public bool IsLimited { get; set; }
        public bool CanSubmitOnce_ { get; set; }
        public bool IsAnonymous { get; set; }
        public string CreationDate { get; set; } = null!;
        public string UpdateDate { get; set; } = null!;
        public string FromEffectDate { get; set; } = null!;
        public string ToEffectDate { get; set; } = null!;
        public int RecordStatusCode { get; set; }
        public string UpdateUserId { get; set; } = null!;
        public string EvaluatedAndEvaluators { get; set; } = null!;
        public string Forms { get; set; } = null!;
        public int? FormsDBID { get; set; }
        public string? FormsDBName { get; set; }

        public virtual FormsRecordStatus RecordStatusCodeNavigation { get; set; } = null!;
    }
}
