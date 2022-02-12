using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public class CustomImageInfo
    {
        public bool isSelected;
        private string MapName_;
        public string MapName
        {
            get { return MapName_; }
            set { MapName_ = value; }
        }
        public int TypeId;
        public string Guid;
        public double MinX;
        public double MaxX;
        public double MinY;
        public double MaxY;

        public int MinZoom;
        public int MaxZoom;

        public bool isMapLayer;

        public bool isRasterLogin;
    }
}
