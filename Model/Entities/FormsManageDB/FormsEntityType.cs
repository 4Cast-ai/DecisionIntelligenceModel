using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormsEntityType
    {
        public int EntityTypeCode { get; set; }
        public string EntityTypeName { get; set; } = null!;
    }
}
