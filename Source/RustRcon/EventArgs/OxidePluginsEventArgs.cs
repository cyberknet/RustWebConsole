using RustRcon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.EventArgs;

public class OxidePluginsEventArgs : ListEventArgs<OxidePlugin>
{
    public OxidePluginsEventArgs(List<OxidePlugin> oxidePlugins) : base(oxidePlugins)
    {
    }
}
