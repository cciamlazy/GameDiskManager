using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDMLib
{
    public class FileSystemHandler
    {
        public static string DataPath { get; private set; }

        public static void Initialize()
        {
            FileSystemHandler.DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\GameDiskManager\\Data\\";
        }

        public static bool IsInitialized()
        {
            return FileSystemHandler.DataPath != null && FileSystemHandler.DataPath != "";
        }

        public static string CombineDataPath(string path)
        {
            if (!IsInitialized())
                FileSystemHandler.Initialize();
            return Path.Combine(FileSystemHandler.DataPath, path);
        }

        public static string CombineDataPath(string path1, string path2)
        {
            return Path.Combine(FileSystemHandler.DataPath, path1, path2);
        }

        public static string CombineDataPath(string path1, string path2, string path3)
        {
            return Path.Combine(FileSystemHandler.DataPath, path1, path2, path3);
        }

        public static string CombineDataPath(string[] paths)
        {
            string[] newPaths = new string[paths.Length + 1];
            newPaths[0] = FileSystemHandler.DataPath;
            for (int i = 0; i < paths.Length; i++)
            {
                newPaths[i + 1] = paths[i];
            }
            return Path.Combine(newPaths);
        }

        public static void CreateDirectory(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
    }
}
