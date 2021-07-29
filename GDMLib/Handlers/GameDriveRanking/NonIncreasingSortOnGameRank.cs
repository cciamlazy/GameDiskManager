using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Handlers.GameDriveRanking
{
    public class NonIncreasingSortOnGameRank : IComparer<RankableGame>
    {
				public int Compare(RankableGame x, RankableGame y)
				{
						if (x.Priority < y.Priority)
						{
								return 1;
						}
						if (x.Priority > y.Priority)
						{
								return -1;
						}
						if (x.Priority == y.Priority)
						{
								if (x.PlayTime2Weeks > y.PlayTime2Weeks)
                {
										return 1;
                }
								if (x.PlayTime2Weeks < y.PlayTime2Weeks)
                {
										return -1;
                }
								if (x.PlayTime2Weeks == y.PlayTime2Weeks)
                {
										// Likely need to implement weighted ranking at this point
										if (x.LastPlayedRank > y.LastPlayedRank)
                    {
												return 1;
                    }
										if (x.LastPlayedRank < y.LastPlayedRank)
                    {
												return -1;
                    }
										if (x.LastPlayedRank == y.LastPlayedRank)
                    {
												return 0;
                    }
                }

								if (x.Size > y.Size)
								{
										return 1;
								}
								if (x.Size < y.Size)
								{
										return -1;
								}
						}
						return 0;
				}
		}
}
