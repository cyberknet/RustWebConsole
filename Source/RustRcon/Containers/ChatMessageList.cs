using RustRcon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.Containers
{
    internal class ChatMessageList : EntityBase
    {
        public List<ChatMessage> Chats { get; set; } = new List<ChatMessage>();
    }
}
