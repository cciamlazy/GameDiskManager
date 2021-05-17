﻿using GameDiskManager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace GameDiskManager.Types
{

    public class GameFile
    {
        public string Location { get; set; }
        public long Size { get; set; }
        public string EZSize { get; set; }
        public FileInfo FileInfo { get; set; }
        public MovingItem MovingItem { get; set; }

        public GameFile (string dir)
        {
            Location = dir;
            this.FileInfo = new FileInfo(dir);
            this.Size = this.FileInfo.Length;
            this.EZSize = Utils.BytesToString(this.Size);
        }
    }
    
    public class ConfigFile
    {

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
        public float PercentDiskSpace { get; set; }
        public string[] Folders { get; set; }
        public GameFile[] GameFiles { get; set; }
        public int Priority { get; set; }
        public DateTime LastPlayed { get; set; }
        public int PlayTime { get; set; }
        public bool Active { get; set; }
        public ConfigFile[] ConfigFiles { get; set; }

        public Game (string dir, string name = "")
        {
            Name = name;
            Location = dir;
            Scan();
        }

        private void Scan ()
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

                var gameExe = appQuery.Where(i => i.FileInfo.Extension == ".exe" && i.FileInfo.Directory.FullName == this.Location).MaxBy(i => i.Size);

                this.Name = gameExe.First<GameFile>().FileInfo.Name.Replace(".exe", "");

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

                GameFiles[i].MovingItem = item;
            }

            Serializer<GameFile[]>
                .WriteToJSONFile(GameFiles
                .Where(x => x.MovingItem.MovingStatus == MigrationStatus.Failed)
                .ToArray(), Path.Combine(dest, "failed.json"));

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
