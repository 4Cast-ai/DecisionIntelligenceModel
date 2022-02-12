using Model.Entities;
using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class ModelComponentData
    {
        public string model_component_guid { get; set; }
        public string name { get; set; }
        public double? score { get; set; }
        public string calculated_date { get; set; }
        public string org_obj_guid { get; set; }
        public string org_obj_name { get; set; }
        public double order { get; set; }
        public bool is_precentage { get; set; }
        public int score_level { get; set; }
        public string professional_instruction { get; set; }
        public List<ModelComponentData> comment_list { get; set; }

        public ModelComponentData()
        {

        }
    }
}
