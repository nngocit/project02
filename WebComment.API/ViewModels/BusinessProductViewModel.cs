using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Web;
using WebComment.API.Models;
using WebComment.Data;

namespace WebComment.API.ViewModels
{
    public class BusinessProductViewModel
    {
        public List<BusinessProductModel> ListProduct { get; set; }
        public B_KhachHangDoanhNghiep KhachhangInfo { get; set; }

    }
}
    