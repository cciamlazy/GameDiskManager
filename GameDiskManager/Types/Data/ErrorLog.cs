using GameDiskManager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types.Data
{
    public class ErrorLog
    {
        public string Name { get; set; } = Environment.UserName;
        public string Date { get; set; }
        public string Time { get; set; }
        public string Version { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
        public Dictionary<string, string> Data { get; set; }

        public static void GenerateLog(string Source, string Message, string StackTrace, string sender = "")
        {
            Dictionary<string, string> data = new Dictionary<string, string>
                {

                };

            ErrorLog log = new ErrorLog()
            {
                Name = Environment.UserName,
                Date = DateTime.Now.ToString("M-d-yyyy"),
                Time = DateTime.Now.ToString("h-mm-ss tt"),
                Version = Program.GetUpdateFile(Path.Combine(Program.DataPath, "CurrentVersion.json")).Version,
                StackTrace = StackTrace,
                Source = Source,
                Message = Message,
                Data = data
            };

            var path = Path.Combine(Program.DataPath + "ErrorLog", log.Date + " " + log.Time + " Error.json");

            Serializer<ErrorLog>.WriteToJSONFile(log, path);
        }
    }
}
