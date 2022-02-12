using System;

namespace Model.Data
{
    [Serializable]
    public class UnitBindingDetails
    {
        public string Unit_Binding_Guid { get; set; }
        public string Unit_Binding_Name { get; set; }
        public string Unit_Binding_Father_Guid { get; set; }
        public string Unit_Binding_Status { get; set; }
        public string Unit_Binding_Create_Date { get; set; }
        public string Unit_Guid { get; set; }
        public string Unit_Name { get; set; }
        public string Unit_Type_Guid { get; set; }
        public string Unit_Type_Name { get; set; }
        public string Unit_Polygon_Guid { get; set; }
        public string Polygon_Name { get; set; }
        public string Visible_Unit_Guid { get; set; }
        public string Unit_Create_Date { get; set; }
        public int organization_type { get; set; }
        public int unit_or_echelon { get; set; }
        public Nullable<int> force_type { get; set; }
        public int rate_type { get; set; }
        public Nullable<int> unit_type_definition { get; set; }
        public bool? HasChildren { get; set; }
        private int _inlevel_order;
        public int In_Level_Order
        {
            get
            {
                return this._inlevel_order;
            }
            set
            {
                if (this._inlevel_order != value)
                {
                    this._inlevel_order = value;
                    //  this.RaisePropertyChanged("In_Level_Order");
                }
            }
        }


    }
}
