using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class Unit
    {
        public Unit()
        {
            InverseUnitParentGu = new HashSet<Unit>();
            PersonUnit = new HashSet<Person>();
            OrganizationUnion1 = new HashSet<OrganizationUnion>();
            OrganizationUnion2 = new HashSet<OrganizationUnion>();
            User = new HashSet<User>();
            Roles = new HashSet<Roles>();
            OrgModelPolygon = new HashSet<OrgModelPolygon>();
        }

        //public string UnitGuid { get; set; }
        //public double UnitId { get; set; }
        //public string UnitName { get; set; }
        //public string UnitDescription { get; set; }
        //public string UnitTypeGuid { get; set; }
        //public string UnitStatus { get; set; }
        //public string UnitParentGuid { get; set; }
        //public string UnitCreateDate { get; set; }
        //public string UnitModifiedDate { get; set; }
        //public string UnitCatalogId { get; set; }
        //public string PolygonGuid { get; set; }
        //public bool? IsEstimateUnit { get; set; }
        public int SerialNum { get; set; }
        public string UnitGuid { get; set; }
        public string UnitName { get; set; }
        public int Order { get; set; }
        public string ParentUnitGuid { get; set; }
        public string ManagerUnitGuid { get; set; }
        public string DefaultModelGuid { get; set; }
       

        public virtual Unit UnitParentGu { get; set; }
        public virtual ModelComponent DefaultModelGu { get; set; }
        public virtual Person ManagerUnitGu { get; set; }


        //public virtual Unit ParentUnitGu { get; set; }

        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<Unit> InverseUnitParentGu { get; set; }
        public virtual ICollection<Person> PersonUnit { get; set; }
        public virtual ICollection<OrganizationUnion> OrganizationUnion1 { get; set; }
        public virtual ICollection<OrganizationUnion> OrganizationUnion2 { get; set; }
        public virtual ICollection<Roles> Roles { get; set; }
        public virtual ICollection<OrgModelPolygon> OrgModelPolygon { get; set; }
    }
}
