using System;
using System.Collections.Generic;
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
