using Gameloop.Vdf.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib.VDF
{
    public static class VDFDataDiscovery
    {
        public static List<string> NumberKeyIteratory(VProperty vProperty)
        {
            List<string> values = new List<string>();

            int i = 1;
            bool flag = false;
            while (!flag)
            {
                string key = i++.ToString();
                if (vProperty.Value[key] != null)
                {
                    values.Add(vProperty.Value[key].ToString());
                }
                else
                    flag = true;
            }

            return values;
        }

    }
}
