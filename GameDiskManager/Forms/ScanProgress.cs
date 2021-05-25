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

namespace GameDiskManager.Forms
{
    public partial class ScanProgress : Form
    {
        private readonly SynchronizationContext synchronizationContext;
        private DateTime previousTime = DateTime.Now;
        public ScanProgress()
        {
            InitializeComponent();
            synchronizationContext = SynchronizationContext.Current;
        }

        public void ProgressMax(int maxProgress)
        {
            progressBar.Maximum = maxProgress;
        }

        public void UpdateProgress(string status, int progress)
        {
            var timeNow = DateTime.Now;
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                Progress p = (Progress)o;
                this.status.Text = p._status;
                this.progressBar.Value = p._progress > progressBar.Maximum ? progressBar.Maximum : p._progress < progressBar.Minimum ? progressBar.Minimum : p._progress;
            }), new Progress(status, progress));
        }

        public int GetProgress()
        {
            return this.progressBar.Value;
        }
    }

    class Progress
    {
        public string _status { get; set; }
        public int _progress { get; set; }
        public Progress(string status, int progress)
        {
            _status = status;
            _progress = progress;
        }
    }
}
