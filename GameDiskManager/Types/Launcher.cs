using GameDiskManager.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public virtual void CloseLauncher()
        {
            Process[] runningProcesses = Process.GetProcesses();
            foreach (Process process in runningProcesses)
            {
                try
                {
                    // now check the modules of the process
                    foreach (ProcessModule module in process.Modules)
                    {
                        if (module.FileName.Equals(Path.GetFileName(this.ExecutableLocation)))
                        {
                            process.Kill();
                        }
                        else
                        {

                        }
                    }
                }
                catch(Exception e)
                {

                }
            }
        }
    }
}
