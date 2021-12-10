using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class FileResponse
    {
        public string FullPath { get; set; }

        public string MimeType { get; set; }

        public bool IsExisted { get => File.Exists(FullPath); }
    }
}
