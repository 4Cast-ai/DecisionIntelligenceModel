using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormStatus
    {
        public FormStatus()
        {
            Form = new HashSet<Form>();
            Score = new HashSet<Score>();
        }

        public int FormStatusId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Form> Form { get; set; }
        public virtual ICollection<Score> Score { get; set; }
    }
}
