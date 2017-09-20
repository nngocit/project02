using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class ActionUserRef //độ ưu tiên cao hơn so với ActionRoleRef
    {
        [Key]
        public int Id { get; set; }

        public int ActionId { get; set; }
        [ForeignKey("ActionId")]
        public virtual Action Action { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public bool CanDo { get; set; } //xác định User có quyền hoặc không có quyền play Action này

    }
}
