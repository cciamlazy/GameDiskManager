using GameDiskManager.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async override Task<GameMigration> Migrate(string dest, DateTime plannedDT)
        {
            GameMigration gm = await base.Migrate(dest, plannedDT);

            Launcher steam = Data.LauncherByID(this.LauncherID);

            string oldDir = steam.GameDirectories.Where(x => x.Contains(Data.DriveByID(gm.From_DriveID).Name)).First();

            string newDir = steam.GameDirectories.Where(x => x.Contains(Data.DriveByID(gm.To_DriveID).Name)).First();

            string newDest = Manifest.Replace(oldDir, newDir);

            Console.WriteLine("Moving {0} to {1}", Manifest, newDest);
            FastMove.FMove(Manifest, newDest);
            Manifest = newDest;

            Data.SaveDataStore();

            return gm;
        }
    }
}
