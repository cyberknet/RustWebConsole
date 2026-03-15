using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon
{
    public class ConnectionChangedEventArgs : System.EventArgs
    {
        public bool IsConnected { get; set; }

        public ConnectionChangedEventArgs(bool isConnected)
        {
            IsConnected = isConnected;
        }
    }
}
