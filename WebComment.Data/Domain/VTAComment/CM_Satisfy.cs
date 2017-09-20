
using System;
using System.ComponentModel.DataAnnotations;

namespace WebComment.Data
{
    public class CM_Satisfy
    {
        [Key]
        public long Id { get; set; }
        public long? CM_ReplyCommentId { get; set; } 
        public DateTime NgayGioTao { get; set; }
        public string LoaiSatisfy { get; set; } 
        public string NoiDung { get; set; } 
        public string Status { get; set; } 
    }
}
