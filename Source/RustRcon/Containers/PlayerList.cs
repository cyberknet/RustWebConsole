using RustRcon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.Containers
{
    public class PlayerList : EntityBase
    {
        public PlayerList() { }
        public PlayerList(List<Player> players)
        {
            if (players != null) Players = players;
            IsValid = players != null;
        }
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
