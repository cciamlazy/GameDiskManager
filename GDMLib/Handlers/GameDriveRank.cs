using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Handlers
{
    public class GameDriveRank
    {
        public GameDriveRank()
        {

        }
    }

    public class RankableGame
    {
        private readonly int PriorityWeight = 80;
        private readonly int LastPlayedWeight = 40;
        private readonly int PlayTimeWeight = 40;

        public int GameID { get; set; }
        public int DriveID { get; set; }
        public long Size { get; set; }
        public int Priority { get; set; }
        public DateTime LastPlayed { get; set; }
        public int PlayTime { get; set; }
        public int PlayTime2Weeks { get; set; }
        public bool Rankable { get; set; }

        public int PriorityRank
        {
            get
            {
                return Priority * PriorityWeight;
            }
        }

        public int LastPlayedRank
        {
            get
            {
                return LastPlayed.To * LastPlayedWeight;
            }
        }
        public int PlayPriority
        {
            get
            {
                return Priority * PriorityWeight;
            }
        }

        public RankableGame(Game g)
        {
            this.GameID = g.GameID;
            this.DriveID = g.DriveID;
            this.Size = g.Size;
            this.Priority = g.Priority;
            this.LastPlayed = g.LastPlayed;
            this.PlayTime = g.PlayTime;
            this.PlayTime2Weeks = g.PlayTime2Weeks;
            this.Rankable = g.Rankable;
        }
    }
}
