using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class UserLoginToken
    {
        [Key]
        public long Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public string Domain { get; set; }

        public string Token { get; set; }

        public DateTime? ExpireDate { get; set; }

        public DateTime CreateDate { get; set; }

        public UserLoginToken()
        {
            CreateDate = DateTime.Now;
        }
    }
}
