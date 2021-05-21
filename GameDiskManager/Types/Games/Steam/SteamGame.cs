using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types.Games
{
    public class SteamGame : Game
    {
        public string appid { get; set; }
        public SteamGame(string dir) : base(dir)
        {

        }
    }
}
