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
using GDMLib;
using GDMLib.Handlers;
using GDMLib.Transitory;

namespace GameDiskManagerApp.Forms
{
    public partial class MigrationProgress : Form
    {
        private readonly SynchronizationContext synchronizationContext;
        GameMigration gameMigration;
        MigrationHandler handler;
        DateTime startTime;
        public MigrationProgress(GameMigration migration)
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
            gameMigration = migration;
            handler = new MigrationHandler(new UpdateMigrationProgressDelegate(this.UpdateProgressDel), this.gameMigration);
            updateProgress.Start();
            migrationWorker.RunWorkerAsync();
        }

        private void AddError(Exception e)
        {
            string err = string.Format("{0}: {1}", (errorList.Items.Count + 1), e.Message);
            errorList.Items.Add(err);
            errors.Text = errorList.Items.Count.ToString();
        }

        private void UpdateMigrationProgress(MigrationProgressState progressState)
        {
            this.gameMigration = this.handler.GameMigration;
            // Set Elapsed Time
            TimeSpan t = TimeSpan.FromMilliseconds((DateTime.Now - startTime).TotalMilliseconds);
            elapsedTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);

            // Set time & speed
            long moved = this.gameMigration.MigrationFiles.Sum(x => x.sent);
            double estSeconds = (this.gameMigration.TotalSize - moved) / ((double)moved / t.TotalSeconds);
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

            totalSize.Text = Utils.BytesToString(this.gameMigration.TotalSize);

            processedAmt.Text = Utils.BytesToString(this.gameMigration.Moved);

            speed.Text = Utils.TransferSpeed(moved, (int)Math.Round(t.TotalSeconds));

            // Progress bar
            progressBar.Maximum = (int)(this.gameMigration.TotalSize / 1000);
            progressBar.Value = (int)(moved / 1000);

            // Progress files
            if (progressState.Files != null)
                files.Text = progressState.Files;
            if (progressState.FileName != null)
                migratingFileName.Text = progressState.FileName;

            if (progressState.State != null)
                status.Text = progressState.State;

            // Errors
            if (progressState.Error != null)
                AddError(progressState.Error);
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (migrationWorker.IsBusy != true)
            {
                // Start the asynchronous operation.
            }
        }

        private void migrationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            startTime = DateTime.Now;

            handler.DoMigration();
        }

        private void UpdateProgressDel(MigrationProgressState progressState)
        {
            migrationWorker.ReportProgress(progressState.State == "Complete" ? 100 : 0, progressState);
        }

        private void migrationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e != null)
                UpdateMigrationProgress(e.UserState as MigrationProgressState);
        }

        private void migrationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // TODO: Add completion code
            Console.WriteLine("Complete");
            progressBar.Value = progressBar.Maximum;
            this.DialogResult = DialogResult.OK;

            if (closeOnComplete.Checked)
                this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (migrationWorker.WorkerSupportsCancellation && migrationWorker.IsBusy)
            {
                migrationWorker.CancelAsync();
            }
        }

        private void backgroundButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            // TODO: Implement status bar icon
        }

        private void updateProgress_Tick(object sender, EventArgs e)
        {
            UpdateMigrationProgress(new MigrationProgressState());
        }
    }
}