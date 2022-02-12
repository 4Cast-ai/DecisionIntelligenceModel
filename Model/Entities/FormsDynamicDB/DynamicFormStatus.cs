using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class DynamicFormStatus
    {
        public DynamicFormStatus()
        {
            DynamicForms = new HashSet<DynamicForm>();
        }

        public int FormStatusCode { get; set; }
        public string? FormStatusName { get; set; }

        public virtual ICollection<DynamicForm> DynamicForms { get; set; }
    }
}
