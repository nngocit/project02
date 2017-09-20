using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebComment.Data
{
    public class Action
    {
        public Action()
        {
            ActionActionGroupRefs = new List<ActionActionGroupRef>();
            ActionRoleRefs = new List<ActionRoleRef>();
            ActionUserRefs = new List<ActionUserRef>();
        }

        [Key]
        public int Id { get; set; }

        public string CodeName { get; set; }

        public string PublicDomainName { get; set; }

        public string ControllerName { get; set; } //game ~ GameController
        public string ActionName { get; set; } //play ~ action Play
        public string ActionDisplayName { get; set; } //Chơi game
        public string AreaName { get; set; } //"", "Admin"

        public bool IsActive { get; set; }
        public string Description { get; set; }

        private string _listRole;
        public string ListRole
        {
            get
            {
                if (string.IsNullOrEmpty(_listRole)) _listRole = "*";
                return _listRole;
            }
            set { _listRole = value; }
        }

        public virtual ICollection<ActionActionGroupRef> ActionActionGroupRefs { get; set; }
        public virtual ICollection<ActionRoleRef> ActionRoleRefs { get; set; }
        public virtual ICollection<ActionUserRef> ActionUserRefs { get; set; }


    }
}
