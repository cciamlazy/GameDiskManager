
namespace GDMUpdater
{
    partial class Updater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Updater));
            this.UpdateAvailable = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ReleaseNotes = new System.Windows.Forms.TextBox();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.RemindButton = new System.Windows.Forms.Button();
            this.SkipButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdateAvailable
            // 
            this.UpdateAvailable.AutoSize = true;
            this.UpdateAvailable.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateAvailable.Location = new System.Drawing.Point(13, 13);
            this.UpdateAvailable.Name = "UpdateAvailable";
            this.UpdateAvailable.Size = new System.Drawing.Size(229, 21);
            this.UpdateAvailable.TabIndex = 0;
            this.UpdateAvailable.Text = "An Optional Update Is Available";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ReleaseNotes);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(17, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 201);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "What\'s New";
            // 
            // ReleaseNotes
            // 
            this.ReleaseNotes.BackColor = System.Drawing.Color.White;
            this.ReleaseNotes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ReleaseNotes.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReleaseNotes.Location = new System.Drawing.Point(6, 24);
            this.ReleaseNotes.Multiline = true;
            this.ReleaseNotes.Name = "ReleaseNotes";
            this.ReleaseNotes.ReadOnly = true;
            this.ReleaseNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ReleaseNotes.Size = new System.Drawing.Size(294, 171);
            this.ReleaseNotes.TabIndex = 0;
            // 
            // UpdateButton
            // 
            this.UpdateButton.FlatAppearance.BorderSize = 0;
            this.UpdateButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.UpdateButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.UpdateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UpdateButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateButton.Location = new System.Drawing.Point(17, 255);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(78, 40);
            this.UpdateButton.TabIndex = 2;
            this.UpdateButton.Text = "Update";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // RemindButton
            // 
            this.RemindButton.FlatAppearance.BorderSize = 0;
            this.RemindButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.RemindButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.RemindButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemindButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RemindButton.Location = new System.Drawing.Point(98, 255);
            this.RemindButton.Name = "RemindButton";
            this.RemindButton.Size = new System.Drawing.Size(145, 40);
            this.RemindButton.TabIndex = 3;
            this.RemindButton.Text = "Remind Me Later";
            this.RemindButton.UseVisualStyleBackColor = true;
            this.RemindButton.Click += new System.EventHandler(this.RemindButton_Click);
            // 
            // SkipButton
            // 
            this.SkipButton.FlatAppearance.BorderSize = 0;
            this.SkipButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.SkipButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SkipButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SkipButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SkipButton.Location = new System.Drawing.Point(246, 255);
            this.SkipButton.Name = "SkipButton";
            this.SkipButton.Size = new System.Drawing.Size(77, 40);
            this.SkipButton.TabIndex = 4;
            this.SkipButton.Text = "Skip";
            this.SkipButton.UseVisualStyleBackColor = true;
            this.SkipButton.Click += new System.EventHandler(this.SkipButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(17, 301);
            this.progressBar1.MarqueeAnimationSpeed = 500;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(306, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // Updater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(336, 333);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.SkipButton);
            this.Controls.Add(this.RemindButton);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.UpdateAvailable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Updater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Updater";
            this.Load += new System.EventHandler(this.Updater_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UpdateAvailable;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ReleaseNotes;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button RemindButton;
        private System.Windows.Forms.Button SkipButton;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

