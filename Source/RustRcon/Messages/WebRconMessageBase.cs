using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RustRcon.Messages
{
    public abstract class WebRconMessageBase
    {
        [JsonIgnore]
        public Guid id { get; set; } = Guid.NewGuid();
        [JsonIgnore]
        public string MessageType { get { return this is WebRconRequest ? "Request" : "Response"; } }
        public int Identifier { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
