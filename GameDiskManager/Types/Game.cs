using GameDiskManager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace GameDiskManager.Types
{

    public class GameFile
    {
        public string Location { get; set; }
        public long Size { get; set; }
        public string EZSize { get; set; }

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
        public GameFile (string location, long size, string ezSize)
        {
            Location = location;
            this.Size = size;
            this.EZSize = ezSize;
        }
    }
    
    public class ConfigFile
    {
        public string Location { get; set; }
        public bool KeepLocation { get; set; }

    }
    public class Game : IEquatable<Game>
    {
        public int LauncherID { get; set; }
        public int GameID { get; set; }
        public int DriveID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
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
            Name = Regex.Replace(new DirectoryInfo(dir).Name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0").Replace("  ", " ");
            Location = dir;
            GameID = Data.Store.GameIndex++;

            Drive d = Data.Store.Drives.Find(x => this.Location.Contains(x.Name));
            DriveID = d.DriveID;
            Scan();
            this.PercentDiskSpace = (double)this.Size / (double)d.TotalSize;
        }

        [JsonConstructor]
        public Game(int launcherId, int gameId, int driveId, string name, string location)
        {
            LauncherID = launcherId;
            GameID = gameId;
            DriveID = driveId;
            Name = name;
            Location = location;
        }

        public void Scan ()
        {
            Console.WriteLine("Scanning");
            if (Directory.Exists(Location))
            {
                Folders = Utility.DepthSearch.GetDirectories(Location).ToArray();
                string[] files = Directory.GetFiles(Location, "*.*", SearchOption.AllDirectories);

                // Get most likely app name
                IEnumerable<GameFile> appQuery =
                    from app in files
                    where new FileInfo(app).Exists
                    select new GameFile(app);

                GameFiles = appQuery.ToArray();

                //var gameExe = appQuery.Where(i => i.FileInfo.Extension == ".exe" && i.FileInfo.Directory.FullName == this.Location).MaxBy(i => i.Size);

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

                if (Data.Store != null && Data.Store.Drives != null)
                {
                    Drive d = Data.Store.Drives.Find(x => x.DriveID == this.DriveID);
                    this.PercentDiskSpace = (double)this.Size / (double)d.TotalSize;
                }
            }
            else
            {
                Console.WriteLine("Directory doesn't exist");
                throw (new Exception());
            }
        }

        public void Migrate (string dest)
        {
            this.Scan();

            Console.WriteLine("Migrating game: " + this.Name);

            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);

            foreach (string s in Folders)
            {
                string path = dest + s;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            for (int i = 0; i < GameFiles.Length; i++)
            {
                MigrationFile item = new MigrationFile()
                {
                    source = Location + GameFiles[i].Location,
                    destination = dest + GameFiles[i].Location,
                    size = GameFiles[i].Size,
                    Status = MigrationStatus.Pending
                };
                FastMove.MoveGameItem(ref item);

                //GameFiles[i].MovingItem = item;
            }
            /*
            Serializer<GameFile[]>
                .WriteToJSONFile(GameFiles
                .Where(x => x.MovingItem.Status == MigrationStatus.Failed)
                .ToArray(), Path.Combine(dest, "failed.json"));
            */
            Location = dest;
            Scan();
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
}
