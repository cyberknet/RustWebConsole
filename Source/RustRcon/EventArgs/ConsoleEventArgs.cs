using RustRcon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.EventArgs;

public class ConsoleEventArgs : ListEventArgs<ConsoleMessage>
{
    public ConsoleEventArgs(List<Entities.ConsoleMessage> list) : base(list) { }
}
