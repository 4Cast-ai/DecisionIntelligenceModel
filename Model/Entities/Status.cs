using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public partial class Status
    {
        public Status()
        {
            PersonStatus = new HashSet<Person>();
        }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public virtual ICollection<Person> PersonStatus { get; set; }
    }
}
