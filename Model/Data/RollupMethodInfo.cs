using Model.Entities;
using System;

namespace Model.Data
{
    [Serializable]
    public class RollupMethodInfo
    {
        public int rollup_method_id { get; set; }
        public string rollup_method_name { get; set; }

        public RollupMethodInfo()
        {

        }

        public RollupMethodInfo(RollupMethod rollup_method)
        {
            this.rollup_method_id = rollup_method.RollupMethodId;
            this.rollup_method_name = rollup_method.RollupMethodName;
        }
    }
}
