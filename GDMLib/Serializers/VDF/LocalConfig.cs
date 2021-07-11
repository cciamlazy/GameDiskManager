using Gameloop.Vdf;
using Gameloop.Vdf.JsonConverter;
using Gameloop.Vdf.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Serializers.VDF
{
    public class LocalConfig
    {
        public List<SteamApp> SteamApps { get; private set; }
        public LocalConfig(string location)
        {
            if (!File.Exists(location)) return;

            SteamApps = new List<SteamApp>();

            dynamic localConfig = VdfConvert.Deserialize(File.ReadAllText(location));
            try
            {
                VObject apps = localConfig.Value.Software.valve.Steam.Apps;

                foreach (var app in apps.ToJson())
                {
                    string s = app.Value.ToString();
                    SteamApp newApp = Serializer<SteamApp>.JSONStringToObject(s);
                    newApp.AppId = int.Parse(app.Key);
                    SteamApps.Add(newApp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public class SteamApp
    {
        public int AppId { get; set; }
        public int LastPlayed { get; set; }
        public int Playtime { get; set; }
        public int Playtime2wks { get; set; }
    }
}
