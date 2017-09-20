
using System;
using System.ComponentModel.DataAnnotations;

namespace WebComment.Data
{
    public class CM_LichLamViec
    {
        [Key]
        public long Id { get; set; }
        public string MaNV { get; set; }
        public int? Ca  { get; set; } 
        public int? Ngay { get; set; }
        public DateTime? NgayLamViec { get; set; } 
    }
}
