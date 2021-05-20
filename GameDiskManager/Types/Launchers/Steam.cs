using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types.Launchers
{
    public class Steam : Launcher
    {


        protected override void ScanGames()
        {
            base.ScanGames();


        }

        private string[] GetSteamDirectories()
        {
            var mainDir = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath", "");

            var Dirx62 = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", "");
        }
    }
}
