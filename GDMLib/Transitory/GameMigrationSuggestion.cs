using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Transitory
{
    public enum Confidence
    {
        High,
        Medium,
        Low
    }
    public class GameMigrationSuggestion
    {
        public int GameID { get; set; }
        public int DriveID { get; set; }
        public int MoveToDriveID { get; set; }
        public Confidence ConfidenceLevel { get; set; }
    }
}
