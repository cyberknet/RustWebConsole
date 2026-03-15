using RustRcon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.Containers
{
    public class ConsoleMessageList : EntityBase
    {
        public List<ConsoleMessage> Consoles { get; set; } = new List<ConsoleMessage>();
    }
}
