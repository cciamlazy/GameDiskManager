using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDMLib;
using GDMLib.Transitory;

namespace GameDiskManagerApp.Forms
{
    public partial class ScanProgressForm : Form
    {
        private readonly SynchronizationContext synchronizationContext;
        private DateTime previousTime = DateTime.Now;
        private UpdateViewDelegate updateViewCallback;
        public ScanProgressForm(UpdateViewDelegate callback)
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;

            updateViewCallback = callback;
        }

        public void ScanGames(int LauncherID)
        {
            scanWorker.RunWorkerAsync(LauncherID);
        }

        public int GetProgress()
        {
            return this.progressBar.Value;
        }

        private void scanWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int? l = e.Argument as int?;

            if (l != null)
            {
                int launcherId = l.Value;
                Launcher launcher = Data.LauncherByID(launcherId);
                launcher.ScanGames(new UpdateProgressDelegate(this.UpdateProgress));
            }
        }

        public void UpdateProgress(ScanProgress progress)
        {
            scanWorker.ReportProgress(progress.Progress / progress.MaxProgress, progress);
        }

        private void scanWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e != null)
            {
                ScanProgress progress = e.UserState as ScanProgress;

                this.status.Text = progress.CurrentStatus;
                this.progressBar.Value = progress.Progress;
                this.progressBar.Maximum = progress.MaxProgress;
            }
        }

        private void scanWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            updateViewCallback();
            this.Close();
        }
    }
}
