using GameDiskManager.Types;
using GameDiskManager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager
{
    public class DataStore
    {
        public List<Drive> Drives { get; set; } = new List<Drive>();
        public List<Game> Games { get; set; } = new List<Game>();
        public List<GameMigration> Migrations { get; set; } = new List<GameMigration>();
        public int DriveIndex { get; set; } = 0;
        public int GameIndex { get; set; } = 0;
        public int MigrationIndex { get; set; } = 0;
    }
    public static class Data
    {
        public static DataStore Store { get; set; }
        private static string SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GameDiskManager\\DataStore.json");
        public static void InitializeDataStore()
        {
            if (File.Exists(SavePath))
            {
                Store = Serializer<DataStore>.LoadFromJSONFile(SavePath);
                foreach (Game g in Store.Games)
                    g.Scan();
            }
            else
            {
                Store = new DataStore
                {
                    Drives = new List<Drive>(),
                    Games = new List<Game>(),
                    Migrations = new List<GameMigration>()
                };
            }

            LoadDrives();
            SaveDataStore();
        }

        private static void LoadDrives()
        {

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                Drive drive = new Drive
                {
                    Name = d.Name,
                    VolumeLabel = d.VolumeLabel,
                    DriveType = d.DriveType,
                    TotalSize = d.TotalSize,
                    TotalFreeSpace = d.TotalFreeSpace,
                    AvailableFreeSpace = d.AvailableFreeSpace,
                    IsReady = d.IsReady,
                    Active = true,
                    Priority = 3,
                };
                int found = Store.Drives.FindIndex(x => x.Name == drive.Name && x.TotalSize == drive.TotalSize && x.DriveType == drive.DriveType);
                if (found >= 0) // Drive exists
                {
                    Store.Drives[found].VolumeLabel = drive.VolumeLabel;
                    Store.Drives[found].TotalFreeSpace = drive.TotalFreeSpace;
                    Store.Drives[found].AvailableFreeSpace = drive.AvailableFreeSpace;
                    Store.Drives[found].IsReady = drive.IsReady;
                }
                else
                {
                    drive.DriveID = Store.DriveIndex++;
                    Store.Drives.Add(drive);
                }
            }
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
            if (!Directory.Exists(Path.GetDirectoryName(SavePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
            Serializer<DataStore>.WriteToJSONFile(Store,
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GameDiskManager\\DataStore.json"));
        }

    }
}
