using GameDiskManager.Forms;
using GameDiskManager.Types.Games;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types.Launchers
{
    public class Steam : Launcher
    {
        public string[] SteamGameDirectories { get; set; }
        ScanProgress _progress;

        public async override Task<bool> ScanGames()
        {
            await base.ScanGames();

            _progress = new ScanProgress();
            _progress.Show();

            SteamGameDirectories = GetSteamDirectories();

            _progress.ProgressMax(CalculateGameCount());

            await Task.Run(() =>
            {
                foreach (string s in SteamGameDirectories)
                {
                    GetSteamGames(s);
                }
            });
            _progress.UpdateProgress("Scan complete", _progress.GetProgress() + 1);
            _progress.Close();
            return true;
        }

        private int CalculateGameCount()
        {
            int count = 0;
            foreach (string s in SteamGameDirectories)
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

        private void GetSteamGames(string dir)
        {
            string steamapps = dir + "\\steamapps\\";
            if (Directory.Exists(steamapps))
            {
                string[] files = Directory.GetFiles(steamapps, "*.acf");
                foreach(string f in files)
                {
                    VProperty gameManifest = VdfConvert.Deserialize(File.ReadAllText(f));

                    List<ConfigFile> configFiles = new List<ConfigFile>();
                    configFiles.Add(new ConfigFile { KeepLocation = false, Location = f });

                    string gameDir = steamapps + "common\\" + gameManifest.Value["installdir"].ToString();

                    if (Data.Store.Games.Find(x => /*x.GetType() == typeof(SteamGame) &&*/ x.Name.Replace(" ","").ToLower() == gameManifest.Value["name"].ToString().Replace(" ", "").ToLower()) == null)
                    {
                        _progress.UpdateProgress("Scanning " + gameManifest.Value["name"].ToString(), _progress.GetProgress() + 1);
                        SteamGame game = new SteamGame(gameDir);

                        game.AppID = gameManifest.Value["appid"].ToString();
                        game.Name = gameManifest.Value["name"].ToString();

                        Data.Store.Games.Add(game);

                        _progress.UpdateProgress("Added " + game.Name, _progress.GetProgress() + 1);
                    }
                    else
                    {
                        _progress.UpdateProgress("Tracked game. Skipping", _progress.GetProgress() + 2);
                    }
                }
                Data.SaveDataStore();
            }
        }
    }
}
