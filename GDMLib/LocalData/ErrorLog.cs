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
        public static ErrorLog GenerateErrorLog(Exception e)
        {
            return new ErrorLog()
            {
                Name = Environment.UserName,
                Date = DateTime.Now.ToString("M-d-yyyy"),
                Time = DateTime.Now.ToString("h-mm-ss tt"),
                Version = Update.GetUpdateFile(FileSystemHandler.CombineDataPath("CurrentVersion.json")).Version,
                StackTrace = e.StackTrace,
                Source = e.Source,
                Message = e.Message,
                //Data = data
            };
        }

        private static void WriteErrorLog(ErrorLog log)
        {
            Serializer<ErrorLog>.WriteToJSONFile(log, FileSystemHandler.CombineDataPath("ErrorLog\\", log.Date + " " + log.Time + " Error.json"));
        }

        public static void CreateErrorLogFolder()
        {
            FileSystemHandler.CreateDirectory(FileSystemHandler.CombineDataPath("ErrorLog"));
        }
    }
}
