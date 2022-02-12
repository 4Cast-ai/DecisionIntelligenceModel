using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public partial class EntityType
    {
        public EntityType()
        {
            Activities = new HashSet<Activity>();
            CalculateScores = new HashSet<CalculateScore>();
            Scores = new HashSet<Score>(); 
            Forms = new HashSet<Form>();
            ActivityEntity = new HashSet<ActivityEntity>();
            ActivityEstimator = new HashSet<ActivityEstimator>();
            ActivityTemplate = new HashSet<ActivityTemplate>();
        }
        public int EntityTypeId { get; set; }
        public string EntityTypeName { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
        public virtual ICollection<CalculateScore> CalculateScores { get; set; }
        public virtual ICollection<Score> Scores { get; set; }
        public virtual ICollection<Form> Forms { get; set; }
        public virtual ICollection<ActivityEntity> ActivityEntity { get; set; }
        public virtual ICollection<ActivityEstimator> ActivityEstimator { get; set; }
        public virtual ICollection<ActivityTemplate> ActivityTemplate { get; set; }



    }
}
