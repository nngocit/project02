
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Objects.DataClasses;

namespace WebComment.Data
{

    public class CM_Comment
    {

        [Key]
        public long Id { get; set; } 
        public DateTime NgayGioTao { get; set; }
        public string NoiDung { get; set; }

        public string SP_MaSP { get; set; }
        public string SP_TenSP { get; set; }
        public string SP_URL { get; set; }
        public string LoaiHienThi { get; set; }
        public string Status { get; set; } //A or D
        public string Likes { get; set; }
        public string Rating { get; set; }
        public string GhiChu { get; set; }

        public long? CM_ThongTinKhachHangId { get; set; }
        [ForeignKey("CM_ThongTinKhachHangId")]
        public virtual CM_ThongTinKhachHang ThongTinKhachHang { get; set; }

        public int? CM_LoaiCommentLevel1Id { get; set; }
        [ForeignKey("CM_LoaiCommentLevel1Id")]
        public virtual CM_LoaiComment LoaiCommentLevel1Id { get; set; }

        public int? CM_LoaiCommentLevel2Id { get; set; }
        [ForeignKey("CM_LoaiCommentLevel2Id")]
        public virtual CM_LoaiComment LoaiCommentLevel2Id { get; set; }

        public int? CM_LoaiCommentLevel3Id { get; set; }
        [ForeignKey("CM_LoaiCommentLevel3Id")]
        public virtual CM_LoaiComment LoaiCommentLevel3Id { get; set; }

        public int? CM_LoaiCommentLevel4Id { get; set; }
        [ForeignKey("CM_LoaiCommentLevel4Id")]
        public virtual CM_LoaiComment LoaiCommentLevel4Id { get; set; }

        public int? CM_TrangThaiId { get; set; }
        [ForeignKey("CM_TrangThaiId")]

        public virtual CM_TrangThai TrangThai { get; set; }
    }
}
