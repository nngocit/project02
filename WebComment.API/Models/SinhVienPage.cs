using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComment.API.Models
{
    public class SinhVienPage
    {
        public List<ProductHomeModel> ListProduct { get; set; }
        public List<PBanneModel> ListBanner { get; set; }
    }
}