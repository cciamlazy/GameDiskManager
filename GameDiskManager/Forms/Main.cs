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

        #region Menu Option Events

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

        private void EditGameItem_Click(object sender, EventArgs e)
        {
            if (SelectedGameItem == null || SelectedGameItem.Tag == null)
                return;

            Game game = Data.Store.Games.Find(x => x.GameID == (int)SelectedGameItem.Tag);
            using(GameConfig gc = new GameConfig(game))
            {
                if (gc.ShowDialog() == DialogResult.OK)
                {
                    Data.Store.Games.
                }
            }
        }

        private void RemoveGameItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripNewMigration_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region RightClickMenu

        private ListViewItem SelectedGameItem;
        private ContextMenuStrip gameMenuOptions;
        const int staticMenuItems = 5;

        private void InitMenuStrip()
        {
            ToolStripItem[] toolStripItems = new ToolStripItem[staticMenuItems + Data.Store.Drives.Count];

            // Static menu items
            toolStripItems[0] = new ToolStripMenuItem { Text = "Add Game", BackColor = Color.White };
            toolStripItems[0].Click += toolStripAddGame_Click;

            toolStripItems[1] = new ToolStripMenuItem { Text = "Edit Game Config", BackColor = Color.White };
            toolStripItems[1].Click += EditGameItem_Click;

            toolStripItems[2] = new ToolStripMenuItem { Text = "Remove Game", BackColor = Color.White };
            toolStripItems[2].Click += RemoveGameItem_Click;

            toolStripItems[3] = new ToolStripMenuItem { Text = "New Migration", BackColor = Color.White };
            toolStripItems[3].Click += toolStripNewMigration_Click;

            toolStripItems[staticMenuItems - 1] = new ToolStripMenuItem { Text = "Quick Migrate:", BackColor = Color.White, Enabled = false };

            // Dynamic drive items
            for (int i = 0; i < Data.Store.Drives.Count; i++)
            {
                toolStripItems[staticMenuItems + i] = new ToolStripMenuItem { Text = "Migrate To " + Data.Store.Drives[i].Name, BackColor = Color.White, Tag = Data.Store.Drives[i].DriveID };
                toolStripItems[staticMenuItems + i].Click += MoveToDriveItem_Click;
            }

            gameMenuOptions = new ContextMenuStrip();
            gameMenuOptions.Items.AddRange(toolStripItems);
        }

        private void MoveToDriveItem_Click(object sender, EventArgs e)
        {
            if (SelectedGameItem == null || SelectedGameItem.Tag == null)
                return;

            ToolStripMenuItem pressed = (ToolStripMenuItem)sender;
            Drive d = Data.Store.Drives.Find(x => x.DriveID == (int)pressed.Tag);
        }

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
                    SelectedGameItem = gameList.FocusedItem;
                    Game g = Data.Store.Games.Find(x => x.GameID == (int)SelectedGameItem.Tag);

                    gameMenuOptions.Items[1].Visible = true;
                    gameMenuOptions.Items[2].Visible = true;
                    gameMenuOptions.Items[staticMenuItems - 1].Visible = true;
                    // Set current drive to not be visible on context menu
                    for (int i = staticMenuItems; i < gameMenuOptions.Items.Count; i++)
                    {
                        bool enabled = true;
                        Drive d = Data.Store.Drives.Find(x => x.DriveID == (int)gameMenuOptions.Items[i].Tag);
                        if (d.AvailableFreeSpace + d.KeepSpaceAvailable < g.Size)
                        {
                            enabled = false;
                            gameMenuOptions.Items[i].Text = gameMenuOptions.Items[i].Text + " (Not enough space)";
                        }
                        else
                        {
                            enabled = true;
                            gameMenuOptions.Items[i].Text = gameMenuOptions.Items[i].Text.Replace(" (Not enough space)", "");
                        }
                        gameMenuOptions.Items[i].Enabled = enabled;
                        gameMenuOptions.Items[i].Visible = i == IndexOf(gameMenuOptions.Items, g.DriveID) ? false : true;
                    }
                    gameMenuOptions.Show(Cursor.Position);
                }
                else
                {
                    gameMenuOptions.Items[1].Visible = false;
                    gameMenuOptions.Items[2].Visible = false;
                    gameMenuOptions.Items[staticMenuItems - 1].Visible = false;
                    for (int i = staticMenuItems; i < gameMenuOptions.Items.Count; i++)
                    {
                        gameMenuOptions.Items[i].Visible = false;
                    }
                    gameMenuOptions.Show(Cursor.Position);
                    SelectedGameItem = null;
                }
            }
        }

        private static int IndexOf(ToolStripItemCollection list, int value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Tag != null && (int)list[i].Tag == value)
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion
    }
}
