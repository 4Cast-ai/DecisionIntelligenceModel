using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class OrgModelPolygon
    {
        public string UnitGuid { get; set; }
        public string ModelComponentGuid { get; set; }
        public string PolygonGuid { get; set; }

        public virtual ModelComponent ModelComponentGu { get; set; }
        public virtual Unit UnitGu { get; set; }

    }
}
