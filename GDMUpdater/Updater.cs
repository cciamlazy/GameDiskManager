//using IWshRuntimeLibrary;
using GDMLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GDMUpdater
{
    public partial class Updater : Form
    {
        Update _Old;
        Update _Update;
        bool isUpdated = false;
        bool forceUpdate = false;

        public Updater()
        {
            InitializeComponent();
            Program.DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GameDiskManager";


            //Load Current version update file
            string curPath = Program.DataPath + "\\Data\\CurrentVersion.json";
            Update currentVersion;
            if (System.IO.File.Exists(curPath))
                currentVersion = Program.GetUpdateFile(curPath);
            else
                currentVersion = Program.GetUpdateFile(curPath.Replace("json", "xml"));

            string oldVersionFile = Program.DataPath + "\\Data\\OldVersion.xml";
            string currentVersionFile = Program.DataPath + "\\Data\\CurrentVersion.xml";
            string newVersionFile = Program.DataPath + "\\Data\\NewVersion.xml";

            if (System.IO.File.Exists(oldVersionFile))
                System.IO.File.Delete(oldVersionFile);
            if (System.IO.File.Exists(currentVersionFile))
                System.IO.File.Delete(currentVersionFile);
            if (System.IO.File.Exists(newVersionFile))
                System.IO.File.Delete(newVersionFile);

            //Download Update File
            if (!System.IO.File.Exists(Program.DataPath + "\\Data\\NewVersion.json"))
            {
                WebUtil.webClient.DownloadFile(WebUtil.BaseSite + WebUtil.getLatestVersion() + "/Data/CurrentVersion.json", Program.DataPath + "\\Data\\NewVersion.json");
            }
            else
            {
                if (Program.isNewer(Program.GetUpdateFile(Program.DataPath + "\\Data\\NewVersion.json").Version, WebUtil.getLatestVersion()))
                    WebUtil.webClient.DownloadFile(WebUtil.BaseSite + WebUtil.getLatestVersion() + "/Data/CurrentVersion.json", Program.DataPath + "\\Data\\NewVersion.json");
            }
            Update update = Program.GetUpdateFile(Program.DataPath + "\\Data\\NewVersion.json");
            /*if (update.RequiredUpdate)
            {
                Process issueTimeTracker = Process.GetProcessesByName("IssueTimeTracker").FirstOrDefault(p => p.MainModule.FileName.StartsWith(Program.DataPath + "\\IssueTimeTracker.exe"));
                bool isRunning = issueTimeTracker != default(Process);
                if (isRunning)
                    issueTimeTracker.Kill();
            }*/
            if (currentVersion != null && !Program.isNewer(currentVersion.Version, update.Version))
                System.IO.File.Delete(Program.DataPath + "\\Data\\NewVersion.json");

            LoadUpdater(currentVersion, update);
        }

        private void LoadUpdater(Update oldFile, Update update)
        {
            _Old = oldFile;
            _Update = update;

            //Load update data into program
            if (update.RequiredUpdate)
            {
                UpdateAvailable.Text = "A Required Update Is Available";
                RemindButton.Enabled = false;
                SkipButton.Enabled = false;
            }
            else
            {
                UpdateAvailable.Text = "An Optional Update Is Available";
                RemindButton.Enabled = true;
                SkipButton.Enabled = true;
            }
            ReleaseNotes.Lines = update.ReleaseNotes.Split('\n');


            if (!Program.isNewer((oldFile != null ? oldFile.Version : ""), update.Version))
            {
                UpdateAvailable.Text = "No Update is Available";
                RemindButton.Text = "Force Update";
                UpdateButton.Enabled = false;
                RemindButton.Enabled = true;
                SkipButton.Enabled = false;
            }
        }

        private bool EndProcess(bool kill = false)
        {
            bool killed = false;
            Process[] issueTimeTracker = Process.GetProcessesByName("GameDiskManager")/*.FirstOrDefault(p => p.MainModule.FileName.StartsWith(Program.DataPath + "\\IssueTimeTracker.exe"))*/;
            bool[] isRunning = new bool[issueTimeTracker.Length];
            for (int i = 0; i < issueTimeTracker.Length; i++)
            {
                isRunning[i] = issueTimeTracker[i] != default(Process);
                if (isRunning[i])
                {
                    if (!kill)
                        issueTimeTracker[i].CloseMainWindow();
                    else
                        issueTimeTracker[i].Kill();
                    killed = true;
                }
            }
            return killed;
        }

        private void Update(Update oldFile, Update updateFile)
        {
            if (EndProcess())
                Thread.Sleep(500);

            progressBar1.Maximum = updateFile.Files.Count * 2;
            string failedList = "";
            string failedStack = "";
            foreach (DownloadFile file in updateFile.Files)
            {
                string path = Program.DataPath + file.FileName;
                this.Enabled = false;
                string version = "";
                if (oldFile != null)
                    version = FindVersionByFile(oldFile, file.FileName);
                bool download = false;
                if (forceUpdate || version == "" || Program.isNewer(version, file.FileVersion) || !System.IO.File.Exists(path))
                    download = true;
                progressBar1.Value++;
                if (download)
                {
                    FileInfo fi = new FileInfo(path);
                    if (!Directory.Exists(fi.Directory.Parent.FullName))
                        Directory.CreateDirectory(fi.Directory.Parent.FullName);
                    if (!Directory.Exists(fi.Directory.FullName))
                        Directory.CreateDirectory(fi.Directory.FullName);
                    try
                    {
                        if (file.FileName.ToLower().Contains("issuetimetracker.exe") || file.FileName.ToLower().Contains("atlassian.jira.dll") || file.FileName.ToLower().Contains("circularprogressbar.dll"))
                        {
                            EndProcess(true);
                        }
                        WebUtil.webClient.DownloadFile(WebUtil.BaseSite + file.FileVersion + file.DownloadLink, path);
                    }
                    catch (Exception e)
                    {
                        failedList += file.FileName + " - " + e.Message + " - " + e.StackTrace + "\n";
                        failedStack += e.StackTrace + " \n" + e.InnerException + "\n\n\n";
                    }
                }
                progressBar1.Value++;
            }

            if (failedList != "")
            {
                MessageBox.Show("Failed to update\n" + failedList, "Failed Update");
            }
            string oldVersionFile = Program.DataPath + "\\Data\\OldVersion.json";
            string currentVersionFile = Program.DataPath + "\\Data\\CurrentVersion.json";
            string newVersionFile = Program.DataPath + "\\Data\\NewVersion.json";

            if (System.IO.File.Exists(currentVersionFile))
            {
                if (System.IO.File.Exists(oldVersionFile))
                    System.IO.File.Delete(oldVersionFile);
                System.IO.File.Copy(currentVersionFile, Program.DataPath + "\\Data\\OldVersion.json");
            }

            if (System.IO.File.Exists(Program.DataPath + "\\Data\\NewVersion.json"))
            {
                if (System.IO.File.Exists(currentVersionFile))
                    System.IO.File.Delete(currentVersionFile);
                System.IO.File.Copy(Program.DataPath + "\\Data\\NewVersion.json", currentVersionFile);
                System.IO.File.Delete(Program.DataPath + "\\Data\\NewVersion.json");
            }

            if (updateFile.VerifyUpdate != null && updateFile.VerifyUpdate)
            {
                if (System.IO.File.Exists(oldVersionFile))
                    System.IO.File.Delete(oldVersionFile);
                if (System.IO.File.Exists(Program.DataPath + "\\GameDiskManager.exe"))
                    Process.Start(Program.DataPath + "\\GameDiskManager.exe");
                this.Close();
            }
            else
            {
                this.Enabled = true;
                isUpdated = true;
                UpdateButton.Text = "Finish";
                UpdateButton.Enabled = true;
            }
        }

        private string FindVersionByFile(Update update, string fileName)
        {
            string version = "";

            if (update != null && update.Files.Count > 0)
            {
                foreach (DownloadFile file in update.Files)
                {
                    if (file.FileName == fileName)
                    {
                        version = file.FileVersion;
                        break;
                    }
                }
            }

            return version;
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (!isUpdated)
            {
                UpdateButton.Enabled = false;
                RemindButton.Enabled = false;
                SkipButton.Enabled = false;
                Update(_Old, _Update);

                string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
                string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", "Game Disk Manager");
                string shortcutLocation = Path.Combine(appStartMenuPath, "GameDiskManager" + ".lnk");
                if (!System.IO.File.Exists(shortcutLocation))
                {
                    DialogResult dialogResult = MessageBox.Show("Would you like to create a shortcut to your start menu?", "Start Menu Shortcut", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        try
                        {
                            AddShortcut();
                        }
                        catch
                        {
                            MessageBox.Show("Failed to create shortcut. File Location: " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GameDiskManager\\GameDiskManager.exe", "Failed");
                        }
                    }
                }

            }
            else
            {
                if (System.IO.File.Exists(Program.DataPath + "\\GameDiskManager.exe"))
                    Process.Start(Program.DataPath + "\\GameDiskManager.exe");
                this.Close();
            }
        }

        private static void AddShortcut()
        {
            string pathToExe = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GameDiskManager\\GameDiskManager.exe";
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", "Game Disk Manager");

            if (!Directory.Exists(appStartMenuPath))
                Directory.CreateDirectory(appStartMenuPath);

            string shortcutLocation = Path.Combine(appStartMenuPath, "GameDiskManager" + ".lnk");

            if (System.IO.File.Exists(shortcutLocation))
            {
                return;
            }
            /*
            //WshShell shell = new WshShell();
            //IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "GameDiskManager";
            shortcut.IconLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GameDiskManager\\ITT.ico";
            shortcut.TargetPath = pathToExe;
            shortcut.Save();*/
        }

        private void RemindButton_Click(object sender, EventArgs e)
        {
            if (RemindButton.Text == "Force Update")
            {
                UpdateButton.Enabled = false;
                RemindButton.Enabled = false;
                SkipButton.Enabled = false;
                forceUpdate = true;
                Update(_Old, _Update);
            }
            else
            {
                if (System.IO.File.Exists(Program.DataPath + "\\GameDiskManager.exe"))
                    Process.Start(Program.DataPath + "\\GameDiskManager.exe");
                this.Close();
            }
        }

        private void SkipButton_Click(object sender, EventArgs e)
        {
            _Update.Skip = true;



            var path = Program.DataPath + "\\Data\\NewVersion.json";

            Serializer<Update>.WriteToJSONFile(_Update, path);

            this.Close();
        }

        private void Updater_Load(object sender, EventArgs e)
        {

        }
    }
}
