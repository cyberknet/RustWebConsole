using RustRcon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.EventArgs;

public class ServerInfoEventArgs : ResultEventArgs<ServerInfo>
{
    public ServerInfoEventArgs(ServerInfo serverInfo) : base(serverInfo)
    {
    }
}
