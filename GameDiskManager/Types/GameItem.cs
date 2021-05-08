using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Types
{
    class GameItem
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public long Size { get; set; }
        public string[] folders { get; set; }
        public string[] files { get; set; }

        public GameItem(string name, string dir)
        {
            Name = name;
            Location = dir;
            try
            {
                Scan();
            }
            catch
            {

            }
        }

        private void Scan()
        {
            if (Directory.Exists(Location))
            {
                folders = Utility.DepthSearch.GetDirectories(Location).ToArray();
                files = Directory.GetFiles(Location, "*.*", SearchOption.AllDirectories);

                long[] sizes = new long[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo fi = new FileInfo(files[i]);
                    if (fi.Exists)
                    {
                        sizes[i] = fi.Length;
                    }
                }
                Size = sizes.Sum();

                for (int i = 0; i < folders.Length; i++)
                {
                    folders[i] = folders[i].Replace(this.Location, "");
                }
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = files[i].Replace(this.Location, "");
                }
            }
            else
            {
                throw (new Exception());
            }
        }
    }
}
