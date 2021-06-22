using GDMLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDMUpdater
{
    static class Program
    {

        public static string DataPath;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => {
                String resourceName = "AssemblyLoadingAndReflection." +
                   new AssemblyName(args.Name).Name + ".dll";
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };*/

            DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\IssueTimeTracker";
            Program.Get45PlusFromRegistry();
            if (!WebUtil.CheckForInternetConnection())
                return;
            if (!Directory.Exists(DataPath + "\\Data"))
            {
                Directory.CreateDirectory(DataPath + "\\Data");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Updater());
        }

        public static Update GetUpdateFile(string path)
        {
            Update update = null;
            if (File.Exists(path))
            {
                if (path.Contains(".xml"))
                    update = Serializer<Update>.LoadFromXMLFile(path);
                else
                    update = Serializer<Update>.LoadFromJSONFile(path);
            }
            else if (File.Exists(path.Replace("xml", "json")))
                update = Serializer<Update>.LoadFromJSONFile(path);
            return update;
        }

        public static bool isNewer(string curVersion, string newVersion)
        {
            if (curVersion == null || curVersion == "")
                return true;
            return new Version(newVersion) > new Version(curVersion);
        }

        private static bool Get45PlusFromRegistry()
        {
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    string version = CheckFor45PlusVersion((int)ndpKey.GetValue("Release"));
                    if (version == "No 4.5 or later version detected" || version == "4.5" || version == "4.5.1")
                    {
                        if (MessageBox.Show("Missing Required Framework", ".NET Framework Version 4.5.2 or later is not detected. Would you like to go to the download page now to get the latest .NET Runtime?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start("https://www.microsoft.com/net/download/windows");
                        }
                        return false;
                    }
                }
                else
                {
                    if (MessageBox.Show("Missing Required Framework", ".NET Framework Version 4.5.2 or later is not detected. Would you like to go to the download page now to get the latest .NET Runtime?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("https://www.microsoft.com/net/download/windows");
                    }
                    return false;
                }
            }
            return true;
        }

        // Checking the version using >= will enable forward compatibility.
        private static string CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 460798)
                return "4.7 or later";
            if (releaseKey >= 394802)
                return "4.6.2";
            if (releaseKey >= 394254)
            {
                return "4.6.1";
            }
            if (releaseKey >= 393295)
            {
                return "4.6";
            }
            if ((releaseKey >= 379893))
            {
                return "4.5.2";
            }
            if ((releaseKey >= 378675))
            {
                return "4.5.1";
            }
            if ((releaseKey >= 378389))
            {
                return "4.5";
            }
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }
    }
}
