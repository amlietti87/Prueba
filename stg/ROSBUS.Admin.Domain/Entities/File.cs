using System;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public class FileInternal
    {
        public FileInternal()
        {

        }

        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string FileType { get; set; }
        public byte[] ByteArray { get; set; }
        public bool ForceDownload { get; set; }
    }
}
