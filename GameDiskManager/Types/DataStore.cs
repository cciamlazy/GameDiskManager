using GameDiskManager.Types;
using GameDiskManager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using GameDiskManager.Types.Launchers;

namespace GameDiskManager
{
    public class DataStore
    {
        public List<Drive> Drives { get; set; } = new List<Drive>();
        public List<Launcher> Launchers { get; set; } = new List<Launcher>();
        public List<Game> Games { get; set; } = new List<Game>();
        public List<GameMigration> Migrations { get; set; } = new List<GameMigration>();
        public int DriveIndex { get; set; } = 0;
        public int LauncherIndex { get; set; } = 0;
        public int GameIndex { get; set; } = 0;
        public int MigrationIndex { get; set; } = 0;
    }
    public static class Data
    {
        public static DataStore Store { get; set; }
        public static string SavePath { get; private set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GameDiskManager\\");
        public static void InitializeDataStore()
        {
            if (File.Exists(SavePath + "DataStore.json"))
            {
                Store = Serializer<DataStore>.LoadFromJSONFile(SavePath + "DataStore.json");
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
            LocateLaunchers();
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

        private static void LocateLaunchers()
        {
            var mainSteamDir = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath", "") ?? Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", "");

            DirectoryInfo steamDir = new DirectoryInfo(mainSteamDir.ToString());

            if (steamDir.Exists)
            {
                //Console.WriteLine(Serializer<Launcher>.ObjectToJSONString(Store.Launchers.Find(x => x.LauncherType == LauncherType.Steam)));
                if (Store.Launchers.Find(x => x.LauncherType == LauncherType.Steam) == null)
                {
                    Store.Launchers.Add(new Steam
                    {
                        LauncherID = Store.LauncherIndex++,
                        LauncherType = LauncherType.Steam,
                        Name = "Steam",
                        Location = steamDir.FullName,
                        DriveID = Store.Drives.Find(x => steamDir.FullName.Contains(x.Name)).DriveID
                    });
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

        public static void UpdateGame(Game game)
        {
            int gameIndex = Store.Games.FindIndex(x => x.GameID == game.GameID);
            Store.Games[gameIndex].Name = game.Name;
            Store.Games[gameIndex].LauncherID = game.LauncherID;
            Store.Games[gameIndex].Location = game.Location;
            Store.Games[gameIndex].Folders = game.Folders;
            Store.Games[gameIndex].GameFiles = game.GameFiles;
            Store.Games[gameIndex].Priority = game.Priority;
            Store.Games[gameIndex].Active = game.Active;
            Store.Games[gameIndex].ConfigFiles = game.ConfigFiles;
            SaveDataStore();
        }

        public static void UpdateData(List<GameMigration> migrations)
        {
            Store.Migrations.AddRange(Store.Migrations.Except<GameMigration>(migrations).ToList());
            SaveDataStore();
        }

        public static void SaveDataStore()
        {
            if (!Directory.Exists(Path.GetDirectoryName(SavePath + "DataStore.json")))
                Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
            Serializer<DataStore>.WriteToJSONFile(Store,
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GameDiskManager\\DataStore.json"));
        }

    }
}
