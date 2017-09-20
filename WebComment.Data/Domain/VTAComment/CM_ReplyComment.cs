
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class CM_ReplyComment
    {
        [Key]
        public long Id { get; set; }
        public DateTime NgayGioTao { get; set; }
        public string NoiDung { get; set; } 
        public string Status { get; set; }
        public string Likes { get; set; }
        public string TypeName { get; set; }  //CMDG or CMCM

        public long? CM_CommentId { get; set; }
        [ForeignKey("CM_CommentId")]
        public virtual CM_Comment Comment { get; set; }

        public long? CM_ThongTinKhachHangId { get; set; }
        [ForeignKey("CM_ThongTinKhachHangId")]
        public virtual CM_ThongTinKhachHang ThongTinKhachHang { get; set; }

        public long? CM_NhanVienTiepNhanCommentId { get; set; }
        [ForeignKey("CM_NhanVienTiepNhanCommentId")]
        public virtual CM_NhanVienTiepNhanComment NhanVienTiepNhanComment { get; set; } // sử dụng trong trường hợp NVCS hoặc SO trả lời comment.

        public string MaNV_VTAReply { get; set; } //use for trường hợp quản lý trả lời comment

    }
}
