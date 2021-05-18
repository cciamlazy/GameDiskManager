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
            ReloadListView();
        }

        ListViewGroup[] groups = new ListViewGroup[Data.Store.Drives.Count];


        private void ReloadListView()
        {
            gameList.Items.Clear();

            for (int i = 0; i < Data.Store.Drives.Count; i++)
            {
                groups[i] = new ListViewGroup
                {
                    Header = String.Format("{1} ({0}) \r\n {2} free of {3}", 
                        Data.Store.Drives[i].Name, Data.Store.Drives[i].VolumeLabel, Utils.BytesToString(Data.Store.Drives[i].AvailableFreeSpace), Utils.BytesToString(Data.Store.Drives[i].TotalSize)),
                    Name = Data.Store.Drives[i].Name,
                    Tag = Data.Store.Drives[i].DriveID
                };
                if (!gameList.Groups.Contains(groups[i]))
                    gameList.Groups.Add(groups[i]);
            }
            foreach (Game g in Data.Store.Games)
            {
                string[] arr = { g.Name, g.EZSize, (g.PercentDiskSpace * 100).ToString() + "%", g.Priority.ToString(), g.Active.ToString() };
                ListViewItem gi = new ListViewItem(arr);
                gi.Tag = g.GameID;
                gi.Group = Array.Find(groups, x => (int)x.Tag == g.DriveID);
                gameList.Items.Add(gi);
            }
        }

        private void toolStripAddGame_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string gamePath = fbd.SelectedPath;

                    Data.Store.Games.Add(new Game(gamePath));

                    Data.SaveDataStore();

                    ReloadListView();
                }
            }
        }
    }
}
