using System;
using System.Collections.Generic;
using WebComment.Commons;

namespace WebComment.API.Models
{
    public class ProductHomeModel
    {
        public decimal GiaChiNhanhSv { get; set; }
        public decimal GiaWebSieuthiSv { get; set; }
        public decimal GiaNiemYetSv { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int RootCategoryId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ImageDetail { get; set; } 
        public decimal PriceHeadOffice { get; set; }
        public decimal PriceUnderHeadOffice { get; set; }
        public decimal PriceOnline { get; set; }
        public decimal PriceUnderOnline { get; set; }
        public decimal PriceExpected { get; set; }
        public string StatusExist { get; set; }
        public string PromotionProduct { get; set; }
        public string htmlPromotionProduct { get { return System.Web.HttpUtility.HtmlDecode(PromotionProduct); } }
        public string ProductFeatures { get; set; }
        
        public string ProductFeaturesRl
        {
            get
            {
                if (ProductFeatures != null)
                {
                    return ProductFeatures.Replace("</tr>,<tr>", "</tr><tr>");
                }
                else
                {
                    return "";
                }

            }
        }

        public string ProductShortDesc { get; set; }
        public string ProductDesc { get; set; }
        public string ProductStandardDesc { get; set; }
        public string ProductStandardImage { get; set; }
        public string SeoKeyword { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string Slug { get; set; }
        public string PromotionType { get; set; }
        public string DisplayType { get; set; }
        public string PromotionDescription { get; set; }
        public string ImageUrl {
            get
            {
                if (ImageDetail!= null && !ImageDetail.Contains("~/Upload/"))
                {
                    return "~/Upload/2015/1/2/" + ImageDetail;
                }
                
               return ImageDetail;
                
            }
        }

        public string CategoryName { get { return Globals.GetCategoryName(RootCategoryId); } }
        public string ImagePart { get { return Globals.GetGeneratedImage(ImageUrl, 160); } }
        public string Url { get { return String.Format("/{0}.html",Slug); } }
        public string StatusStock
        {
            get
            {
                switch (StatusExist)
                {
                    case "I":
                        return "Còn hàng";
                    case "D":
                        return "Đặt mua trước";
                    case "U":
                        return "Sắp có hàng";
                    case "T":
                        return "Tạm hết hàng";
                    case "O":
                        return "Hết hàng";
                    case "L":
                        return "Liên hệ";
                    case "C":
                        return "Hàng còn ít";
                    case "K":
                        return "Không kinh doanh";
                    case "A":
                        return "Còn hàng";
                    default:
                        return "[Liên hệ]";
                }
            }
        }
        
    }
    public class PromotionProduct
    {
        public string HtmlProductPromotion { get; set; }
        public int ProductCode { get; set; }
    }

   
    public class HomeProductVM
    {
        public string CategoryName { get; set; }
        public IEnumerable<ProductHomeModel> ListProduct { get; set; }
        public int CategoryId { get; set; }
        public BannerModel Banner { get; set; }

    }

    public class SponsortModel
    {
        public int SponsorId { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int Orderby { get;set; }
        public DateTime Start_date { get;set; }
        public DateTime End_date { get; set; }

    }
    public class ProdectCode
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int ParentId { get;set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ImageDetail { get;set; }
        public decimal PriceExpected { get;set; }
        public string StatusExist { get; set; }
        public string ProductFeatures { get; set; }
        public string ProductShortDesc { get; set; }
        public string ProductDesc { get; set; }
        public string ProductStandardDesc { get; set; }
        public string ProductStandardImage { get; set; }
        public string SeoKeyword { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string Slug { get; set; }
    }
    public class CampaignModel
    {
        public int BusinessCampaignId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductColorCode { get; set; }
    }

    public class ImageModel
    {
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public string ImageDesicription { get; set; }
        public string Type { get; set; }
        public string ImagePart
        {
            get
            {
                if (!ImageUrl.Contains("~/Upload/"))
                {
                    return "~/Upload/2015/1/2/" + ImageUrl;
                }

                return ImageUrl;

            }
        }
    }

    public class BlockModel
    {
        public int BlockId { get; set; }
        public string BlockName { get; set; }
        public int Position { get; set; }
        public List<int> ListProductId { get; set; }
    }

    public class BreadCrumbModel
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}