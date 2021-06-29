using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Transitory
{
    public class ScanProgress
    {
        public int MaxProgress { get; set; }
        public int Progress { get; set; }
        public string CurrentStatus { get; set; }

        public void UpdateProgress(string status, int progress)
        {
            this.Progress = progress;
            this.CurrentStatus = status;
        }
    }
}
