using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace GDMLib
{
    public class Game : IEquatable<Game>
    {
        public int LauncherID { get; set; }
        public int GameID { get; set; }
        public int DriveID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ExecutableLocation { get; set; }
        private long _size { get
            {
                return Size;
            }
            set
            {
                Size = value;
                EZSize = Utils.BytesToString(value);
            }
        }
        public long Size { get; set; }
        public string EZSize { get; set; }
        public double PercentDiskSpace { get; set; }

        [JsonIgnore]
        public string[] Folders { get; set; }

        [JsonIgnore]
        public GameFile[] GameFiles { get; set; }
        public int Priority { get; set; }
        public DateTime LastPlayed { get; set; }
        public int PlayTime { get; set; }
        public int PlayTime2Weeks { get; set; }
        public bool Rankable { get; set; } = true;
        public List<ConfigFile> ConfigFiles { get; set; }

        public Game(string dir, string name = "")
        {
            this.Name = name;
            if (name == "")
                this.Name = Regex.Replace(new DirectoryInfo(dir).Name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0").Replace("  ", " ");

            this.Location = dir;
            this.GameID = Data.GameIndex++;

            this.DriveID = Data.GetDriveIDByName(this.Location);
            Scan();
        }

        [JsonConstructor]
        public Game(int launcherId, int gameId, int driveId, string name, string location, int priority, DateTime lastplayed, int playtime, int playtime2Weeks, bool active, List<ConfigFile> configfiles)
        {
            LauncherID = launcherId;
            GameID = gameId;
            DriveID = driveId;
            Name = name;
            Location = location;
            Priority = priority;
            LastPlayed = lastplayed;
            PlayTime = playtime;
            PlayTime2Weeks = playtime2Weeks;
            Active = active;
            ConfigFiles = configfiles;
        }

        public void Scan()
        {
            if (Directory.Exists(Location))
            {
                GatherFolders();

                GatherFiles();

                this.ExecutableLocation = GetLikelyGameExeLocation(this.GameFiles);

                this._size = CalculateTotalFileSize();

                RemoveFolderFilePath();

                SetPercentDiskSpace();

                Data.UpdateGame(this);
            }
            else
            {
                Console.WriteLine("Directory doesn't exist");
            }
        }

        private void GatherFolders()
        {
            this.Folders = DepthSearch.GetDirectories(Location).ToArray();
        }

        private string[] GatherFiles()
        {
            string[] files = Directory.GetFiles(this.Location, "*.*", SearchOption.AllDirectories);

            IEnumerable<GameFile> appQuery =
                    from app in files
                    where new FileInfo(app).Exists
                    select new GameFile(app);

            GameFiles = appQuery.ToArray();

            return files;
        }

        private string GetLikelyGameExeLocation(GameFile[] files)
        {
            var gameExe = files.Where(x => ConsiderGameExe(new FileInfo(x.Location)))
                .MaxBy(x => new FileInfo(x.Location).Length).Take(1).First();

            if (gameExe != null)
                return gameExe.Location;

            return "";
        }

        string[] InvalidExeNames = { "UnityCrashHandler32.exe" };
        private bool ConsiderGameExe(FileInfo file)
        {
            bool flag = false;

            if (file.Extension == ".exe")
                flag = true;
            else
                return false;

            foreach (string s in InvalidExeNames)
            {
                if (file.Name.Contains(s))
                    return false;
            }

            return flag;
        }

        private void SetPercentDiskSpace()
        {
            try
            {
                this.PercentDiskSpace = (double)this.Size / (double)Data.GetDriveByID(this.DriveID).TotalSize;
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Drive not found. Can't calculate percent disk space taken");
            }
        }

        private void RemoveFolderFilePath()
        {
            for (int i = 0; i < Folders.Length; i++)
            {
                Folders[i] = Folders[i].Replace(this.Location, "");
            }
            for (int i = 0; i < GameFiles.Length; i++)
            {
                GameFiles[i].Location = GameFiles[i].Location.Replace(this.Location, "");
            }
        }

        private long CalculateTotalFileSize()
        {
            return this.GameFiles.Sum(x => x.Size);
        }

        public virtual GameMigration GenerateMigration (int toDriveId)
        {
            return this.GenerateMigration(this.Location.Replace(Data.DriveByID(this.DriveID).Name, Data.DriveByID(toDriveId).Name), DateTime.Now);
        }

        public virtual GameMigration GenerateMigration (string dest, DateTime plannedDT)
        {
            this.Scan();

            GameMigration migration = new GameMigration
            {
                MigrationID = Data.MigrationIndex++,
                GameID = this.GameID,
                From_DriveID = this.DriveID,
                To_DriveID = Drive.GetDriveID(dest),
                Status = MigrationStatus.Pending,
                PlannedDateTime = plannedDT,
                DestinationRoot = dest,
                TotalSize = this.Size
            };

            Console.WriteLine("Migrating game: {0} from {1} to {2}", this.Name, this.Location, dest);

            migration.MigrationFiles = new MigrationFile[GameFiles.Length];

            for (int i = 0; i < GameFiles.Length; i++)
            {
                MigrationFile item = new MigrationFile()
                {
                    source = Location + GameFiles[i].Location,
                    destination = dest + GameFiles[i].Location,
                    size = GameFiles[i].Size,
                    Status = MigrationStatus.Pending
                };

                migration.MigrationFiles[i] = item;
            }

            //await migration.MigrateGame();

            /*
            Serializer<GameFile[]>
                .WriteToJSONFile(GameFiles
                .Where(x => x.MovingItem.Status == MigrationStatus.Failed)
                .ToArray(), Path.Combine(dest, "failed.json"));
            */
            Location = dest;
            Scan();

            return migration;
        }

        public bool Equals(Game other)
        {
            if (other is null)
                return false;

            return this.GameID == other.GameID && this.Location == other.Location;
        }

        public override bool Equals(object obj) => Equals(obj as Game);
        public override int GetHashCode() => (GameID, Location).GetHashCode();
    }

    public class GameFile
    {
        public string Location { get; set; }
        public long Size { get; set; }
        public string EZSize { get; set; }
        public int exeRank { get; set; }

        public GameFile(string location)
        {
            Location = location;
            if (location != null)
            {
                var FileInfo = new FileInfo(location);
                this.Size = FileInfo.Length;
                this.EZSize = Utils.BytesToString(this.Size);
            }
        }

        [JsonConstructor]
        public GameFile(string location, long size, string ezSize)
        {
            Location = location;
            this.Size = size;
            this.EZSize = ezSize;
        }
    }

    public class ConfigFile
    {
        public ConfigIdentifier Identifier { get; set; }
        public string Location { get; set; }
        public bool KeepLocation { get; set; }
        public string RelativeLocation { get; set; }
        public bool KeepRelative { get; set; }

    }

    public enum ConfigIdentifier
    {
        Config,
        Save,
        Manifest
    }
}
