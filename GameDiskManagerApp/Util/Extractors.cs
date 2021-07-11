using GDMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameDiskManagerApp.Util
{
    public static class Extractors
    {
        public static Game GetGameFromListViewItem(ListViewItem item)
        {
            return Data.GetGameByID((int)item.Tag);
        }
    }
}
