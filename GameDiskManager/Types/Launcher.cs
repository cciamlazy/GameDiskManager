using GameDiskManager.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types
{
    public enum LauncherType
    {
        Steam
    }
    public class Launcher
    {
        public int LauncherID { get; set; }
        public int DriveID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ExecutableLocation { get; set; }
        public LauncherType LauncherType { get; set; }
        public string[] GameDirectories { get; set; }

        public async virtual Task<bool> ScanGames()
        {
            return true;
        }
    }
}
