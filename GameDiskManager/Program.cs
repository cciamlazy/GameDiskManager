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

        private static void CreateErrorLogFolder()
        {
            if (!Directory.Exists(Path.Combine(Program.DataPath, "ErrorLog")))
                Directory.CreateDirectory(Path.Combine(Program.DataPath, "ErrorLog"));
        }
    }
}
