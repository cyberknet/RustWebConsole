using RustRcon.Entities;
using RustRcon.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace RustRcon.Parsers;

public class NoParser : ParserBase
{
    public NoParser(Action<EntityBase> eventCallback) : base(eventCallback)
    {
    }

    public override bool TryParseMessage(WebRconResponse response, out EntityBase entity)
    {
        entity = new Unprocessed()
        {
            Identifier = response.Identifier,
            Id = response.id,
            Type = response.Type,
            StackTrace = response.Stacktrace,
            Message = response.Message
        };
        return true;
    }
}
