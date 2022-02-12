using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public partial class ActivityEntity
    {
        public ActivityEntity()
        {
            ActivityEstimator = new HashSet<ActivityEstimator>();
        }
        
        public int ActivityEntityId { get; set; }
        public string EntityGuid { get; set; }
        public int EntityType { get; set; }
        public string ActivityGuid { get; set; }

       
        public virtual EntityType EntityTypeId { get; set; }
        public virtual Activity ActivityGu { get; set; }

        public virtual ICollection<ActivityEstimator> ActivityEstimator { get; set; }

    }
}
