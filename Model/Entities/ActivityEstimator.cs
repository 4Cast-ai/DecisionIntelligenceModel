using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public partial class ActivityEstimator
    {
        public int ActivityEntity { get; set; }
        public string EstimatedGuid { get; set; }
        public int EstimatedType { get; set; }

        public virtual ActivityEntity ActivityEntityId { get; set; }
        public virtual EntityType EstimatedTypeId { get; set; }

    }
}
