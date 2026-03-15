using RustRcon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.EventArgs;

public class ChatEventArgs : ListEventArgs<ChatMessage>
{
    public ChatEventArgs(List<ChatMessage> chats) : base(chats)
    {
    }
}
