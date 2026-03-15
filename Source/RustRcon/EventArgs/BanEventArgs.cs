using RustRcon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.EventArgs;

public class BanEventArgs : ListEventArgs<Ban>
{
    public BanEventArgs(List<Ban> bans) : base(bans)
    {
    }

}
