using GameDiskManager.Types;
using GameDiskManager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager
{
    public class DataStore
    {
        public List<Drive> Drives { get; set; }
        public List<Game> Games { get; set; }
        public List<GameMigration> Migrations { get; set; }
    }
    public static class Data
    {
        public static DataStore Store { get; set; } 
        public static void InitializeDataStore()
        {
            Store = Serializer<DataStore>.LoadFromJSONFile(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GameDiskManager\\DataStore.json"));
        }

        public static void UpdateData(List<Drive> drives)
        {
            Store.Drives.AddRange(Store.Drives.Except<Drive>(drives).ToList());
            SaveDataStore();
        }

        public static void UpdateData(List<Game> games)
        {
            Store.Games.AddRange(Store.Games.Except<Game>(games).ToList());
            SaveDataStore();
        }

        public static void UpdateData(List<GameMigration> migrations)
        {
            Store.Migrations.AddRange(Store.Migrations.Except<GameMigration>(migrations).ToList());
            SaveDataStore();
        }

        public static void SaveDataStore()
        {
            Serializer<DataStore>.WriteToJSONFile(Store,
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GameDiskManager\\DataStore.json"));
        }

    }
}
