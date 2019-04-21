using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.MirObjects
{
    class GuildWarKill
    {
        public GuildWarKill(string _playerName, string _guildName, long _diedTime)
        {
            PlayerName = _playerName;
            GuildName = _guildName;
            DiedTime = _diedTime;
        }

        public string PlayerName { set; get; }

        public string GuildName { set; get; }

        public long DiedTime { set; get; }
    }
}
