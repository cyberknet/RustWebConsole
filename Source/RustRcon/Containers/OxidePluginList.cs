using RustRcon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.Containers
{
    public class OxidePluginList : EntityBase
    {
        public List<OxidePlugin> Plugins { get; set; }
    }
}
