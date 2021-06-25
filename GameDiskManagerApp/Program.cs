using GDMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

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
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void InitializeSetup()
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
    }
}
