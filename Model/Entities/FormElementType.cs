using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormElementType
    {
        public FormElementType()
        {
            FormElement = new HashSet<FormElement>();
        }

        public int FormElementTypeGuid { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FormElement> FormElement { get; set; }
    }
}
