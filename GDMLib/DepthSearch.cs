using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib
{
    public static class DepthSearch
    {
        public static List<string> GetDirectories(string path, string searchPattern = "*",
        SearchOption searchOption = SearchOption.AllDirectories)
        {
            if (searchOption == SearchOption.TopDirectoryOnly)
                return Directory.GetDirectories(path, searchPattern).ToList();

            var directories = new List<string>(GetDirectories(path, searchPattern));

            for (var i = 0; i < directories.Count; i++)
                directories.AddRange(GetDirectories(directories[i], searchPattern));

            return directories;
        }

        private static List<string> GetDirectories(string path, string searchPattern)
        {
            try
            {
                return Directory.GetDirectories(path, searchPattern).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<string>();
            }
        }

        public static long DirectorySize(string directory)
        {
            long size = 0;
            Console.WriteLine("Scanning");
            if (Directory.Exists(directory))
            {
                string[] folders = GetDirectories(directory).ToArray();
                string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

                //Get all the sizes
                long[] sizes = new long[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo fi = new FileInfo(files[i]);
                    if (fi.Exists)
                    {
                        sizes[i] = fi.Length;
                    }
                }
                size = sizes.Sum();
            }
            else
            {
                Console.WriteLine("Directory doesn't exist");
                throw (new Exception());
            }
            return size;
        }
    }
}
