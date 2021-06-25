using GameDiskManager.Forms;
using GameDiskManager.Types.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

namespace GameDiskManager
{
    static class Program
    {
        public static string DataPath;

        public static BugReporter bugReporter;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if !DEBUG
            AdminRelauncher();
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Program.OnThreadException);
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        private static void InitializeLocalData()
        {
            Program.DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GameDiskManager\\Data\\";

            Types.Data.Setting.Load();

            Data.InitializeDataStore();
        }

        private static void AdminRelauncher()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Assembly.GetEntryAssembly().CodeBase;

                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("This program must be run as an administrator! \n\n" + ex.ToString());
                }
            }
        }

        private static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

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
                MessageBox.Show("Fatal error prevented the generation of bug reporter. You are seeing this to prevent an error loop.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
