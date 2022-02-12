using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormTemplateStructure
    {
        public int FormTemplateStructureId { get; set; }
        public string FormTemplateGuid { get; set; }
        public string ModelComponentGuid { get; set; }
        public string FormElementGuid { get; set; }
        public int? Order { get; set; }

        public virtual FormElement FormElementGu { get; set; }
        public virtual FormTemplate FormTemplateGu { get; set; }
        public virtual ModelComponent ModelComponentGu { get; set; }
    }
}
