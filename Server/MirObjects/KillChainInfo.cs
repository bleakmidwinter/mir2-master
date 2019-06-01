using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.MirDatabase;
using Server.MirEnvir;

namespace Server.MirObjects
{
    public class KillChainInfo
    {
        public ushort Kills = 0;
        public ushort KillsRequired;
        public int Duration;
        public int TimePassed;
        public long StartTime;
        public MonsterInfo mobInfo;
        public string Message;

        public KillChainInfo(MonsterInfo mInfo, ushort KillsNeeded, long startTime)
        {
            mobInfo = mInfo;

            KillsRequired = KillsNeeded;
            StartTime = startTime;
            Duration = KillsRequired * Settings.KillChainDurationPerMob; //duration in seconds
        }

        internal bool CheckChainKill(MonsterInfo info, ref bool isKillChainMob)
        {
            if (info.Index == mobInfo.Index)
            {
                Kills++;
                isKillChainMob = true;
            }
            else
            {
                isKillChainMob = false;
            }

            //return Kills >= KillsRequired ? true : false;
            return Kills >= KillsRequired;
        }
    }
}
