using RustRcon.Containers;
using RustRcon.Entities;
using RustRcon.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RustRcon.Parsers
{
    public class ConsoleParser : ParserBase
    {
        public ConsoleParser(Action<EntityBase> eventCallback) : base(eventCallback) { }

        public override bool TryParseMessage(WebRconResponse response, out EntityBase entity)
        {
            var test = JsonSerializer.Deserialize<List<ConsoleMessage>>(response.Message, _jsonOptions);
            if (test != null)
                entity = new ConsoleMessageList() { Consoles = test };
            else
                entity = new ServerInfo();

            return entity != null;
        }
    }
}
