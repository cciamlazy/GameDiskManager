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
        public bool Active { get; set; }
        public List<ConfigFile> ConfigFiles { get; set; }

        public Game (string dir, string name = "")
        {
            this.Name = name;
            if (name == "")
                this.Name = Regex.Replace(new DirectoryInfo(dir).Name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0").Replace("  ", " ");

            this.Location = dir;
            this.GameID = Data.GameIndex++;

            Drive d = Data.DriveByID(Data.GetDriveIDByName(this.Location));
            DriveID = d.DriveID;
            Scan();
            this.PercentDiskSpace = (double)this.Size / (double)d.TotalSize;
        }

        [JsonConstructor]
        public Game(int launcherId, int gameId, int driveId, string name, string location, int priority, DateTime lastplayed, int playtime, bool active, List<ConfigFile> configfiles)
        {
            LauncherID = launcherId;
            GameID = gameId;
            DriveID = driveId;
            Name = name;
            Location = location;
            Priority = priority;
            LastPlayed = lastplayed;
            PlayTime = playtime;
            Active = active;
            ConfigFiles = configfiles;
        }

        public void Scan()
        {
            if (Directory.Exists(Location))
            {
                Folders = DepthSearch.GetDirectories(Location).ToArray();
                string[] files = Directory.GetFiles(Location, "*.*", SearchOption.AllDirectories);

                // Get most likely app name
                IEnumerable<GameFile> appQuery =
                    from app in files
                    where new FileInfo(app).Exists
                    select new GameFile(app);

                GameFiles = appQuery.ToArray();

                /*foreach (GameFile f in )
                {
                    FileInfo info = new FileInfo(f.Location);

                    Console.WriteLine(info.Extension);
                    int rank = 0;
                    if (info.Extension == ".exe") rank += 1;
                    if (info.Name.Replace(".exe", "").Replace(" ", "").ToLower().Contains(info.Directory.Name)) rank += 2;
                    if (info.Name.Replace(".exe", "").Replace(" ", "").ToLower().Equals(info.Directory.Name)) rank += 3;
                    f.exeRank = rank;
                }*/

                


                var gameExe = GameFiles.Where(x => new FileInfo(x.Location).Extension == ".exe")
                    //.Where(x => new FileInfo(x.Location).Directory.FullName == this.Location)
                    .Where(x => !new FileInfo(x.Location).Name.Contains("UnityCrashHandler32.exe"))
                    .MaxBy(x => new FileInfo(x.Location).Length).Take(1);

                if (gameExe != null && gameExe.Count() > 0)
                    this.ExecutableLocation = gameExe.First().Location;

                //this.Name = gameExe.First<GameFile>().FileInfo.Name.Replace(".exe", "");

                //Get all the sizes
                long[] sizes = new long[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo fi = new FileInfo(files[i]);
                    if (fi.Exists)
                    {
                        sizes[i] = fi.Length;
                    }
                }
                _size = sizes.Sum();


                for (int i = 0; i < Folders.Length; i++)
                {
                    Folders[i] = Folders[i].Replace(this.Location, "");
                }
                for (int i = 0; i < GameFiles.Length; i++)
                {
                    GameFiles[i].Location = GameFiles[i].Location.Replace(this.Location, "");
                }

                try
                {
                    this.PercentDiskSpace = (double)this.Size / (double)Data.GetDriveByID(this.DriveID).TotalSize;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine("Drive not found. Can't calculate percent disk space taken");
                }

                Data.UpdateGame(this);
            }
            else
            {
                Console.WriteLine("Directory doesn't exist");
            }
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
