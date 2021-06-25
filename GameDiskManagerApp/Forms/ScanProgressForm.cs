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
using GDMLib.TransitoryData;

namespace GameDiskManagerApp.Forms
{
    public partial class ScanProgressForm : Form
    {
        private readonly SynchronizationContext synchronizationContext;
        private DateTime previousTime = DateTime.Now;
        public ScanProgressForm()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
        }

        public void ScanGames(int LauncherID)
        {
            scanWorker.RunWorkerAsync(LauncherID);
        }

        /*public void UpdateProgress(string status, int progress)
        {
            var timeNow = DateTime.Now;
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                Progress p = (Progress)o;
                this.status.Text = p._status;
                this.progressBar.Value = p._progress > progressBar.Maximum ? progressBar.Maximum : p._progress < progressBar.Minimum ? progressBar.Minimum : p._progress;
            }), new Progress(status, progress));
        }*/

        public int GetProgress()
        {
            return this.progressBar.Value;
        }

        private void scanWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int? launcherId = e.Argument as int?;
            var del = new UpdateProgressDelegate(ScanProgressForm.UpdateProgress);
            if (launcherId != null)
                Data.Store.Launchers[Data.Store.Launchers.FindIndex(x => x.LauncherID == launcherId)].ScanGames(del);
        }

        public void UpdateProgress(ScanProgress progress)
        {
            this.status.Text = progress.CurrentStatus;
            this.progressBar.Value = progress.Progress;
            this.progressBar.Maximum = progress.MaxProgress;
        }
    }
}
