using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Transitory
{
    public class MigrationProgressState
    {
        public string State { get; set; }
        public string Files { get; set; }
        public string FileName { get; set; }
        public Exception Error { get; set; }
    }
}
