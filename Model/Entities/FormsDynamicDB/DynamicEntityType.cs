using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class DynamicEntityType
    {
        public DynamicEntityType()
        {
            DynamicFormEvaluatedTypeNavigations = new HashSet<DynamicForm>();
            DynamicFormEvaluatorTypeNavigations = new HashSet<DynamicForm>();
        }

        public int EntityTypeCode { get; set; }
        public string? EntityTypeName { get; set; }

        public virtual ICollection<DynamicForm> DynamicFormEvaluatedTypeNavigations { get; set; }
        public virtual ICollection<DynamicForm> DynamicFormEvaluatorTypeNavigations { get; set; }
    }
}
