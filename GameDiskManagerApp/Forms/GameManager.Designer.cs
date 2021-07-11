
using GameDiskManagerApp.Interfaces;

namespace GameDiskManagerApp.Forms
{
    partial class GameManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("C:/", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameManager));
            this.gameList = new System.Windows.Forms.ListView();
            this.image = new System.Windows.Forms.ColumnHeader();
            this.game = new System.Windows.Forms.ColumnHeader();
            this.size = new System.Windows.Forms.ColumnHeader();
            this.diskTaken = new System.Windows.Forms.ColumnHeader();
            this.lastPlayedHeader = new System.Windows.Forms.ColumnHeader();
            this.playtimeHeader = new System.Windows.Forms.ColumnHeader();
            this.playtime2WksHeader = new System.Windows.Forms.ColumnHeader();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripManage = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripAddGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripNewMigration = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripReloadGameList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLaunchers = new System.Windows.Forms.ToolStripDropDownButton();
            this.addLauncherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameImages = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameList
            // 
            this.gameList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gameList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.image,
            this.game,
            this.size,
            this.diskTaken,
            this.lastPlayedHeader,
            this.playtimeHeader,
            this.playtime2WksHeader});
            this.gameList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gameList.FullRowSelect = true;
            this.gameList.GridLines = true;
            listViewGroup1.Footer = "";
            listViewGroup1.Header = "C:/";
            listViewGroup1.Name = "Test";
            listViewGroup1.Subtitle = "";
            listViewGroup1.TaskLink = "";
            this.gameList.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.gameList.HideSelection = false;
            this.gameList.Location = new System.Drawing.Point(0, 28);
            this.gameList.Name = "gameList";
            this.gameList.Size = new System.Drawing.Size(588, 350);
            this.gameList.TabIndex = 0;
            this.gameList.UseCompatibleStateImageBehavior = false;
            this.gameList.View = System.Windows.Forms.View.Details;
            this.gameList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.gameList_ColumnClick);
            this.gameList.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.gameList_ItemDrag);
            this.gameList.DragDrop += new System.Windows.Forms.DragEventHandler(this.gameList_DragDrop);
            this.gameList.DragEnter += new System.Windows.Forms.DragEventHandler(this.gameList_DragEnter);
            this.gameList.DragOver += new System.Windows.Forms.DragEventHandler(this.gameList_DragOver);
            this.gameList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gameList_MouseUp);
            // 
            // image
            // 
            this.image.Text = "•";
            this.image.Width = 30;
            // 
            // game
            // 
            this.game.Text = "Game";
            this.game.Width = 120;
            // 
            // size
            // 
            this.size.Text = "Size";
            // 
            // diskTaken
            // 
            this.diskTaken.Text = "% Disk Space";
            // 
            // lastPlayedHeader
            // 
            this.lastPlayedHeader.Text = "Last Played";
            this.lastPlayedHeader.Width = 80;
            // 
            // playtimeHeader
            // 
            this.playtimeHeader.Text = "Playtime";
            // 
            // playtime2WksHeader
            // 
            this.playtime2WksHeader.Text = "Playtime last 2 weeks";
            this.playtime2WksHeader.Width = 150;
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.White;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripFile,
            this.toolStripManage,
            this.toolStripLaunchers});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(588, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripFile
            // 
            this.toolStripFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripFile.Image")));
            this.toolStripFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripFile.Name = "toolStripFile";
            this.toolStripFile.Size = new System.Drawing.Size(38, 22);
            this.toolStripFile.Text = "File";
            // 
            // toolStripManage
            // 
            this.toolStripManage.BackColor = System.Drawing.Color.White;
            this.toolStripManage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripManage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAddGame,
            this.toolStripNewMigration,
            this.toolStripReloadGameList});
            this.toolStripManage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripManage.Image")));
            this.toolStripManage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripManage.Name = "toolStripManage";
            this.toolStripManage.Size = new System.Drawing.Size(63, 22);
            this.toolStripManage.Text = "Manage";
            // 
            // toolStripAddGame
            // 
            this.toolStripAddGame.BackColor = System.Drawing.Color.White;
            this.toolStripAddGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripAddGame.Name = "toolStripAddGame";
            this.toolStripAddGame.Size = new System.Drawing.Size(206, 22);
            this.toolStripAddGame.Text = "Add Game";
            this.toolStripAddGame.Click += new System.EventHandler(this.toolStripAddGame_Click);
            // 
            // toolStripNewMigration
            // 
            this.toolStripNewMigration.BackColor = System.Drawing.Color.White;
            this.toolStripNewMigration.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripNewMigration.Name = "toolStripNewMigration";
            this.toolStripNewMigration.Size = new System.Drawing.Size(206, 22);
            this.toolStripNewMigration.Text = "New Migration";
            this.toolStripNewMigration.Click += new System.EventHandler(this.toolStripNewMigration_Click);
            // 
            // toolStripReloadGameList
            // 
            this.toolStripReloadGameList.Name = "toolStripReloadGameList";
            this.toolStripReloadGameList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.toolStripReloadGameList.Size = new System.Drawing.Size(206, 22);
            this.toolStripReloadGameList.Text = "Reload Game List";
            this.toolStripReloadGameList.Click += new System.EventHandler(this.toolStripReloadGameList_Click);
            // 
            // toolStripLaunchers
            // 
            this.toolStripLaunchers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLaunchers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLauncherToolStripMenuItem});
            this.toolStripLaunchers.Image = ((System.Drawing.Image)(resources.GetObject("toolStripLaunchers.Image")));
            this.toolStripLaunchers.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLaunchers.Name = "toolStripLaunchers";
            this.toolStripLaunchers.Size = new System.Drawing.Size(74, 22);
            this.toolStripLaunchers.Text = "Launchers";
            // 
            // addLauncherToolStripMenuItem
            // 
            this.addLauncherToolStripMenuItem.Name = "addLauncherToolStripMenuItem";
            this.addLauncherToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.addLauncherToolStripMenuItem.Text = "Add Launcher";
            // 
            // gameImages
            // 
            this.gameImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.gameImages.ImageSize = new System.Drawing.Size(16, 16);
            this.gameImages.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // GameManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(588, 379);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.gameList);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "GameManager";
            this.Text = "Game Manager";
            this.SizeChanged += new System.EventHandler(this.GameManager_SizeChanged);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView gameList;
        private GameListColumnSorter gameListColumnSorter;
        private System.Windows.Forms.ColumnHeader game;
        private System.Windows.Forms.ColumnHeader size;
        private System.Windows.Forms.ColumnHeader diskTaken;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripManage;
        private System.Windows.Forms.ToolStripMenuItem toolStripAddGame;
        private System.Windows.Forms.ToolStripMenuItem toolStripNewMigration;
        private System.Windows.Forms.ToolStripDropDownButton toolStripLaunchers;
        private System.Windows.Forms.ToolStripMenuItem addLauncherToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripFile;
        private System.Windows.Forms.ColumnHeader image;
        private System.Windows.Forms.ImageList gameImages;
        private System.Windows.Forms.ToolStripMenuItem toolStripReloadGameList;
        private System.Windows.Forms.ColumnHeader lastPlayedHeader;
        private System.Windows.Forms.ColumnHeader playtimeHeader;
        private System.Windows.Forms.ColumnHeader playtime2WksHeader;
    }
}