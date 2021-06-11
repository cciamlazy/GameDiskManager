
namespace GameDiskManager.Forms
{
    partial class MigrationProgress
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
            this.label1 = new System.Windows.Forms.Label();
            this.elapsedTime = new System.Windows.Forms.Label();
            this.estimatedTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.errors = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.files = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.processedAmt = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.speed = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.totalSize = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.migratingFileName = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.errorList = new System.Windows.Forms.ListBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.backgroundButton = new System.Windows.Forms.Button();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Elapsed time:";
            // 
            // elapsedTime
            // 
            this.elapsedTime.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.elapsedTime.Location = new System.Drawing.Point(208, 17);
            this.elapsedTime.Name = "elapsedTime";
            this.elapsedTime.Size = new System.Drawing.Size(56, 17);
            this.elapsedTime.TabIndex = 1;
            this.elapsedTime.Text = "00:00:00";
            this.elapsedTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // estimatedTime
            // 
            this.estimatedTime.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.estimatedTime.Location = new System.Drawing.Point(208, 44);
            this.estimatedTime.Name = "estimatedTime";
            this.estimatedTime.Size = new System.Drawing.Size(56, 17);
            this.estimatedTime.TabIndex = 3;
            this.estimatedTime.Text = "00:00:00";
            this.estimatedTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Estimated time:";
            // 
            // errors
            // 
            this.errors.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.errors.Location = new System.Drawing.Point(249, 98);
            this.errors.Name = "errors";
            this.errors.Size = new System.Drawing.Size(15, 17);
            this.errors.TabIndex = 7;
            this.errors.Text = "0";
            this.errors.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Errors:";
            // 
            // files
            // 
            this.files.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.files.Location = new System.Drawing.Point(237, 71);
            this.files.Name = "files";
            this.files.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.files.Size = new System.Drawing.Size(27, 17);
            this.files.TabIndex = 5;
            this.files.Text = "0/0";
            this.files.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 17);
            this.label7.TabIndex = 4;
            this.label7.Text = "Files:";
            // 
            // processedAmt
            // 
            this.processedAmt.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.processedAmt.Location = new System.Drawing.Point(570, 71);
            this.processedAmt.Name = "processedAmt";
            this.processedAmt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.processedAmt.Size = new System.Drawing.Size(27, 17);
            this.processedAmt.TabIndex = 13;
            this.processedAmt.Text = "0 K";
            this.processedAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(348, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 17);
            this.label4.TabIndex = 12;
            this.label4.Text = "Processed:";
            // 
            // speed
            // 
            this.speed.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.speed.Location = new System.Drawing.Point(552, 44);
            this.speed.Name = "speed";
            this.speed.Size = new System.Drawing.Size(45, 17);
            this.speed.TabIndex = 11;
            this.speed.Text = "0 KB/s";
            this.speed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(348, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 17);
            this.label8.TabIndex = 10;
            this.label8.Text = "Speed:";
            // 
            // totalSize
            // 
            this.totalSize.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.totalSize.Location = new System.Drawing.Point(566, 17);
            this.totalSize.Name = "totalSize";
            this.totalSize.Size = new System.Drawing.Size(31, 17);
            this.totalSize.TabIndex = 9;
            this.totalSize.Text = "0 M";
            this.totalSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(348, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 17);
            this.label10.TabIndex = 8;
            this.label10.Text = "Total size:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 125);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 17);
            this.label11.TabIndex = 14;
            this.label11.Text = "Migrating";
            // 
            // migratingFileName
            // 
            this.migratingFileName.AutoSize = true;
            this.migratingFileName.Location = new System.Drawing.Point(15, 152);
            this.migratingFileName.Name = "migratingFileName";
            this.migratingFileName.Size = new System.Drawing.Size(67, 17);
            this.migratingFileName.TabIndex = 15;
            this.migratingFileName.Text = "File/Name";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(18, 182);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(573, 30);
            this.progressBar.TabIndex = 16;
            // 
            // errorList
            // 
            this.errorList.BackColor = System.Drawing.Color.White;
            this.errorList.FormattingEnabled = true;
            this.errorList.ItemHeight = 17;
            this.errorList.Location = new System.Drawing.Point(18, 219);
            this.errorList.Name = "errorList";
            this.errorList.Size = new System.Drawing.Size(573, 174);
            this.errorList.TabIndex = 17;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(478, 399);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(113, 29);
            this.cancelButton.TabIndex = 18;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(359, 399);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(113, 29);
            this.pauseButton.TabIndex = 19;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            // 
            // backgroundButton
            // 
            this.backgroundButton.Location = new System.Drawing.Point(240, 399);
            this.backgroundButton.Name = "backgroundButton";
            this.backgroundButton.Size = new System.Drawing.Size(113, 29);
            this.backgroundButton.TabIndex = 20;
            this.backgroundButton.Text = "Background";
            this.backgroundButton.UseVisualStyleBackColor = true;
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // MigrationProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(603, 433);
            this.Controls.Add(this.backgroundButton);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.errorList);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.migratingFileName);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.processedAmt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.speed);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.totalSize);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.errors);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.files);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.estimatedTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.elapsedTime);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MigrationProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MigrationProgress";
            this.Load += new System.EventHandler(this.MigrationProgress_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label elapsedTime;
        private System.Windows.Forms.Label estimatedTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label errors;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label files;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label processedAmt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label speed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label totalSize;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label migratingFileName;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ListBox errorList;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button backgroundButton;
        private System.Windows.Forms.Timer updateTimer;
    }
}