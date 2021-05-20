using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types
{
    public class Launcher
    {
        public int LauncherID { get; set; }
        public int DriveID { get; set; }

        protected virtual void ScanGames()
        {

        }
    }
}
