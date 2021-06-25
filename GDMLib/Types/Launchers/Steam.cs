using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using GDMLib.Games;
using GDMLib.TransitoryData;
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
        ScanProgress scanProgress = new ScanProgress();
        public override void ScanGames(UpdateProgressDelegate callback)
        {
            GameDirectories = GetSteamDirectories();

            scanProgress.MaxProgress = CalculateGameCount(); //Duplication of code, refactor this

            callback(scanProgress);

            foreach (string s in GameDirectories)
            {
                ScanSteamGames(s, ref scanProgress);
            }

            scanProgress.CurrentStatus = "Scan Complete";
            scanProgress.Progress = scanProgress.MaxProgress;
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
            return count;
        }

        private string[] GetSteamDirectories()
        {
            List<string> dirs = new List<string>();
            dirs.Add(Location);
            string steamapps = this.Location + "\\steamapps\\";
            if (Directory.Exists(steamapps))
            {
                VProperty libfolders = VdfConvert.Deserialize(File.ReadAllText(Path.Combine(steamapps, "libraryfolders.vdf")));

                int i = 1;
                bool flag = false;
                while (!flag)
                {
                    string key = i++.ToString();
                    if (libfolders.Value[key] != null)
                    {
                        dirs.Add(libfolders.Value[key].ToString());
                    }
                    else
                        flag = true;
                }
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
                Data.SaveDataStore();
            }
        }

        private void AddOrUpdateGame(string steamappsDir, string manifestDir, ref ScanProgress scanProgress)
        {
            VProperty gameManifest = VdfConvert.Deserialize(File.ReadAllText(manifestDir));

            List<ConfigFile> configFiles = new List<ConfigFile>();

            string gameDir = steamappsDir + "common\\" + gameManifest.Value["installdir"].ToString();

            configFiles.Add(new ConfigFile { 
                KeepLocation = false, 
                Location = manifestDir, 
                KeepRelative = true, 
                Identifier = ConfigIdentifier.Manifest, 
                RelativeLocation = "..\\" + Utils.GetRelativePath(gameDir, manifestDir)
            });

            SteamGame game = Data.Store.Games.Find(x => x.Name.Replace(" ", "").ToLower() == gameManifest.Value["name"].ToString().Replace(" ", "").ToLower()) as SteamGame;

            if (game == null)
            {
                scanProgress.UpdateProgress("Scanning " + gameManifest.Value["name"].ToString(), scanProgress.Progress + 1);
                game = new Games.SteamGame(gameDir);

                game.AppID = gameManifest.Value["appid"].ToString();
                game.Name = gameManifest.Value["name"].ToString();
                game.Manifest = manifestDir;

                scanProgress.UpdateProgress("Added " + game.Name, scanProgress.Progress + 1);
            }
            else
            {
                game.Scan();

                scanProgress.UpdateProgress("Alreading tracking " + game.Name + ". Scanning", scanProgress.Progress + 2);
            }
        }
    }
}
