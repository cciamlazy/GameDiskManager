using GDMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Principal;
using System.Reflection;
using GameDiskManagerApp.Forms;

namespace GameDiskManagerApp
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary> 
        [STAThread]
        static void Main()
        {
            InitializeLocalData();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameManager());
        }

        private static void InitializeLocalData()
        {
            FileSystemHandler.Initialize();
#if !DEBUG
            AdminRelauncher();
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Program.OnThreadException);
#endif

            Setting.Load();

            Data.InitializeDataStore();
        }


        internal static void OnThreadException(object sender, System.Threading.ThreadExceptionEventArgs t)
        {
            Exception e = t.Exception;
            StackTrace st = new StackTrace(e);
            try
            {
                ErrorLog.CreateErrorLogFolder();

                ErrorLog log = ErrorLog.GenerateErrorLog(e);
                //BugReporter bugReporter = new BugReporter(log);
            }
            catch
            {
                MessageBox.Show("Fatal error prevented the generation of bug reporter. You are seeing this to prevent an error loop.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
}
