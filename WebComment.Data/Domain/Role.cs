using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebComment.Data
{
    public class Role : IdentityRole
    {
        public string Description { get; set; } //mô tả về Role

        public string Type { get; set; } //phân loại Role: vd: theo Level, theo chức danh, vị trí, theo phòng ban,...

        public string Code { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }

        public int Priority { get; set; }
        public string ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Role Parent { get; set; }

        public System.DateTime CreateDate { get; set; }
        public System.DateTime? LastUpdateDate { get; set; }

        public virtual ICollection<UserInRole> UserInRoles { get; set; }

        public Role()
        {
            CreateDate = DateTime.Now;
        }
    }
}
