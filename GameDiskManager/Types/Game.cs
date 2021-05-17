using GameDiskManager.Utility;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
            this.EZSize = Game.BytesToString(this.Size);
        }
    }
    public class Game
    {
        public string Name { get; set; }
        public string Location { get; set; }
        private long _size { get
            {
                return Size;
            } 
            set
            {
                Size = value;
                EZSize = BytesToString(value);
            } 
        }
        public long Size { get; set; }
        public string EZSize { get; set; }
        public string[] Folders { get; set; }
        public GameFile[] GameFiles { get; set; }

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
                MovingItem item = new MovingItem()
                {
                    source = Location + GameFiles[i].Location,
                    destination = dest + GameFiles[i].Location,
                    size = GameFiles[i].Size,
                    MovingStatus = MovingStatus.Pending
                };
                FastMove.MoveGameItem(ref item);

                GameFiles[i].MovingItem = item;
            }

            Serializer<GameFile[]>
                .WriteToJSONFile(GameFiles
                .Where(x => x.MovingItem.MovingStatus == MovingStatus.Failed)
                .ToArray(), Path.Combine(dest, "failed.json"));

            Location = dest;
            Scan();
        }

        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }
}
