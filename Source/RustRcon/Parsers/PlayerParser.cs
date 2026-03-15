using RustRcon.Containers;
using RustRcon.Entities;
using RustRcon.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RustRcon.Parsers
{
    public class PlayerParser : ParserBase
    {
        public PlayerParser(Action<EntityBase> eventCallback) : base(eventCallback) { }

        public override bool TryParseMessage(WebRconResponse response, out EntityBase entity)
        {
            bool parsed = false;
            var players = JsonSerializer.Deserialize<List<Player>>(response.Message, _jsonOptions);
            if (players != null)
            {
                entity = new PlayerList(players);
                parsed = true;
            }
            else
            {
                entity = new PlayerList();
            }

            return parsed;
        }
    }
}
