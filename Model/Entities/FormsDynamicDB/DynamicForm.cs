using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class DynamicForm
    {
        public DynamicForm()
        {
            DynamicFormComponents = new HashSet<DynamicFormComponent>();
        }

        public int FormRecordId { get; set; }
        public string ActivityGuid { get; set; } = null!;
        public string FormGuid { get; set; } = null!;
        public string EvaluatedGuid { get; set; } = null!;
        public string EvaluatorGuid { get; set; } = null!;
        public string CreationDate { get; set; } = null!;
        public string UpdateDate { get; set; } = null!;

        public virtual ICollection<DynamicFormComponent> DynamicFormComponents { get; set; }
    }
}
