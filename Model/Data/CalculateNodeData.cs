using Model.Entities;
using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class CalculateNodeData
    {
        public CalculateMCData model_data { get; set; }
        public List<ScoreData> score_data { get; set; }
        public List<OrgObjScoreData> reference_score_list { get; set; }

        public CalculateNodeData()
        {
            this.model_data = new CalculateMCData();
            this.score_data = new List<ScoreData>();
            //this.reference_score_list = new List<Org_Obj_Score>();
        }

        public CalculateNodeData(ModelComponent mc, ModelStructure ms)
        {
            this.model_data = new CalculateMCData(mc, ms);
            this.score_data = new List<ScoreData>();
            //this.reference_score_list = new List<Org_Obj_Score>();
        }

        public CalculateNodeData(CalculateNodeData cn)
        {
            this.model_data = new CalculateMCData(cn.model_data);
        }
    }
}
