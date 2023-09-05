using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class FileDto : EntityDto<string>
    {
        public override string Description
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.FileDescription))
                {
                    return this.FileName;
                }
                return FileDescription;
            }
        }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public byte[] ByteArray { get; set; }
        public string FileType { get; set; }
        public bool ForceDownload { get; set; }
    }
}
