using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using GDMLib.Games;
using GDMLib.Serializers.VDF;
using GDMLib.Transitory;
using GDMLib.VDF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Launchers
{
    public class Steam : Launcher
    {
        private ScanProgress scanProgress = new ScanProgress();
        private List<SteamApp> steamApps = new List<SteamApp>();
        public override void ScanGames(UpdateProgressDelegate callback)
        {
            base.ScanGames(callback);

            GatherSteamData();

            scanProgress.MaxProgress = CalculateGameCount(); //Duplication of code, refactor this

            updateProgressDelegate(scanProgress);

            foreach (string s in GameDirectories)
            {
                ScanSteamGames(s, ref scanProgress);
            }

            CompleteScan();
        }

        private void GatherSteamData()
        {
            GameDirectories = GetSteamDirectories();

            foreach (string s in GameDirectories)
            {
                GetSteamApps(s);
            }
        }

        private void GetSteamApps(string dir)
        {
            IEnumerable<LocalConfig> configs = GetUserLocalConfigs(string.Format("{0}\\userdata\\", dir));

            foreach (LocalConfig c in configs)
            {
                foreach (SteamApp app in c.SteamApps)
                {
                    SteamApp existing = GetSteamApp(app.AppId);

                    if (Exists(existing))
                    {
                        int lastPlayed = Utils.GetMax(existing.LastPlayed, app.LastPlayed);
                        int playTime = Utils.GetMax(existing.Playtime, app.Playtime);
                        int playtime2wks = PlayTimeInLastTwoWeeks(lastPlayed, playTime);
                        steamApps[steamApps.IndexOf(existing)] = new SteamApp
                        {
                            AppId = app.AppId,
                            LastPlayed = lastPlayed,
                            Playtime = playTime,
                            Playtime2wks = playtime2wks
                        };
                    }
                    else
                    {
                        app.Playtime2wks = PlayTimeInLastTwoWeeks(app.LastPlayed, app.Playtime);
                        steamApps.Add(app);
                    }
                }
            }
        }

        private void CompleteScan()
        {
            scanProgress.CurrentStatus = "Scan Complete";
            scanProgress.Progress = scanProgress.MaxProgress;
            updateProgressDelegate(scanProgress);
        }

        private int CalculateGameCount()
        {
            int count = 0;
            foreach (string s in GameDirectories)
            {
                string steamapps = s + "\\steamapps\\";
                if (Directory.Exists(steamapps))
                {
                    count += Directory.GetFiles(steamapps, "*.acf").Count();
                }
            }
            return count + 1;
        }

        private string[] GetSteamDirectories()
        {
            List<string> dirs = new List<string>();
            dirs.Add(Location);
            string steamapps = this.Location + "\\steamapps\\";
            if (Directory.Exists(steamapps))
            {
                VProperty libfolders = VdfConvert.Deserialize(File.ReadAllText(Path.Combine(steamapps, "libraryfolders.vdf")));

                dirs.AddRange(VDFDataDiscovery.NumberKeyIteratory(libfolders));
            }

            return dirs.ToArray();
        }



        private void ScanSteamGames(string dir, ref ScanProgress scanProgress)
        {
            string steamapps = dir + "\\steamapps\\";

            if (Directory.Exists(steamapps))
            {
                string[] files = Directory.GetFiles(steamapps, "*.acf");
                foreach(string f in files)
                {
                    AddOrUpdateGame(steamapps, f, ref scanProgress);
                }
            }
        }

        private int PlayTimeInLastTwoWeeks(int lastplayed, int playtime)
        {
            DateTime twoWeeks = DateTime.Now.AddDays(-14);
            DateTime lastPlayedDT = Utils.ConvertEpochToDateTime(lastplayed);
            if (lastPlayedDT < twoWeeks)
                return 0;
            return playtime;
        }

        private List<LocalConfig> GetUserLocalConfigs(string dir)
        {
            if (!Directory.Exists(dir)) return new List<LocalConfig>();

            string[] users = Directory.GetDirectories(dir);
            List<LocalConfig> configs = new List<LocalConfig>();
            foreach (string s in users)
            {
                DirectoryInfo info = new DirectoryInfo(s);
                int userid = 0;
                if (int.TryParse(info.Name.ToString(), out userid))
                {
                    LocalConfig localConfig = new LocalConfig(s + "\\config\\localconfig.vdf");
                    configs.Add(localConfig);
                }
            }
            return configs;
        }

        private void AddOrUpdateGame(string steamappsDir, string manifestDir, ref ScanProgress scanProgress)
        {
            VProperty gameManifest = VdfConvert.Deserialize(File.ReadAllText(manifestDir));

            string gameDir = steamappsDir + "common\\" + gameManifest.Value["installdir"].ToString();

            SteamGame game = Data.GetGameByName(gameManifest.Value["name"].ToString()) as SteamGame;

            if (Exists(game))
            {
                scanProgress.UpdateProgress("Alreading tracking " + game.Name + ". Scanning", scanProgress.Progress);
                game.Scan();
                scanProgress.UpdateProgress("Scanned " + game.Name, scanProgress.Progress + 1);
            }
            else
            {
                scanProgress.UpdateProgress("Scanning " + gameManifest.Value["name"].ToString(), scanProgress.Progress);
                game = new Games.SteamGame(gameDir);
                scanProgress.UpdateProgress("Added " + game.Name, scanProgress.Progress + 1);
            }

            game.AppID = gameManifest.Value["appid"].ToString();
            game.Name = gameManifest.Value["name"].ToString();
            game.Manifest = manifestDir;

            if (Exists(game.AppID))
            {
                SteamApp app = GetSteamApp(int.Parse(game.AppID));

                if (Exists(app))
                {
                    game.LastPlayed = Utils.ConvertEpochToDateTime(app.LastPlayed);
                    game.PlayTime = app.Playtime;
                    game.PlayTime2Weeks = app.Playtime2wks;
                }
            }

            Data.UpdateGame(game);

            updateProgressDelegate(scanProgress);
        }

        private bool Exists(object obj)
        {
            return obj != null;
        }

        private SteamApp GetSteamApp(int appid)
        {
            return steamApps.Find(x => x.AppId == appid);
        }
    }
}
