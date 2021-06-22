using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib
{
    public class WebUtil
    {

        public static string BaseSite = "http://csmithut.net/IssueTimeTracker/";
        public static WebClient webClient = new WebClient();

        /// <summary>
        /// Check if there is an internet connection
        /// </summary>
        /// <returns>True if there is a connection, false if not</returns>
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static string getLatestVersion()
        {
            return webClient.DownloadString(BaseSite);
        }
    }
}
