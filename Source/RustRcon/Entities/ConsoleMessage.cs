using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.Entities
{
    public class ConsoleMessage : MessageLogBase
    {
        public string StackTrace { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public ConsoleMessage() { }
        public ConsoleMessage(string? message, string? stackTrace, string? type, int? time)
        {
            if (message != null) Message = message;
            if (stackTrace != null) StackTrace = stackTrace;
            if (type != null) Type = type;
            if (time != null) Time = time.Value;
        }
    }
}
