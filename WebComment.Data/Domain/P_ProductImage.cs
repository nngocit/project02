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
    
    public partial class P_ProductImage
    {
        [Key]
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string ProductColorCode { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public string ImageDescription { get; set; }
        public string Type { get; set; }
    }
}
