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
    public partial class MigrationProgress : Form
    {
        GameMigration gameMigration;
        public MigrationProgress(GameMigration migration)
        {
            InitializeComponent();
            gameMigration = migration;
        }

        private void MigrationProgress_Load(object sender, EventArgs e)
        {
            
        }
    }
}
