using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class DynamicScore
    {
        public int DynamicScoresID { get; set; }
        public string FormGuid { get; set; } = null!;
        public string ModelComponentGuid { get; set; } = null!;
        public string? Comment { get; set; }
        public double? Score { get; set; }

        public virtual DynamicForm FormGu { get; set; } = null!;
    }
}
