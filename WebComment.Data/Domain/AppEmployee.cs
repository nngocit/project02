using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class AppEmployee
    {
        [Key]
        public long Id { get; set; }

        public string DeviceUniqueId { get; set; }
        public string EmployeeId { get; set; }
        public string BranchId { get; set; }
        public string AreaId { get; set; }
        public string TokenId { get; set; }
        public string DeviceOs { get; set; }
        public DateTime? InstalledDateTime { get; set; }

        public string HoTen { get; set; }
        public string Email { get; set; }
        public string ChucDanh { get; set; }
        public string PhongBan { get; set; }

        public string DoTuoiKH { get; set; }
        public string GioiTinhKH { get; set; }
    }
}
