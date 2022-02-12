using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities
{
    [Serializable]
    public partial class OrganizationUnion
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("OrganizationUnionId")]
        public int OrganizationUnionId { get; set; }
        public string OrganizationUnionGuid { get; set; }
        public string OrgObjGuid { get; set; }
        public string ParentOrgObjGuid { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Unit OrgObjGu { get; set; }
        public virtual Unit ParentOrgObjGu { get; set; }
    }
}
