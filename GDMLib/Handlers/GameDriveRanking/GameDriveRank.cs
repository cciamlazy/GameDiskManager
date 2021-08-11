using GDMLib.Handlers.GameDriveRanking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Handlers
{
    public class GameDriveRank
    {
        private List<RankableDrive> Drives;
        private List<RankableGame> Games;

        public GameDriveRank(List<Drive> drives, List<Game> games)
        {
            Drives = new List<RankableDrive>();
            foreach (Drive d in drives)
            {
                if (d.AcceptRankedGames && d.Active)
                {
                    Drives.Add(new RankableDrive(d));
                }
            }

            Games = new List<RankableGame>();
            foreach (Game g in games)
            {
                if (g.Size > 0 && g.Rankable)
                    Games.Add(new RankableGame(g));
            }
        }

        public void Solve()
        {
            float num = 0f;
            if (!CheckIfSolutionCanExist())
            {
                return;
            }
        }

        private bool CheckIfSolutionCanExist()
        {
            NonIncreasingSortOnDriveRank comparer = new NonIncreasingSortOnDriveRank();
            Drives.Sort(comparer);
            NonIncreasingSortOnGameRank comparer2 = new NonIncreasingSortOnGameRank();
            Games.Sort(comparer2);
            string text = string.Empty;
            if (Drives[0].MinimumReject > 0f)
            {
                text = "the minimum reject required is " + Drives[0].MinimumReject;
            }
            if (Games[0].Size > Drives[0].SpaceAvailableToWorkWith - Drives[0].MinimumReject)
            {
                Console.WriteLine("Stocks too small.\n\n The biggest Stock is " + Drives[0].SpaceAvailableToWorkWith + ", " + text + ", the biggest Item is " + Games[0].Size + ".\n\nA solution can't exist.", "Bin Packing - Cutting Stock");
                return false;
            }
            return true;
        }
    }

    public class RankableDrive
    {
        public int DriveID { get; set; }
        public long TotalSize { get; set; }
        public long TotalFreeSpace { get; set; }
        public long AvailableFreeSpace { get; set; }
        public long KeepSpaceAvailable { get; set; }
        public int Priority { get; set; }
        public long NonMoveableUsedSpace { get; set; }
        public long SpaceAvailableToWorkWith { get; set; }

        // For Ranking
        public float MinimumReject;

        public RankableDrive(Drive d)
        {
            this.DriveID = d.DriveID;
            this.TotalSize = d.TotalSize;
            this.TotalFreeSpace = d.TotalFreeSpace;
            this.AvailableFreeSpace = d.AvailableFreeSpace;
            this.KeepSpaceAvailable = d.KeepSpaceAvailable;
            this.Priority = d.Priority;
            this.NonMoveableUsedSpace = d.NonMoveableUsedSpace;

            this.SpaceAvailableToWorkWith = this.TotalSize - this.NonMoveableUsedSpace - this.KeepSpaceAvailable;
        }
    }

    public class RankableGame
    {
        private readonly double PriorityWeight = 80;
        private readonly double LastPlayedWeight = 40;
        private readonly double PlayTimeWeight = 40;

        public int GameID { get; set; }
        public int DriveID { get; set; }
        public long Size { get; set; }
        public int Priority { get; set; }
        public DateTime LastPlayed { get; set; }
        public int PlayTime { get; set; }
        public int PlayTime2Weeks { get; set; }
        public bool Rankable { get; set; }

        public double PriorityRank
        {
            get
            {
                return Priority * PriorityWeight;
            }
        }

        public double LastPlayedRank
        {
            get
            {
                return (DateTime.Now - LastPlayed).TotalDays;
            }
        }
        public double PlayPriority
        {
            get
            {
                return PlayTime * PlayTimeWeight;
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
