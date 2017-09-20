
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class CM_NVQuanLyDanhGia
    {
        [Key]
        public long Id { get; set; }

        public long? CM_CommentId { get; set; }
        [ForeignKey("CM_CommentId")]
        public CM_Comment Comment { get; set; }

        public string MaNVQuanLy_VTA { get; set; }

        public string PhongBan { get; set; }

        public DateTime? NgayGioDanhGia { get; set; }
        public decimal? DiemDanhGia { get; set; }

        public string Status { get; set; }
    }
}
