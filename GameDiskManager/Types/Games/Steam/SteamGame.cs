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
        public string AppID { get; set; }
        public string Manifest { get; set; }
        public SteamGame(string dir) : base(dir)
        {

        }

        [JsonConstructor]
        public SteamGame(int launcherId, int gameId, int driveId, string name, string location, int priority, DateTime lastplayed, int playtime, bool active, List<ConfigFile> configfiles, string appid, string manifest) 
            : base (launcherId, gameId, driveId, name, location, priority, lastplayed, playtime, active, configfiles)
        {
            AppID = appid;
            Manifest = manifest;
        }

        public override void Migrate(string dest, DateTime plannedDT)
        {
            base.Migrate(dest, plannedDT);


        }
    }
}
