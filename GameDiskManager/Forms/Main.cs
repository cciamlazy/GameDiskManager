using GameDiskManager.Types;
using GameDiskManager.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameDiskManager.Forms
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            InitializeGameList();
        }

        private void InitializeGameList()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            List<Drive> drives = new List<Drive>();

            foreach (DriveInfo d in allDrives)
            {
                Drive drive = new Drive
                {
                    Name = d.Name,
                    VolumeLabel = d.VolumeLabel,
                    DriveType = d.DriveType,
                    TotalSize = d.TotalSize,
                    TotalFreeSpace = d.TotalFreeSpace,
                    AvailableFreeSpace = d.AvailableFreeSpace,
                    IsReady = d.IsReady,
                    Active = true,
                    Priority = 3,
                };
                drives.Add(drive);
            }
        }
    }
}
