using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib
{
    public class Drive : IEquatable<Drive>
    {
        public int DriveID { get; set; }
        public string Name { get; set; }
        public string VolumeLabel { get; set; }
        public DriveType DriveType { get; set; }
        public long TotalSize { get; set; }
        public long TotalFreeSpace { get; set; }
        public long AvailableFreeSpace { get; set; }
        public long KeepSpaceAvailable { get; set; }
        public bool IsReady { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }

        /// TODO: Implement winsat information

        public bool Equals(Drive other)
        {
            if (other is null)
                return false;

            return this.DriveID == other.DriveID || (this.Name == other.Name && this.TotalSize == other.TotalSize);
        }

        public async void RunWinSatTest()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = String.Format("winsat disk -drive {0} -xml {1}{2}.xml", Name.Replace(":\\", ""), FileSystemHandler.CombineDataPath("winsat\\"), Name.Replace(":\\", ""));
            process.StartInfo = startInfo;
            process.Start();

            // Wait until file is written
            await Task.Delay(5000);
        }

        public static int GetDriveID(string dir)
        {
            return GetDriveID(new DirectoryInfo(dir));
        }

        public static int GetDriveID(DirectoryInfo dir)
        {
            return Data.Store.Drives.Find(x => dir.FullName.Contains(x.Name)).DriveID;
        }

        public override bool Equals(object obj) => Equals(obj as Drive);
        public override int GetHashCode() => (DriveID, Name).GetHashCode();
    }
}
