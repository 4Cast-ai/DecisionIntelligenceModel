using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class DynamicForm
    {
        public DynamicForm()
        {
            DynamicScores = new HashSet<DynamicScore>();
        }

        public string FormGuid { get; set; } = null!;
        public int FormStatus { get; set; }
        public string ActivityGuid { get; set; } = null!;
        public string FormTemplateGuid { get; set; } = null!;
        public string EvaluatorGuid { get; set; } = null!;
        public int EvaluatorType { get; set; }
        public string EvaluatedGuid { get; set; } = null!;
        public int EvaluatedType { get; set; }
        public string CreationDate { get; set; } = null!;
        public string UpdateDate { get; set; } = null!;
        public string LastUpdateUserGuid { get; set; } = null!;

        public virtual DynamicEntityType EvaluatedTypeNavigation { get; set; } = null!;
        public virtual DynamicEntityType EvaluatorTypeNavigation { get; set; } = null!;
        public virtual DynamicFormStatus FormStatusNavigation { get; set; } = null!;
        public virtual ICollection<DynamicScore> DynamicScores { get; set; }
    }
}
