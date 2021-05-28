using GameDiskManager.Utility;
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
        public int GameID { get; set; }
        public int Time_ms { get; set; }
        public int EstTime_ms { get; set; }
        public DateTime PlannedDateTime { get; set; }
        public DateTime ActualDateTime { get; set; }
        public MigrationStatus Status { get; set; }
        public MigrationFile[] MigrationFiles { get; set; }

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

            if (DestinationRoot == "")
            {
                DestinationRoot = Data.GameByID(this.GameID).Location.Replace(Data.DriveByID(this.From_DriveID).Name, Data.DriveByID(this.To_DriveID).Name);
            }

            DateTime startTime = DateTime.Now;
            this.ActualDateTime = startTime;

            if (!Directory.Exists(DestinationRoot))
                Directory.CreateDirectory(DestinationRoot);

            foreach (string s in Data.GameByID(this.GameID).Folders)
            {
                string path = DestinationRoot + s;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            for (int i = 0; i < this.MigrationFiles.Length; i++)
            {
                FastMove.MoveGameFile(ref this.MigrationFiles[i]);
            }

            // TODO: Implement Configuration migration

            this.Time_ms = DateTime.Now.Millisecond - startTime.Millisecond;


            MigrationFile[] failed = this.MigrationFiles
                .Where(x => x.Status == MigrationStatus.Failed)
                .ToArray();

            game.DriveID = this.To_DriveID;
            game.ExecutableLocation.Replace(game.Location, this.DestinationRoot);
            game.Location = this.DestinationRoot;

            Data.UpdateGame(game);

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
