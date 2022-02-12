using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class ActivityStatus
    {
        public ActivityStatus()
        {
            Activity = new HashSet<Activity>();
        }

        public int ActivityStatusId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Activity> Activity { get; set; }
    }
}
