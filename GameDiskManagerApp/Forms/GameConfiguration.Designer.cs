
namespace GameDiskManagerApp.Forms
{
    partial class GameConfiguration
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._name = new System.Windows.Forms.TextBox();
            this._location = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.browseButton = new System.Windows.Forms.Button();
            this._size = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Location:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Size:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Launcher:";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 151);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 258);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuration / Save Files";
            // 
            // _name
            // 
            this._name.Location = new System.Drawing.Point(88, 10);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(215, 27);
            this._name.TabIndex = 5;
            // 
            // _location
            // 
            this._location.Location = new System.Drawing.Point(88, 46);
            this._location.Name = "_location";
            this._location.Size = new System.Drawing.Size(184, 27);
            this._location.TabIndex = 6;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(88, 109);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(215, 28);
            this.comboBox1.TabIndex = 7;
            // 
            // browseButton
            // 
            this.browseButton.FlatAppearance.BorderSize = 0;
            this.browseButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.browseButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseButton.Location = new System.Drawing.Point(278, 46);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(25, 27);
            this.browseButton.TabIndex = 8;
            this.browseButton.Text = "📁";
            this.browseButton.UseVisualStyleBackColor = true;
            // 
            // _size
            // 
            this._size.AutoSize = true;
            this._size.Location = new System.Drawing.Point(84, 80);
            this._size.Name = "_size";
            this._size.Size = new System.Drawing.Size(40, 20);
            this._size.TabIndex = 9;
            this._size.Text = "0 GB";
            // 
            // GameConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(315, 419);
            this.Controls.Add(this._size);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this._location);
            this.Controls.Add(this._name);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "GameConfiguration";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Configuration";
            this.Load += new System.EventHandler(this.GameConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox _name;
        private System.Windows.Forms.TextBox _location;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Label _size;
    }
}