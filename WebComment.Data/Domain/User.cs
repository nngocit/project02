using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebComment.Data
{
    public class User : IdentityUser
    {
        public User()
        {
            CreateDate = DateTime.Now;
            UserInRoles = new List<UserInRole>();
        }

        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string PersonalId { get; set; }
        public string EmployeeId { get; set; }
        public string Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime? LastUpdateDate { get; set; }

        public DateTime? LastAssign { get; set; }
        public long? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        public virtual ICollection<UserInRole> UserInRoles { get; set; }

        //info for WebComment
        public string Avatar { get; set; }
        public string PhongBan { get; set; }
        public string ChucDanh { get; set; }
    }
}
