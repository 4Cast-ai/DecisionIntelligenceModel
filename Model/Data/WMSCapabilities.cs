using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public class WMSCapabilities
    {
        public string Version;
        public Exception Error;
        public string WMSGeoserverURL;
        public List<CustomImageInfo> Layers = new List<CustomImageInfo>();
    }
}
