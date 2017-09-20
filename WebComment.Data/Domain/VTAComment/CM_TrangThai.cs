
using System.ComponentModel.DataAnnotations;

namespace WebComment.Data
{
    public class CM_TrangThai
    {
        [Key]
        public int Id { get; set; }
        public string TenTrangThai { get; set; } 
        public string LoaiTrangThai { get; set; } //TrangThai cho Comment
        public string Status { get; set; } //Active, Deactive
    }
}
