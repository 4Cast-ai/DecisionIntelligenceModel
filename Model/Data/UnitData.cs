using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public class UnitData
    {
        public int SerialNum { get; set; }
        public string UnitGuid { get; set; }
        public string UnitName { get; set; }
        public int Order { get; set; }
        public string ParentUnitGuid { get; set; }
        public string ParentUnitName { get; set; }
        public string ManagerUnitPersonGuid { get; set; }
        public string ManagerUnitPersonName { get; set; }
        public string DefaultModelGuid { get; set; }
        public string DefaultModelName { get; set; }
        public int[] DescriptionsGuids { get; set; }
        public DescriptionsData[] DescriptionsData { get; set; }
        public string[] ActivityTemplatesGuids { get; set; }
        public ActivityTemplateDataInfo[] ActivityTemplatesData { get; set; }
        public string[] ModelsGuids { get; set; }
        public ModelData[] ModelsData { get; set; }
    }
}
