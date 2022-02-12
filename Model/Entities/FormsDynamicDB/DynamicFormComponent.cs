using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class DynamicFormComponent
    {
        public int FormComponentRecordId { get; set; }
        public int FormRecordId { get; set; }
        public string ModelComponentGuid { get; set; } = null!;
        public string Score { get; set; } = null!;
        public string Comment { get; set; } = null!;

        public virtual DynamicForm FormRecord { get; set; } = null!;
    }
}
