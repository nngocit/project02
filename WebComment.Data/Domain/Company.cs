using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public partial class Company
    {
        [Key]
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Nullable<long> ParentCompanyId { get; set; }
        [ForeignKey("ParentCompanyId")]
        public virtual Company ParentCompany { get; set; }
        public string Note { get; set; }
        public Nullable<int> CompanyLevel { get; set; }
        public string Status { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
