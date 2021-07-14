using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.Serializers.WinSat
{
    public static class WinSatHandler
    {
        public static long GetReadSpeed(char driveLetter)
        {
            string output = ProcessHandler.RunAndOutput(string.Format("winsat disk -drive {0} -read", driveLetter));
            Console.WriteLine(output);

            return 0;
        }
    }
}
