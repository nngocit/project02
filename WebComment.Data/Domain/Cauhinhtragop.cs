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
    
    public partial class Cauhinhtragop
    {
        [Key]
        public long Id { get; set; }
        public int ProductId { get; set; }
        public string Ntc { get; set; }
        public string MaNtc { get; set; }
        public string Tengoitragop { get; set; }
        public string Magoitragop { get; set; }
        public string Loaigiayto { get; set; }
        public string Magiayto { get; set; }
        public int KhoanvayMin { get; set; }
        public int KhoanvayMax { get; set; }
        public int TratruocMin { get; set; }
        public int TratruocMax { get; set; }
        public int KyhanMin { get; set; }
        public int KyhanMax { get; set; }
        public double Laisuat { get; set; }
        public bool Show { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
