using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormsEvaluatedType
    {
        public int EvaluatedTypeCode { get; set; }
        public string EvaluatedTypeName { get; set; } = null!;
    }
}
