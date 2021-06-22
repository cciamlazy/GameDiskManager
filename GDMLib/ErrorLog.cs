using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib 
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
        private static ErrorLog GenerateErrorLog(Exception e)
        {
            return new ErrorLog()
            {
                Name = Environment.UserName,
                Date = DateTime.Now.ToString("M-d-yyyy"),
                Time = DateTime.Now.ToString("h-mm-ss tt"),
                Version = Update.GetUpdateFile(Path.Combine(DataPath, "CurrentVersion.json")).Version,
                StackTrace = e.StackTrace,
                Source = e.Source,
                Message = e.Message,
                //Data = data
            };
        }

        private static void WriteErrorLog(ErrorLog log)
        {
            var path = Path.Combine(Program.DataPath + "ErrorLog", log.Date + " " + log.Time + " Error.json");

            Serializer<ErrorLog>.WriteToJSONFile(log, path);
        }
    }
}
