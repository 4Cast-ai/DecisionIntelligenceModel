using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
  public partial class UserPreference
    {
     
        public string UserGuid { get; set; }
        public int UserTheme { get; set; }
        public int UserLayOut { get; set; }

        public virtual User User { get; set; }

    }
}
