using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class Description
    {
        public Description()
        {
            ModelDescription = new HashSet<ModelDescription>();
            //OrganizationObjectConnection = new HashSet<OrganizationObjectConnection>();
            EntityDescription = new HashSet<EntityDescription>();
            ActivityTemplateDescription = new HashSet<ActivityTemplateDescription>();
        }

        public int DescriptionGuid { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public DateTime? Creator { get; set; }
        public DateTime? Modify { get; set; }
        public int? TypeGuid { get; set; }
        public string CreatorUserGuid { get; set; }
        public string ModifyUserGuid { get; set; }

        public virtual ICollection<ModelDescription> ModelDescription { get; set; }
        //public virtual ICollection<OrganizationObjectConnection> OrganizationObjectConnection { get; set; }
        public virtual ICollection<EntityDescription> EntityDescription { get; set; }
        public virtual ICollection<ActivityTemplateDescription> ActivityTemplateDescription { get; set; }
    }
}
