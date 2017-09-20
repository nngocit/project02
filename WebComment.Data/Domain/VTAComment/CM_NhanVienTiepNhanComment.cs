
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class CM_NhanVienTiepNhanComment
    {
        [Key]
        public long Id { get; set; }

        public long? CM_CommentId { get; set; }
        [ForeignKey("CM_CommentId")]
        public CM_Comment Comment { get; set; }

        public string MaNVTiepNhan_VTA { get; set; }

        public string PhongBan { get; set; }

        public DateTime? NgayGioTiepNhan { get; set; }
        public DateTime? NgayGioTraLoi { get; set; }

        public string Status { get; set; }
    }
}
