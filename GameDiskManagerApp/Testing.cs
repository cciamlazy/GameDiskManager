using Gameloop.Vdf;
using Gameloop.Vdf.JsonConverter;
using Gameloop.Vdf.Linq;
using GDMLib;
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

namespace GameDiskManagerApp
{
    public partial class Testing : Form
    {
        public Testing()
        {
            InitializeComponent();

            Test();
        }

        private void Test()
        {
            string vdf = "C:\\Program Files (x86)\\Steam\\userdata\\34290913\\config\\localconfig.vdf";
            dynamic localConfig = VdfConvert.Deserialize(File.ReadAllText(vdf));
            VObject apps = localConfig.Value.Software.valve.Steam.Apps;

            foreach (var app in apps.ToJson())
            {
                SteamApp newApp = Serializer<SteamApp>.JSONStringToObject(app.Value.ToString());
                newApp.key = int.Parse(app.Key);
                Console.WriteLine("key: {0} | LastPlayed: {1} | Playtime: {2} | Playtime2wks: {3}", newApp.key, newApp.LastPlayed, newApp.Playtime, newApp.Playtime2wks);
            }
        }

    }

    class LocalConfig
    {

    }

    class SteamApp
    {
        public int key { get; set; }
        public int LastPlayed { get; set; }
        public int Playtime { get; set; }
        public int Playtime2wks { get; set; }

    }
}
