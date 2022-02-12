using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Data
{
    [Serializable]
    public class CalculateTreeData : ICloneable
    {
        public string candidateID { get; set; }
        public string polygonGuid { get; set; }

        public string report_guid { get; set; }
        public CalculateNodeData data { get; set; }
        public List<CalculateTreeData> children { get; set; }

        public CalculateTreeData()
        {
            this.data = new CalculateNodeData();
            this.children = new List<CalculateTreeData>();
        }

        public CalculateTreeData(ModelComponent mc, ModelStructure ms)
        {
            this.data = new CalculateNodeData(mc, ms);
            this.children = new List<CalculateTreeData>();
        }

        public CalculateTreeData(ModelComponent mc, ModelStructure ms, List<ThresholdData> allModelThresholds, string[] allorigins)
            : this(mc, ms)
        {
            this.data.model_data.origin_threshold_list = allModelThresholds.Where(t => allorigins.Contains(t.ModelComponentOriginGuid)).ToList();
            this.data.model_data.threshold_list = allModelThresholds.Where(t => allorigins.Contains(t.ModelComponentDestinationGuid)).ToList();
        }

        public CalculateTreeData(CalculateMCData modelData) : this()
        {
            this.data.model_data = modelData;
        }

        public CalculateTreeData(CalculateTreeData calculateTreeData)
        {
            this.candidateID = calculateTreeData.candidateID;
            this.polygonGuid = calculateTreeData.polygonGuid;
            this.report_guid = calculateTreeData.report_guid;
            this.data = calculateTreeData.data;
            this.children = calculateTreeData.children;
        }

        public IEnumerable<CalculateTreeData> GetNodeAndDescendants()
        {
            return new[] { this }
                   .Concat(children.SelectMany(child => child.GetNodeAndDescendants()));
        }

        public Object Clone()
        {
            return new CalculateTreeData
            {
                candidateID = this.candidateID,
                polygonGuid = this.polygonGuid,
                report_guid = this.report_guid,
                data = new CalculateNodeData(this.data),
                children = this.children// new List<CalculateTreeData>()
            };
        }
    }
}
