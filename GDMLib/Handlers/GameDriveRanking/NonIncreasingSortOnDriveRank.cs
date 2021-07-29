using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Handlers.GameDriveRanking
{
    public class NonIncreasingSortOnDriveRank : IComparer<RankableDrive>
    {
				public int Compare(RankableDrive x, RankableDrive y)
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
								if (x.SpaceAvailableToWorkWith > y.SpaceAvailableToWorkWith)
								{
										return 1;
								}
								if (x.SpaceAvailableToWorkWith < y.SpaceAvailableToWorkWith)
								{
										return -1;
								}
						}
						return 0;
				}
		}
}
