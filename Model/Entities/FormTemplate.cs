using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormTemplate
    {
        public FormTemplate()
        {
            AtInFt = new HashSet<AtInFt>();
            Form = new HashSet<Form>();
            FormTemplateStructure = new HashSet<FormTemplateStructure>();
        }

        public string FormTemplateGuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ModifiedDate { get; set; }
        public string CreateDate { get; set; }
        public string CreatorUserGuid { get; set; }

        public virtual User CreatorUserGu { get; set; }
        public virtual ICollection<AtInFt> AtInFt { get; set; }
        public virtual ICollection<Form> Form { get; set; }
        public virtual ICollection<FormTemplateStructure> FormTemplateStructure { get; set; }
    }
}
