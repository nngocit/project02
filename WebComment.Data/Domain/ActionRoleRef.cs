using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class ActionRoleRef
    {
        [Key]
        public int Id { get; set; }

        public int ActionId { get; set; }
        [ForeignKey("ActionId")]
        public virtual Action Action { get; set; }

        public string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

    }
}
