using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormElementConnection
    {
        public string FormElementGuid { get; set; }
        public string ModelComponentGuid { get; set; }
        public int Order { get; set; }

        public virtual FormElement FormElementGu { get; set; }
        public virtual ModelComponent ModelComponentGu { get; set; }
    }
}
