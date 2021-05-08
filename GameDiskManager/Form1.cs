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

namespace GameDiskManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    dest.Text = fbd.SelectedPath;
                    GameItem game = new GameItem("test", fbd.SelectedPath);

                    Serializer<GameItem>.WriteToJSONFile(game, Path.Combine(fbd.SelectedPath, "config.json"));
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    dest.Text = fbd.SelectedPath;
                    GameItem game = new GameItem("test", fbd.SelectedPath);

                    Serializer<GameItem>.WriteToJSONFile(game, Path.Combine(fbd.SelectedPath, "config.json"));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FastMove.MoveTime(source.Text, dest.Text);
        }
    }
}
