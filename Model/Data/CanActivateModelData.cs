using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class CanActivateModelData
    {
        public List<Data> not_in_form_list { get; set; }
        public List<Data> incorrect_weight_list { get; set; }
        public List<Data> not_active_list { get; set; }

        public CanActivateModelData()
        {
            this.not_in_form_list = new List<Data>();
            this.incorrect_weight_list = new List<Data>();
            this.not_active_list = new List<Data>();
        }
    }
}
