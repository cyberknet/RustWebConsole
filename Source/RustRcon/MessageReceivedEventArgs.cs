using RustRcon.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon
{
    public class MessageReceivedEventArgs
    {
        public WebRconResponse Response { get; set; }
        public bool Handled { get; set; } = false;
        public MessageReceivedEventArgs(WebRconResponse response, bool handled)
        {
            Response = response;
            Handled = handled;
        }
    }
}
