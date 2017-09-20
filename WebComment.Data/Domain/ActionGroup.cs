using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebComment.Data
{
    public class ActionGroup
    {
        public ActionGroup()
        {
            ActionActionGroupRefs = new List<ActionActionGroupRef>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string CodeName { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }

        public virtual ICollection<ActionActionGroupRef> ActionActionGroupRefs { get; set; }
    }
}
