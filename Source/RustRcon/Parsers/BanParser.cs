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
    internal class BanParser : ParserBase
    {
        public BanParser(Action<EntityBase> eventCallback) : base(eventCallback) { }

        public override bool TryParseMessage(WebRconResponse response, out EntityBase entity)
        {
            var test = JsonSerializer.Deserialize<List<Ban>>(response.Message, _jsonOptions);
            if (test != null)
                entity = new BanList() { Bans = test };
            else
                entity = new BanList();

            return test != null;
        }
    }
}
