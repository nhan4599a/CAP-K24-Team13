using System.IO;

namespace Shared.Models
{
    public class FileResponse
    {
        public virtual string FullPath { get; set; }

        public virtual string MimeType { get; set; }

        public virtual bool IsExisted { get => File.Exists(FullPath); }
    }
}
