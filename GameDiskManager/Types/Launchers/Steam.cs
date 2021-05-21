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
        public bool isSteam { get; set; } = true;

        public override void ScanGames()
        {
            base.ScanGames();

            GetSteamDirectories();
        }

        private string[] GetSteamDirectories()
        {
            string steamapps = this.Location + "\\steamapps\\";
            if (Directory.Exists(steamapps))
            {
                VProperty libfolders = VdfConvert.Deserialize(File.ReadAllText(Path.Combine(steamapps, "libraryfolders.vdf")));
                Console.WriteLine(libfolders.ToString());
            }

            return new string[1];
        }
    }
}
