using System.Collections.Generic;
using System.IO;

namespace GDMLib
{
    public class Update
    {
        public string Version { get; set; }
        public string ReleaseDate { get; set; }
        public string ReleaseNotes { get; set; }
        public bool RequiredUpdate { get; set; }
        public bool VerifyUpdate { get; set; }
        public bool Skip { get; set; }
        public List<UpdateData> UpdateData = new List<UpdateData>();
        public List<DownloadFile> Files = new List<DownloadFile>();

        public static Update GetUpdateFile(string path)
        {
            Update update = null;
            if (File.Exists(path))
            {
                update = Serializer<Update>.LoadFromJSONFile(path);
            }
            else
            {
                throw new FileNotFoundException();
            }
            return update;
        }
    }
}
