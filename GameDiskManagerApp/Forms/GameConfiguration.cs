using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDMLib;

namespace GameDiskManagerApp.Forms
{
    public partial class GameConfiguration : Form
    {
        public Game _game { private set; get; }
        public GameConfiguration(Game game)
        {
            InitializeComponent();

            _game = game;
        }

        private void GameConfig_Load(object sender, EventArgs e)
        {
            _name.Text = _game.Name;
            _location.Text = _game.Location;
            _size.Text = _game.EZSize;
        }
    }
}
