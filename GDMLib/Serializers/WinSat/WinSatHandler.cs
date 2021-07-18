using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Serializers.WinSat
{
    public static class WinSatHandler
    {
        public static long GetReadSpeed(char driveLetter)
        {
            string fileName = string.Format("{0}_{1}_{2}.{3}", driveLetter, DateTime.Now.ToString("MMddyyyy"), "read", "xml");
            string outPath = string.Format("{0}WinSat\\", FileSystemHandler.DataPath);
            FileSystemHandler.CreateDirectory(outPath);
            string output = ProcessHandler.RunAndOutput(string.Format("winsat disk -drive {0} -read -xml {1}", driveLetter, outPath + fileName));
            //Console.WriteLine(output);

            if (File.Exists(outPath + fileName))
                Console.WriteLine(File.ReadAllText(outPath + fileName));

            return 0;
        }
    }
}
