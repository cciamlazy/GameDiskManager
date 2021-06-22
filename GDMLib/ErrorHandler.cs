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
                bugReporter = new BugReporter(log);
            }
            catch
            {
                //it done messed up
            }
        }

        private static void CreateErrorLogFolder()
        {
            if (!Directory.Exists(Path.Combine(Program.DataPath, "ErrorLog")))
                Directory.CreateDirectory(Path.Combine(Program.DataPath, "ErrorLog"));
        }

        private static ErrorLog GenerateErrorLog(Exception e)
        {
            return new ErrorLog()
            {
                Name = Environment.UserName,
                Date = DateTime.Now.ToString("M-d-yyyy"),
                Time = DateTime.Now.ToString("h-mm-ss tt"),
                Version = GetUpdateFile(Path.Combine(DataPath, "CurrentVersion.json")).Version,
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
