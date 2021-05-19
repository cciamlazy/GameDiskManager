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
            InitMenuStrip();
        }


        private ContextMenuStrip gameMenuOptions;

        private void InitMenuStrip()
        {
            ToolStripItem[] toolStripItems = new ToolStripItem[Data.Store.Drives.Count];
            for (int i = 0; i < Data.Store.Drives.Count; i++)
            {
                toolStripItems[i] = new ToolStripMenuItem { Text = "Migrate To " + Data.Store.Drives[i].Name, BackColor = Color.White, Tag = Data.Store.Drives[i].DriveID };
                toolStripItems[i].Click += MoveToDriveItem_Click;
            }
            
            gameMenuOptions = new ContextMenuStrip();
            gameMenuOptions.Items.AddRange(toolStripItems);
        }

        private void MoveToDriveItem_Click(object sender, EventArgs e)
        {
            if (SelectedMenuItem == null || SelectedMenuItem.Tag == null)
                return;

            Console.WriteLine(sender.GetType().Name);
            //ToolStripMenuItem pressed = (ToolStripMenuItem)sender;
            //Data.Store.Drives.Find(x => x.DriveID == (int)pressed.Tag);
            

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
                string[] arr = { g.Name, g.EZSize, Math.Round((g.PercentDiskSpace * 100), 2).ToString() + "%", g.Priority.ToString(), g.Active.ToString() };
                ListViewItem gi = new ListViewItem(arr);
                gi.Tag = g.GameID;
                gi.Group = Array.Find(groups, x => (int)x.Tag == g.DriveID);
                gameList.Items.Add(gi);
            }
            gameList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            gameList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
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

        ListViewItem lviDraggedItem;

        private void gameList_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gameList_DragDrop(object sender, DragEventArgs e)
        {
            ListViewHitTestInfo htInfo = gameList.HitTest(gameList.PointToClient(new Point(e.X, e.Y)));

            ListViewItem lviSibling = htInfo.Item;

            ListViewGroup lvgGroup = lviSibling.Group;

            lvgGroup.Items.Add(lviDraggedItem);

            lviDraggedItem = null;
        }

        private void gameList_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lviDraggedItem = (ListViewItem)e.Item;
            gameList.DoDragDrop(gameList.SelectedItems, DragDropEffects.Move);
        }

        private void gameList_DragOver(object sender, DragEventArgs e)
        {
            ListViewHitTestInfo htInfo = gameList.HitTest(gameList.PointToClient(new Point(e.X, e.Y)));

            ListViewItem lviSibling = htInfo.Item;

            ListViewGroup lvgGroup = lviSibling.Group;
        }

        private ListViewItem SelectedMenuItem;

        /// <summary>
        /// Open Menu when right clicking game item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gameList_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if ((gameList.FocusedItem != null && gameList.FocusedItem.Bounds.Contains(e.Location)) || (gameList.SelectedItems != null && gameList.SelectedItems.Count == 1 && gameList.SelectedItems[0].Bounds.Contains(e.Location)))
                {
                    gameMenuOptions.Show(Cursor.Position);
                    SelectedMenuItem = gameList.FocusedItem;
                    Game g = Data.Store.Games.Find(x => x.GameID == (int)SelectedMenuItem.Tag);

                    // Set current drive to not be visible on context menu
                    for (int i = 0; i < gameMenuOptions.Items.Count; i++)
                        gameMenuOptions.Items[i].Visible = i == IndexOf(gameMenuOptions.Items, g.DriveID) ? false : true;
                                    }
                else
                {
                    gameMenuOptions.Show(Cursor.Position);
                    SelectedMenuItem = null;
                }
            }
        }

        private static int IndexOf(ToolStripItemCollection list, int value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if ((int)list[i].Tag == value)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
