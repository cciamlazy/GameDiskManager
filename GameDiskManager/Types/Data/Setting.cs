using GameDiskManager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types.Data
{
    public static class Setting
    {
        public static Settings Value = new Settings();

        public static void Save()
        {
            string path = Program.DataPath + "\\Settings.json";
            Serializer<Settings>.WriteToJSONFile(Setting.Value, path);
        }

        public static void Load()
        {
            string path = Program.DataPath + "\\Settings.json";
            if (!File.Exists(path))
                Create();
            Value = Serializer<Settings>.LoadFromJSONFile(path);
        }

        public static void Create()
        {
            Setting.Value = new Settings();

            Setting.Save();
        }

        public static void Reload()
        {
            Load();
        }

        public static void Update()
        {

            Save();
        }

        public static void Reset()
        {
            Create();
        }
    }
}
