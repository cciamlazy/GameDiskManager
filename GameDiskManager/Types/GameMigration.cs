using GameDiskManager.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types
{
    public class GameMigration : IEquatable<GameMigration>
    {
        public int MigrationID { get; set; }
        public int From_DriveID { get; set; }
        public int To_DriveID { get; set; }
        public string DestinationRoot { get; set; }
        public string SourceRoot { get; set; }
        public int GameID { get; set; }
        public int Time_ms { get; set; }
        public int EstTime_ms { get; set; }
        public DateTime PlannedDateTime { get; set; }
        public DateTime ActualDateTime { get; set; }
        public MigrationStatus Status { get; set; }
        [JsonIgnore]
        public MigrationFile[] MigrationFiles { get; set; }
        public MigrationFile[] Failed { get; set; }

        public bool Equals(GameMigration other)
        {
            if (other is null)
                return false;

            return this.MigrationID == other.MigrationID && this.GameID == other.GameID;
        }

        public override bool Equals(object obj) => Equals(obj as GameMigration);
        public override int GetHashCode() => (MigrationID, GameID).GetHashCode();

        public async Task<GameMigration> MigrateGame()
        {
            int delay = this.PlannedDateTime.Millisecond - DateTime.Now.Millisecond;
            if (delay > 0)
            {
                Console.WriteLine("Delaying migration {0} seconds", delay / 1000);
                await Task.Delay(delay);
            }

            Game game = Data.GameByID(this.GameID);

            SourceRoot = game.Location;

            if (DestinationRoot == "")
            {
                DestinationRoot = Data.GameByID(this.GameID).Location.Replace(Data.DriveByID(this.From_DriveID).Name, Data.DriveByID(this.To_DriveID).Name);
            }

            DateTime startTime = DateTime.Now;
            this.ActualDateTime = startTime;

            if (!Directory.Exists(DestinationRoot))
                Directory.CreateDirectory(DestinationRoot);

            Console.WriteLine("Creating Directories");

            foreach (string s in Data.GameByID(this.GameID).Folders)
            {
                string path = DestinationRoot + s;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            for (int i = 0; i < this.MigrationFiles.Length; i++)
            {
                Console.Write("\rMigrating Game Files: {0}/{1}", (i + 1).ToString(), this.MigrationFiles.Length);
                FastMove.MoveGameFile(ref this.MigrationFiles[i]);
            }
            /*
            Console.WriteLine("Migrating Configs");

            ConfigFile[] configs = game.ConfigFiles.Where(x => !x.KeepLocation).ToArray();
            for (int i = 0; i < configs.Count(); i++)
            {
                var relativePath = Utils.GetRelativePath(Path.GetDirectoryName(game.Location), configs[i].Location);
                var newPath = Path.Combine(this.DestinationRoot, relativePath);
                FastMove.FMove(configs[i].Location, newPath);
            }*/

            // TODO: Implement Configuration migration

            this.Time_ms = DateTime.Now.Millisecond - startTime.Millisecond;

            Console.WriteLine("Done");

            this.Failed = this.MigrationFiles
                .Where(x => x.Status == MigrationStatus.Failed)
                .ToArray();

            if (this.Failed.Where(x => x.Status == MigrationStatus.Failed && x.source.Contains(".exe")).Count() > 0)
                this.Status = MigrationStatus.Failed;

            game.DriveID = this.To_DriveID;
            game.ExecutableLocation = (game.ExecutableLocation == null ? "" : game.ExecutableLocation.Replace(game.Location, this.DestinationRoot));
            game.Location = this.DestinationRoot;

            Console.WriteLine(game.Location);


            this.Status = this.Status != MigrationStatus.Failed ? MigrationStatus.Successful : MigrationStatus.Failed;

            Data.UpdateGame(game);

            // size time in milliseconds per hour
            long tsize = game.Size * 3600000 / Time_ms;
            tsize = tsize / (int)Math.Pow(2, 30);

            TimeSpan t = TimeSpan.FromMilliseconds(Time_ms);
            string time = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

            Console.WriteLine(tsize + "GB/hour");

            Console.WriteLine("Migration Complete in {0} and migrated at an average speed of {1}", tsize, time );

            Data.Store.Migrations.Add(this);

            Data.SaveDataStore();

            return this;
        }
    }

    public enum MigrationStatus
    {
        Pending,
        Migrating,
        Successful,
        Failed,
        Reversed
    }
    public class MigrationFile
    {
        public string source { get; set; }
        public string destination { get; set; }
        public long size { get; set; }
        public int Time_ms { get; set; }
        public MigrationStatus Status { get; set; }
    }
}
