using GameDiskManager.Types;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="dest"></param>
        public static void MigrateGame(Game game, string dest)
        {
            DateTime start_time = DateTime.Now;
            game.Migrate(dest, DateTime.Now);
            long size = DepthSearch.DirectorySize(dest);
            int milliseconds = 1 + (int)((DateTime.Now - start_time).TotalMilliseconds);
            // size time in milliseconds per hour
            long tsize = size * 3600000 / milliseconds;
            tsize = tsize / (int)Math.Pow(2, 30);
            Console.WriteLine(tsize + "GB/hour");
        }

        public static void MoveGameFile (ref MigrationFile file)
        {
            file.Status = MigrationStatus.Migrating;
            DateTime start_time = DateTime.Now;
            try
            {
                FMove(file.source, file.destination, true);
                file.Status = MigrationStatus.Successful;
            }
            catch(Exception e)
            {
                file.Status = MigrationStatus.Failed;
                Console.WriteLine(e.Message);
            }
            finally
            {
                file.Time_ms = 1 + (int)((DateTime.Now - start_time).TotalMilliseconds);
            }
        }

        /// <summary> Fast file move with big buffers
        /// </summary>
        /// <param name="source">Source file path</param> 
        /// <param name="destination">Destination file path</param> 
        private static void FMove(string source, string destination, bool skipValidation = true)
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
            CopyPropertiesTo(source, destination);
            if (!skipValidation)
                ValidateTransfer(source, destination);

            File.Delete(source);
        }

        private static bool ValidateTransfer(string source, string dest)
        {
            // TODO : More validation, if required

            return FileCompare(source, dest);

            //return false;
        }

        // This method accepts two strings the represent two files to 
        // compare. A return value of 0 indicates that the contents of the files
        // are the same. A return value of any other value indicates that the 
        // files are not the same.
        private static bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open, FileAccess.Read);
            fs2 = new FileStream(file2, FileMode.Open, FileAccess.Read);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return ((file1byte - file2byte) == 0);
        }

        public static void CopyPropertiesTo(string source, string dest)
        {
            FileInfo sfi = new FileInfo(source);
            FileInfo dfi = new FileInfo(dest);

            dfi.Attributes = sfi.Attributes;
            dfi.CreationTime = sfi.CreationTime;
            dfi.CreationTimeUtc = sfi.CreationTimeUtc;
            dfi.LastAccessTime = sfi.LastAccessTime;
            dfi.LastAccessTimeUtc = sfi.LastAccessTimeUtc;
            dfi.LastWriteTime = sfi.LastWriteTime;
            dfi.LastWriteTimeUtc = sfi.LastWriteTimeUtc;
            try
            {
                dfi.SetAccessControl(sfi.GetAccessControl());
            }
            catch (Exception e)
            {
                Console.WriteLine("Tried setting access control and failed: " + e.Message);
            }
        }
    }
}
