using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types
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
        public bool IsReady { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }


        public bool Equals(Drive other)
        {
            if (other is null)
                return false;

            return this.DriveID == other.DriveID || (this.Name == other.Name && this.TotalSize == other.TotalSize);
        }

        public override bool Equals(object obj) => Equals(obj as Drive);
        public override int GetHashCode() => (DriveID, Name).GetHashCode();
    }
}
