using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormElement
    {
        public FormElement()
        {
            CalculateScore = new HashSet<CalculateScore>();
            FormElementConnection = new HashSet<FormElementConnection>();
            FormTemplateStructure = new HashSet<FormTemplateStructure>();
            Score = new HashSet<Score>();
        }

        public string FormElementGuid { get; set; }
        public int? FormElementType { get; set; }
        public string FormElementTitle { get; set; }

        public virtual FormElementType FormElementTypeNavigation { get; set; }
        public virtual ICollection<CalculateScore> CalculateScore { get; set; }
        public virtual ICollection<FormElementConnection> FormElementConnection { get; set; }
        public virtual ICollection<FormTemplateStructure> FormTemplateStructure { get; set; }
        public virtual ICollection<Score> Score { get; set; }
    }
}
