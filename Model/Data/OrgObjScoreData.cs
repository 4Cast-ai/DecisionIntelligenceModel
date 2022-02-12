using System;

namespace Model.Data
{
    [Serializable]
    public class OrgObjScoreData
    {
        public string parent_org_obj_guid { get; set; }
        public string org_obj_guid { get; set; }
        public string org_obj_name { get; set; }
        public int order { get; set; }
        public double? score { get; set; }
        public int score_level { get; set; }
        public CalculateTreeData tree { get; set; }

        public OrgObjScoreData()
        {

        }

        public OrgObjScoreData(string parent_org_obj_guid, string org_obj_guid, string org_obj_name, int order, double? score, int score_level, CalculateTreeData tree)
        {
            this.parent_org_obj_guid = parent_org_obj_guid;
            this.org_obj_guid = org_obj_guid;
            this.org_obj_name = org_obj_name;
            this.order = order;
            this.score = score;
            this.score_level = score_level;
            this.tree = tree;
        }
    }
}