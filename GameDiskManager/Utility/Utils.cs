using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GameDiskManager.Utility
{
    public static class Utils
    {
        /// <summary>
        /// Get relative path
        /// </summary>
        /// <param name="relativeTo"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetRelativePath(string relativeTo, string path)
        {
            if (path == "") return "";
            var uri = new Uri(relativeTo);
            var rel = Uri.UnescapeDataString(uri.MakeRelativeUri(new Uri(path)).ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (rel.Contains(Path.DirectorySeparatorChar.ToString()) == false)
            {
                rel = $".{ Path.DirectorySeparatorChar }{ rel }";
            }
            return rel;
        }

        public static string BytesToString(long byteCount, int decimalPlaces = 1)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), decimalPlaces);
            return (Math.Sign(byteCount) * num).ToString() + " " + suf[place];
        }

        public static string TransferSpeed(long amount, int seconds)
        {
            if (seconds == 0) return "0 B/s";
            return string.Format("{0}/s", BytesToString(amount/seconds));
        }

        // Scale an image without disorting it.
        // Return a centered rectangle in the destination area.
        public static RectangleF ScaleRect(
            RectangleF source_rect, RectangleF dest_rect)
        {
            float source_aspect =
                source_rect.Width / source_rect.Height;
            float wid = dest_rect.Width;
            float hgt = dest_rect.Height;
            float dest_aspect = wid / hgt;

            if (source_aspect > dest_aspect)
            {
                // The source is relatively short and wide.
                // Use all of the available width.
                hgt = wid / source_aspect;
            }
            else
            {
                // The source is relatively tall and thin.
                // Use all of the available height.
                wid = hgt * source_aspect;
            }

            // Center it.
            float x = dest_rect.Left + (dest_rect.Width - wid) / 2;
            float y = dest_rect.Top + (dest_rect.Height - hgt) / 2;
            return new RectangleF(x, y, wid, hgt);
        }
    }
}
