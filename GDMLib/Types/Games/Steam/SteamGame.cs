using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Games
{
    public class SteamGame : Game
    {
        public string AppID { get; set; }
        public string Manifest { get
            {
                if (this.ConfigFiles == null || this.ConfigFiles.Count == 0)
                    return "";
                return this.ConfigFiles.Find(x => x.Identifier == ConfigIdentifier.Manifest).Location;
            } set
            {
                if (this.ConfigFiles == null || this.ConfigFiles.Count == 0)
                {
                    this.ConfigFiles = new List<ConfigFile>();
                    this.ConfigFiles.Add(new ConfigFile
                    {
                        Identifier = ConfigIdentifier.Manifest,
                        KeepLocation = false,
                        KeepRelative = true,
                        Location = value,
                        RelativeLocation = "..\\" + Utils.GetRelativePath(this.Location, value)
                    });
                }
                else
                {
                    this.ConfigFiles.Find(x => x.Identifier == ConfigIdentifier.Manifest).Location = value;
                }
            }
        }
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

        public override GameMigration GenerateMigration(string dest, DateTime plannedDT)
        {


            return base.GenerateMigration(dest, plannedDT);
        }
    }
}
