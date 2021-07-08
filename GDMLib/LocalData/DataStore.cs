using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using GDMLib.Launchers;

namespace GDMLib
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
        private static DataStore Store { get; set; }

        #region Control Variables
        public static int DriveIndex
        {
            get { return Store.DriveIndex; }
            set
            {
                if (value == Store.DriveIndex + 1)
                    Store.DriveIndex++;
            }
        }

        public static int LauncherIndex
        {
            get { return Store.LauncherIndex; }
            set
            {
                if (value == Store.LauncherIndex + 1)
                    Store.LauncherIndex++;
            }
        }

        public static int GameIndex
        {
            get { return Store.GameIndex; }
            set
            {
                if (value == Store.GameIndex + 1)
                    Store.GameIndex++;
            }
        }

        public static int MigrationIndex
        {
            get { return Store.MigrationIndex; }
            set
            {
                if (value == Store.MigrationIndex + 1)
                    Store.MigrationIndex++;
            }
        }

        public static Drive DriveByID(int DriveID)
        {
            return Store.Drives.Find(d => d.DriveID == DriveID);
        }

        public static Game GameByID(int GameID)
        {
            return Store.Games.Find(d => d.GameID == GameID);
        }

        public static Launcher LauncherByID(int LauncherID)
        {
            return Store.Launchers.Find(d => d.LauncherID == LauncherID);
        }

        public static GameMigration MigrationByID(int MigrationID)
        {
            return Store.Migrations.Find(d => d.MigrationID == MigrationID);
        }

        #endregion
        public static void InitializeDataStore()
        {
            if (File.Exists(FileSystemHandler.CombineDataPath("DataStore.json")))
            {
                Store = Serializer<DataStore>.LoadFromJSONFile(FileSystemHandler.CombineDataPath("DataStore.json"));
                /*foreach (Game g in Store.Games)
                    g.Scan();*/
            }
            if (Store == null)
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
                if (d.IsReady)
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
                    int found = -1;
                    if (Store.Drives != null)
                        found = Store.Drives.FindIndex(x => x.Name == drive.Name && x.TotalSize == drive.TotalSize && x.DriveType == drive.DriveType);
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
                        LauncherID = LauncherIndex++,
                        LauncherType = LauncherType.Steam,
                        Name = "Steam",
                        Location = steamDir.FullName,
                        ExecutableLocation = steamDir.FullName + "\\steam.exe",
                        DriveID = Store.Drives.Find(x => steamDir.FullName.Contains(x.Name)).DriveID
                    });
                }
                else
                {

                }
                // TODO: Implement Icon Loading.. Probably for games too
                /*if (File.Exists() && !File.Exists(SavePath + "\\Resources\\Launch"))
                {
                    Icon.ExtractAssociatedIcon(l.ExecutableLocation)
                }*/
            }
        }

        #region DataStore Interaction

        public static void AddMigration(GameMigration migration)
        {
            if (!(migration.MigrationID >= 0))
                migration.MigrationID = MigrationIndex++;
            Store.Migrations.Add(migration);
        }

        public static int GetDriveIDByName(string name)
        {
            return Store.Drives.Find(x => name.Contains(x.Name)).DriveID;
        }

        #endregion

        #region Get Data

        public static int DriveCount { get { return Store.Drives.Count; } }

        public static Drive GetDriveByIndex (int i)
        {
            if (i < DriveCount)
                return Store.Drives[i];
            return null;
        }

        public static Drive GetDriveByID(int driveId)
        {
            if (Data.Store != null && Data.Store.Drives != null)
            {
                return Data.Store.Drives.Find(x => x.DriveID == driveId);
            }
            throw new DirectoryNotFoundException();
        }

        public static int LauncherCount { get { return Store.Launchers.Count; } }

        public static Launcher GetLauncherByIndex (int i)
        {
            if (i < LauncherCount)
                return Store.Launchers[i];
            return null;
        }

        public static int GameCount { get { return Store.Games.Count; } }
        public static List<Game> GameList { get { return Store.Games; } }

        public static Game GetGameByName(string name)
        {
            return Store.Games.Find(x => x.Name.Replace(" ", "").ToLower() == name.Replace(" ", "").ToLower());
        }

        public static Game GetGameByID(int gameId)
        {
            return Store.Games.Find(x => x.GameID == gameId);
        }

        #endregion

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
            int gameIndex = Store.Games.FindIndex(x => x.GameID == game.GameID || x.Location == game.Location);
            if (gameIndex == -1)
            {
                Store.Games.Add(game);
            }
            else
            {
                Store.Games[gameIndex] = game;
                /*Store.Games[gameIndex].Name = game.Name;
                Store.Games[gameIndex].LauncherID = game.LauncherID;
                Store.Games[gameIndex].Location = game.Location;
                Store.Games[gameIndex].Folders = game.Folders;
                Store.Games[gameIndex].GameFiles = game.GameFiles;
                Store.Games[gameIndex].Priority = game.Priority;
                Store.Games[gameIndex].Active = game.Active;
                Store.Games[gameIndex].ConfigFiles = game.ConfigFiles;*/
            }
            SaveDataStore();
        }

        public static void UpdateData(List<GameMigration> migrations)
        {
            Store.Migrations.AddRange(Store.Migrations.Except<GameMigration>(migrations).ToList());
            SaveDataStore();
        }

        private static void SaveDataStore()
        {
            FileSystemHandler.CreateDirectory(FileSystemHandler.CombineDataPath(""));
            Serializer<DataStore>.WriteToJSONFile(Store,
                FileSystemHandler.CombineDataPath("DataStore.json"));
        }

    }
}
