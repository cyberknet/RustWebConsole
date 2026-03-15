using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.Messages
{
    public class WebRconRequest : WebRconMessageBase
    {
        public string Name { get; set; } = "RustyCon";
    }
}
