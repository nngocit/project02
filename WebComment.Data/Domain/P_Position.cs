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

    public partial class P_Position
    {
        [Key]
        public int Id { get; set; }

        public string Position { get; set; }
        public string PositionName { get; set; }
        

    }
}
