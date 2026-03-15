using RustRcon.Converters;
using RustRcon.Entities;
using RustRcon.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RustRcon.Parsers
{
    public abstract class ParserBase
    {
        protected JsonSerializerOptions _jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        public abstract bool TryParseMessage(WebRconResponse response, out EntityBase entity);
        public Action<EntityBase>? FireEvent;

        public ParserBase(Action<EntityBase> eventCallback)
        {
            FireEvent = eventCallback;
            _jsonOptions.Converters.Add(new DateTimeConverter());

        }
    }
}
