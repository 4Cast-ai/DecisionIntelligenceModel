using System;

namespace Model.Data
{
    [Serializable]
    public class OrgModelPolygonData
    {
        public string OrgObjGuid { get; set; }
        public string ModelComponentGuid { get; set; }
        public string PolygonGuid { get; set; }
    }
}
