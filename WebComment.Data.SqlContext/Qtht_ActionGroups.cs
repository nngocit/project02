//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VNPOST.Data.SqlContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class Qtht_ActionGroups
    {
        public Qtht_ActionGroups()
        {
            this.Qtht_ActionActionGroupRefs = new HashSet<Qtht_ActionActionGroupRefs>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string CodeName { get; set; }
    
        public virtual ICollection<Qtht_ActionActionGroupRefs> Qtht_ActionActionGroupRefs { get; set; }
    }
}
