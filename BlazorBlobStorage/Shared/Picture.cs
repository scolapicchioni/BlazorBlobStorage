using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorBlobStorage.Shared {
    public class Picture {
        public int Id { get; set; }
        public string FileName { get; set; }
        public Stream  FileContent { get; set; }
        public string ContentType { get; set; }
    }
}
