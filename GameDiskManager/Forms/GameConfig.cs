using GameDiskManager.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameDiskManager.Forms
{
    public partial class GameConfig : Form
    {
        public Game _game { private set; get; }
        public GameConfig(Game game)
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
