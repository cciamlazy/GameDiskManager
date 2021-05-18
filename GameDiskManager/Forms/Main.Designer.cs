
namespace GameDiskManager.Forms
{
    partial class Main
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("C:/", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.gameList = new System.Windows.Forms.ListView();
            this.game = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.size = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.diskTaken = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.priority = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.active = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripAddGame = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // gameList
            // 
            this.gameList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gameList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.game,
            this.size,
            this.diskTaken,
            this.priority,
            this.active});
            this.gameList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameList.FullRowSelect = true;
            this.gameList.GridLines = true;
            listViewGroup1.Header = "C:/";
            listViewGroup1.Name = "Test";
            this.gameList.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.gameList.HideSelection = false;
            this.gameList.Location = new System.Drawing.Point(0, 28);
            this.gameList.Name = "gameList";
            this.gameList.Size = new System.Drawing.Size(576, 339);
            this.gameList.TabIndex = 0;
            this.gameList.UseCompatibleStateImageBehavior = false;
            this.gameList.View = System.Windows.Forms.View.Details;
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
            // priority
            // 
            this.priority.Text = "Priority";
            // 
            // active
            // 
            this.active.Text = "Active";
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.White;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripFile});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(588, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripFile
            // 
            this.toolStripFile.BackColor = System.Drawing.Color.White;
            this.toolStripFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAddGame});
            this.toolStripFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripFile.Image")));
            this.toolStripFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripFile.Name = "toolStripFile";
            this.toolStripFile.Size = new System.Drawing.Size(38, 22);
            this.toolStripFile.Text = "File";
            // 
            // toolStripAddGame
            // 
            this.toolStripAddGame.BackColor = System.Drawing.Color.White;
            this.toolStripAddGame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripAddGame.Name = "toolStripAddGame";
            this.toolStripAddGame.Size = new System.Drawing.Size(180, 22);
            this.toolStripAddGame.Text = "Add Game";
            this.toolStripAddGame.Click += new System.EventHandler(this.toolStripAddGame_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(588, 379);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.gameList);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main";
            this.Text = "Main";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView gameList;
        private System.Windows.Forms.ColumnHeader game;
        private System.Windows.Forms.ColumnHeader size;
        private System.Windows.Forms.ColumnHeader diskTaken;
        private System.Windows.Forms.ColumnHeader priority;
        private System.Windows.Forms.ColumnHeader active;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripAddGame;
    }
}