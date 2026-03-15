using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.Messages
{
    public class WebRconResponse : WebRconMessageBase
    {
        public string Type { get; set; } = string.Empty;
        public string Stacktrace { get; set; } = string.Empty;
    }
}
