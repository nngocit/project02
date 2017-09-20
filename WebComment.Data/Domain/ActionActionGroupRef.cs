using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class ActionActionGroupRef
    {
        [Key]
        public long Id { get; set; }

        public int ActionId { get; set; }
        [ForeignKey("ActionId")]
        public virtual Action Action { get; set; }

        public int ActionGroupId { get; set; }
        [ForeignKey("ActionGroupId")]
        public virtual ActionGroup ActionGroup { get; set; }
    }
}
