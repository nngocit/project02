using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class AppDevice
    {
        [Key]
        public long Id { get; set; }

        public string DeviceIMEI { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceOs { get; set; }
        public string AppVersion { get; set; }
        public DateTime? DateCreate { get; set; }
        public string Status { get; set; }
        public string DeviceVersion { get; set; }
        public string DeviceName { get; set; }
        public string DeviceBrand { get; set; }
    }
}
