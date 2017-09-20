
using System;
using System.ComponentModel.DataAnnotations;

namespace WebComment.Data
{
    public class CM_UserLoginHistory
    {
        [Key]
        public long Id { get; set; }
        public string MaNV { get; set; }
        public string PhongBan  { get; set; } 
        public DateTime? Date { get; set; }
        public DateTime? LogInTime { get; set; } 
        public DateTime? LogOutTime { get; set; }
    }
}
