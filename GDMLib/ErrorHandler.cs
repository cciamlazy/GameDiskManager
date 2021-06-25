using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib
{
    class ErrorHandler
    {



        internal static void OnThreadException(object sender, System.Threading.ThreadExceptionEventArgs t)
        {
            Exception e = t.Exception;
            StackTrace st = new StackTrace(e);
            try
            {
                CreateErrorLogFolder();

                ErrorLog log = GenerateErrorLog(e);
                //bugReporter = new BugReporter(log);
            }
            catch
            {
                //it done messed up
            }
        }

        private static void CreateErrorLogFolder()
        {
            FileSystemHandler.CreateDirectory(FileSystemHandler.CombineDataPath("ErrorLog"));
        }

        private static ErrorLog GenerateErrorLog(Exception e)
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
            var path = FileSystemHandler.CombineDataPath("ErrorLog\\", log.Date + " " + log.Time + " Error.json");

            Serializer<ErrorLog>.WriteToJSONFile(log, path);
        }
    }
}
