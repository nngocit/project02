//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace WebComment.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class O_OrderType
    {
        [Key]
        public int Id { get; set; }
        public string OrderType { get; set; }
        public string Note { get; set; }
    }
}
