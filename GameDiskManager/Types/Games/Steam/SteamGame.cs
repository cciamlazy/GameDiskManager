using Newtonsoft.Json;
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

        [JsonConstructor]
        public SteamGame(int launcherId, int gameId, int driveId, string name, string location) : base (launcherId, gameId, driveId, name, location)
        {

        }

        public override void Migrate(string dest)
        {
            base.Migrate(dest);
        }
    }
}
