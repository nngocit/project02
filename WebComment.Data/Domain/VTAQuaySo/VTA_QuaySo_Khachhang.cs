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
    
    public partial class VTA_QuaySo_Khachhang
    {
        [Key]
        public long Id { get; set; }
        public string Ngay { get; set; }
        public string MaCn { get; set; }
        public string TenKho { get; set; }
        public string TenKH { get; set; }
        public string Phone { get; set; }
        public string DiaChi { get; set; }
        public string InvoiceNo { get; set; }
        public string MaKh { get; set; }
        public int LoaiQua { get; set; }
        public int Tuan { get; set; }
        public int Tinh { get; set; }
        public int EventId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}