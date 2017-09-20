
using System;
using System.ComponentModel.DataAnnotations;

namespace WebComment.Data
{
    public class CM_ThongTinKhachHang
    {
        [Key]
        public long Id { get; set; } 
        public string ChucDanh { get; set; }
        public string HoTen { get; set; }
        public string Avatar { get; set; }
        public string GioiTinh { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string IPAddress { get; set; }
        public DateTime NgayGioTao { get; set; }

        public string CookieId { get; set; }
    }
}
