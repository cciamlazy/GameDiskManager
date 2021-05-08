using System;
using System.IO;

namespace GameDiskManager.Utility
{
    public static class FastMove
    {
        /// <summary> Time the Move
        /// </summary> 
        /// <param name="source">Source file path</param> 
        /// <param name="destination">Destination file path</param> 
        public static void MoveTime(string source, string destination)
        {
            DateTime start_time = DateTime.Now;
            FMove(source, destination);
            long size = new FileInfo(destination).Length;
            int milliseconds = 1 + (int)((DateTime.Now - start_time).TotalMilliseconds);
            // size time in milliseconds per hour
            long tsize = size * 3600000 / milliseconds;
            tsize = tsize / (int)Math.Pow(2, 30);
            Console.WriteLine(tsize + "GB/hour");
        }

        /// <summary> Fast file move with big buffers
        /// </summary>
        /// <param name="source">Source file path</param> 
        /// <param name="destination">Destination file path</param> 
        static void FMove(string source, string destination)
        {
            int array_length = (int)Math.Pow(2, 19);
            byte[] dataArray = new byte[array_length];
            using (FileStream fsread = new FileStream
            (source, FileMode.Open, FileAccess.Read, FileShare.None, array_length))
            {
                using (BinaryReader bwread = new BinaryReader(fsread))
                {
                    using (FileStream fswrite = new FileStream
                    (destination, FileMode.Create, FileAccess.Write, FileShare.None, array_length))
                    {
                        using (BinaryWriter bwwrite = new BinaryWriter(fswrite))
                        {
                            for (; ; )
                            {
                                int read = bwread.Read(dataArray, 0, array_length);
                                if (0 == read)
                                    break;
                                bwwrite.Write(dataArray, 0, read);
                            }
                        }
                    }
                }
            }
            File.Delete(source);
        }
    }
}
