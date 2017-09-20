using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComment.API.ViewModels;

namespace WebComment.API.Models
{
    public class DetailModel
    {
        public ProductHomeModel Product { get; set; }
        public List<ProductHomeModel> SanPhamMuonMua { get; set; }
        public List<ProductColor> ListColor { get; set; }
        public HappyCareModel HappyCare { get; set; }
        public List<ImageModel> ListImage { get; set; }
        public BannerModel Banner { get; set; }

    }
}