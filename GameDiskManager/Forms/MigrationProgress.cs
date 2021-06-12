using GameDiskManager.Types;
using GameDiskManager.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameDiskManager.Forms
{
    public partial class MigrationProgress : Form
    {
        private readonly SynchronizationContext synchronizationContext;
        GameMigration gameMigration;
        DateTime startTime;
        public MigrationProgress(GameMigration migration)
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
            gameMigration = migration;
        }

        private void AddError(Exception e)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                string err = string.Format("{0}: {1}", (errorList.Items.Count + 1), e.Message);
                errorList.Items.Add(err);
                errors.Text = errorList.Items.Count.ToString();
            }), true);
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            //UpdateMigrationProgress();
        }

        private void UpdateMigrationProgress(int progress, MigrationProgressState progressState)
        {
            // Set Elapsed Time
            TimeSpan t = TimeSpan.FromMilliseconds((DateTime.Now - startTime).TotalMilliseconds);
            elapsedTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);

            // Set time & speed
            long moved = this.gameMigration.MigrationFiles.Sum(x => x.sent);
            double estSeconds = (gameMigration.TotalSize - moved) / ((double)moved / t.TotalSeconds);
            if (estSeconds <= TimeSpan.MaxValue.TotalSeconds)
            {
                TimeSpan es =
                    TimeSpan.FromSeconds(estSeconds);

                estimatedTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                            es.Hours,
                            es.Minutes,
                            es.Seconds);
            }
            else
            {
                estimatedTime.Text = "Estimating...";
            }

            processedAmt.Text = Utils.BytesToString(this.gameMigration.Moved);

            speed.Text = Utils.TransferSpeed(moved, (int)Math.Round(t.TotalSeconds));

            // Progress bar
            progressBar.Maximum = (int)(this.gameMigration.TotalSize / 1000);
            progressBar.Value = (int)(moved / 1000);

            // Progress files
            files.Text = progressState.Files;
            migratingFileName.Text = progressState.FileName;

            // Errors
            if (progressState.Error != null)
                AddError(progressState.Error);
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (migrationWorker.IsBusy != true)
            {
                // Start the asynchronous operation.
                migrationWorker.RunWorkerAsync();
            }
        }

        private void migrationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            startTime = DateTime.Now;

            Game game = Data.GameByID(this.gameMigration.GameID);

            this.gameMigration.TotalSize = game.Size;

            this.gameMigration.SourceRoot = game.Location;

            if (this.gameMigration.DestinationRoot == "")
            {
                this.gameMigration.DestinationRoot = Data.GameByID(this.gameMigration.GameID).Location.Replace(Data.DriveByID(this.gameMigration.From_DriveID).Name,
                    Data.DriveByID(this.gameMigration.To_DriveID).Name);
            }

            this.gameMigration.ActualDateTime = startTime;

            if (!Directory.Exists(this.gameMigration.DestinationRoot))
                Directory.CreateDirectory(this.gameMigration.DestinationRoot);

            foreach (string s in Data.GameByID(this.gameMigration.GameID).Folders)
            {
                string path = this.gameMigration.DestinationRoot + s;
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }

            for (int i = 0; i < this.gameMigration.MigrationFiles.Length; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    if (i == this.gameMigration.MigrationFiles.Count())
                        break;

                    MigrationProgressState progressState = new MigrationProgressState
                    {
                        State = "Migrating Game Files...",
                        Files = string.Format("{0}/{1}", (i + 1).ToString(), this.gameMigration.MigrationFiles.Length),
                        FileName = this.gameMigration.MigrationFiles[i].source.Replace(game.Location, "")
                    };

                    worker.ReportProgress(i, progressState);

                    FastMove.MoveGameFile(ref this.gameMigration.MigrationFiles[i]);

                    if (this.gameMigration.MigrationFiles[i].Status == MigrationStatus.Failed)
                    {
                        progressState.Error = this.gameMigration.MigrationFiles[i].Exception;
                        worker.ReportProgress(i, progressState);
                    }
                    this.gameMigration.Moved += this.gameMigration.MigrationFiles[i].sent;
                }
            }

            ConfigFile[] configs = game.ConfigFiles.Where(x => !x.KeepLocation).ToArray();
            for (int i = 0; i < configs.Count(); i++)
            {
                if (!configs[i].KeepLocation)
                {
                    MigrationProgressState progressState = new MigrationProgressState
                    {
                        State = "Migration Configs...",
                        Files = string.Format("{0}/{1}", (i + 1).ToString(), configs.Count()),
                        FileName = configs[i].Location
                    };

                    string newPath = "";

                    if (configs[i].KeepRelative)
                    {
                        string relativePath = Utils.GetRelativePath(Path.GetDirectoryName(game.Location), configs[i].Location);
                        newPath = Path.Combine(this.gameMigration.DestinationRoot, relativePath);
                    }
                    MigrationFile configFile = new MigrationFile
                    {
                        source = configs[i].Location,
                        destination = newPath
                    };
                    FastMove.FMove(ref configFile);

                    if (configFile.Status == MigrationStatus.Failed)
                    {
                        progressState.Error = configFile.Exception;
                    }
                    worker.ReportProgress(i, progressState);
                }
            }

            // TODO: Implement Configuration migration

            this.gameMigration.Time_ms = (int)Math.Round((DateTime.Now - startTime).TotalMilliseconds);

            Console.WriteLine("Done");

            this.gameMigration.Failed = this.gameMigration.MigrationFiles
                .Where(x => x.Status == MigrationStatus.Failed)
                .ToArray();

            if (this.gameMigration.Failed.Where(x => x.Status == MigrationStatus.Failed && x.source.Contains(".exe")).Count() > 0)
                this.gameMigration.Status = MigrationStatus.Failed;

            game.DriveID = this.gameMigration.To_DriveID;
            game.ExecutableLocation = (game.ExecutableLocation == null ? "" : game.ExecutableLocation.Replace(game.Location, this.gameMigration.DestinationRoot));
            game.Location = this.gameMigration.DestinationRoot;

            //Console.WriteLine(game.Location);


            this.gameMigration.Status = this.gameMigration.Status != MigrationStatus.Failed ? MigrationStatus.Successful : MigrationStatus.Failed;

            Data.UpdateGame(game);

            // size time in milliseconds per hour
            long tsize = game.Size * 3600000 / this.gameMigration.Time_ms;
            tsize = tsize / (int)Math.Pow(2, 30);

            TimeSpan t = TimeSpan.FromMilliseconds(this.gameMigration.Time_ms);
            string time = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

            Console.WriteLine(tsize + "GB/hour");

            Console.WriteLine("Migration Complete in {0} and migrated at an average speed of {1}", tsize, time);

            Data.Store.Migrations.Add(this.gameMigration);

            Data.SaveDataStore();
        }

        private void migrationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e != null)
                UpdateMigrationProgress(e.ProgressPercentage, e.UserState as MigrationProgressState);
        }

        private void migrationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Add completion Code
            Console.WriteLine("Complete");
            progressBar.Value = progressBar.Maximum;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (migrationWorker.WorkerSupportsCancellation && migrationWorker.IsBusy)
            {
                migrationWorker.CancelAsync();
            }
        }
    }

    class MigrationProgressState
    {
        public string State { get; set; }
        public string Files { get; set; }
        public string FileName { get; set; }
        public Exception Error { get; set; }
    }
}
