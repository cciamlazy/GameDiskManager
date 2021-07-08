﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDMLib;

namespace GameDiskManagerApp.Forms
{
    public partial class GameManager : Form
    {
        public GameManager()
        {
            InitializeComponent();
            InitializeLaunchers();
            InitializeGameList();
        }
        private void InitializeGameList()
        {
            ReloadListView();
            InitMenuStrip();
        }

        ListViewGroup[] driveGroups = new ListViewGroup[Data.DriveCount];

        ToolStripMenuItem[] launcherMenuItems;
        ToolStripMenuItem[,] launcherMenuSubItems;

        private void InitializeLaunchers()
        {
            launcherMenuItems = new ToolStripMenuItem[Data.LauncherCount];
            launcherMenuSubItems = new ToolStripMenuItem[Data.LauncherCount, 2];
            for (int i = 0; i < Data.LauncherCount; i++)
            {
                Launcher l = Data.GetLauncherByIndex(i);

                // 
                // scanMenuItem
                // 
                launcherMenuSubItems[i, 0] = new ToolStripMenuItem();
                launcherMenuSubItems[i, 0].Name = l.Name + "ScanMenuItem";
                launcherMenuSubItems[i, 0].Size = new System.Drawing.Size(180, 22);
                launcherMenuSubItems[i, 0].Text = "Scan";
                launcherMenuSubItems[i, 0].Tag = l.LauncherID;
                launcherMenuSubItems[i, 0].Click += launcherScanMenu_Click;

                // 
                // LauncherMenuItem
                // 
                launcherMenuItems[i] = new ToolStripMenuItem();
                launcherMenuItems[i].DropDownItems.AddRange(new ToolStripItem[] { launcherMenuSubItems[i, 0] });
                launcherMenuItems[i].Name = l.Name + "MenuItem";
                launcherMenuItems[i].Size = new System.Drawing.Size(180, 22);
                launcherMenuItems[i].Text = l.Name;
                if (File.Exists(l.ExecutableLocation))
                {
                    Icon icon = Icon.ExtractAssociatedIcon(l.ExecutableLocation);
                    launcherMenuItems[i].Image = icon.ToBitmap();
                }
            }
            this.toolStripLaunchers.DropDownItems.AddRange(launcherMenuItems);
        }

        private void ReloadListView()
        {
            ClearGameLists();
            gameList.SmallImageList = gameImages;

            for (int i = 0; i < Data.DriveCount; i++)
            {
                driveGroups[i] = getDriveListGroup(Data.GetDriveByIndex(i));
                if (!gameList.Groups.Contains(driveGroups[i]))
                    gameList.Groups.Add(driveGroups[i]);
            }
            foreach (Game g in Data.GameList)
            {
                gameList.Items.Add(getGameListItem(g));
            }
            gameList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            gameList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private ListViewItem getGameListItem(Game game)
        {
            string[] arr = { "", game.Name, game.EZSize, getPercentDisk(game.PercentDiskSpace), game.Priority.ToString(), game.Active.ToString() };
            ListViewItem gameItem = new ListViewItem(arr);
            gameItem.Tag = game.GameID;
            gameItem.Group = getDriveGroup(game.DriveID);
            if (Setting.Value.Load_GameIcons)
                gameItem.ImageIndex = AddImageGetIndex(GetGameIcon(game.ExecutableLocation));

            return gameItem;
        }

        private Icon GetGameIcon(string exeLocation)
        {
            if (exeLocation != null && File.Exists(exeLocation))
            {
                return Icon.ExtractAssociatedIcon(exeLocation);
            }
            return null;
        }

        private int AddImageGetIndex(Icon icon)
        {
            int imageIndex = -1;
            if (icon != null)
            {
                gameImages.Images.Add(icon);
                imageIndex = gameImages.Images.Count - 1;
            }

            return imageIndex;
        }

        private ListViewGroup getDriveGroup(int DriveID)
        {
            return Array.Find(driveGroups, x => (int)x.Tag == DriveID);
        }

        private string getPercentDisk(double percent)
        {
            return Math.Round((percent * 100), 2).ToString() + "%";
        }

        private ListViewGroup getDriveListGroup(Drive drive)
        {
            return new ListViewGroup
            {
                Header = String.Format("{1} ({0}) \r\n {2} free of {3}",
                        drive.Name, drive.VolumeLabel, Utils.BytesToString(drive.AvailableFreeSpace), Utils.BytesToString(drive.TotalSize)),
                Name = drive.Name,
                Tag = drive.DriveID
            };
        }

        private void ClearGameLists()
        {
            gameList.Items.Clear();
            gameImages.Images.Clear();
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

                    new Game(gamePath);

                    ReloadListView();
                }
            }
        }

        private void EditGameItem_Click(object sender, EventArgs e)
        {
            if (SelectedGameItem == null || SelectedGameItem.Tag == null)
                return;

            Game game = Data.GetGameByID((int)SelectedGameItem.Tag);
            using (GameConfiguration gc = new GameConfiguration(game))
            {
                if (gc.ShowDialog() == DialogResult.OK)
                {
                    //Data.Store.Games.
                }
            }
        }

        private void RemoveGameItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripNewMigration_Click(object sender, EventArgs e)
        {

        }

        private void toolStripReloadGameList_Click(object sender, EventArgs e)
        {
            ReloadListView();
        }

        private  void launcherScanMenu_Click(object sender, EventArgs e)
        {
            if (sender == null || ((ToolStripMenuItem)sender).Tag == null)
                return;
            ToolStripMenuItem clickedMenuItem = sender as ToolStripMenuItem;
            
            int launcherId = (int)clickedMenuItem.Tag;

            ScanProgressForm scan = new ScanProgressForm(new UpdateViewDelegate(ReloadListView));
            scan.Show();
            scan.ScanGames(launcherId);

            //ReloadListView();
        }
        #endregion

        #region RightClickMenu

        private ListViewItem SelectedGameItem;
        private ContextMenuStrip gameMenuOptions;
        const int staticMenuItems = 5;

        private void InitMenuStrip()
        {
            ToolStripItem[] toolStripItems = new ToolStripItem[staticMenuItems + Data.DriveCount];

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
            for (int i = 0; i < Data.DriveCount; i++)
            {
                Drive drive = Data.DriveByID(i);
                toolStripItems[staticMenuItems + i] = new ToolStripMenuItem { Text = "Migrate To " + drive.Name, BackColor = Color.White, Tag = drive.DriveID };
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
            int toDriveId = (int)pressed.Tag;
            int gameId = (int)SelectedGameItem.Tag;

            //Drive d = Data.DriveByID(toDriveId);

            Game g = Data.GameByID(gameId);

            GameMigration migration = g.GenerateMigration(toDriveId);

            using (MigrationProgress mp = new MigrationProgress(migration))
            {
                mp.ShowDialog();
                //mp.StartMigration();
            }

            ReloadListView();
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
                    Game g = Data.GameByID((int)SelectedGameItem.Tag);

                    gameMenuOptions.Items[1].Visible = true;
                    gameMenuOptions.Items[2].Visible = true;
                    gameMenuOptions.Items[staticMenuItems - 1].Visible = true;
                    // Set current drive to not be visible on context menu
                    for (int i = staticMenuItems; i < gameMenuOptions.Items.Count; i++)
                    {
                        bool enabled = true;
                        Drive d = Data.DriveByID((int)gameMenuOptions.Items[i].Tag);
                        if (d.AvailableFreeSpace + d.KeepSpaceAvailable < g.Size)
                        {
                            enabled = false;
                            gameMenuOptions.Items[i].Text = gameMenuOptions.Items[i].Text.Replace(" (Not enough space)", "") + " (Not enough space)";
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
