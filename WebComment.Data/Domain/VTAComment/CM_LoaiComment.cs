
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    using System;
    using System.Collections.Generic;

    public class CM_LoaiComment
    {
        [Key]
        public int Id { get; set; } 
        public string TenLoaiComment { get; set; }
        public int? Level { get; set; }
        public string HierarchyPath { get; set; }
        public string Status { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual CM_LoaiComment Parent { get; set; }
    }
}
