using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComment.Data;

namespace WebComment.API.ViewModels
{
    public class AffiliateModel
    {
        public int AffiliateBranchId { get; set; }
        public string Name { get; set; }

        public AffiliateModel(A_Affiliate_branch model)
        {
            AffiliateBranchId = model.Affiliate_branch_id;
            Name = model.Name;
        }
    }
    public class BusinessModel
    {
        public int BusinessId { get; set; }
        public string Name { get; set; }

        public BusinessModel(A_Business model)
        {
            BusinessId = model.Business_id;
            Name = model.Name;
        }
    }

    public class BusinessCampaignModel
    {
        public int BusinessCampaignId { get; set; }
        public string Name { get; set; }

        public BusinessCampaignModel(A_Business_campaign model)
        {
            BusinessCampaignId = model.Business_campaign_id;
            Name = model.Name;
        }
    }
    public class CampaignProductModel
    {
        public int Id { get; set; }
        public int BusinessCampaignId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductColorCode { get; set; }
        public string Nhomhang { get; set; }
        public string Brand { get; set; }
        public string StoreCode { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Status { get; set; }
        public string PromotionDescription { get; set; }
        public string Note { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string DisplayType { get; set; }
        public string PromotionType { get; set; }


        public CampaignProductModel(A_Business_products model, string productName)
        {
            Id = model.Id;
            BusinessCampaignId = model.Business_campaign_id;
            ProductId = model.Product_id;
            ProductCode = model.Product_code;
            ProductColorCode = model.Product_color_code;
            Nhomhang = model.Nhomhang;
            Brand = model.Brand;

            StoreCode = model.Tbl_branch_code;
            DateStart = model.Date_start;
            DateEnd = model.Date_end;
            Status = model.Status;
            PromotionDescription = model.Promotion_description;
            Note = model.Note;
            ModifiedDate = model.Modified_date;
            ProductName = productName;
            DisplayType = model.Display_type;
            PromotionType = model.Promotion_type;

        }

        public CampaignProductModel()
        {
           
        }
    }
    public class CampaignProductParameter
    {
       
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PromotionDescription { get; set; }
        public List<int> ListCampaignProduct { get; set; }
        public string ProductCode { get; set; }
        public int BusinessCampaignId { get; set; }
        public string DisplayType { get; set; }
        public string PromotionType { get; set; }

       
    }
    public class BusinessCampaigViewnModel
    {
        public A_Business Business{ get; set; }
        public A_Business_campaign Campaign { get; set; }

        
    }
    public class BannerViewnModel:P_Banner
    {
     

    }
     public class BusinessProductParameter
      {
       
        public long Id { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PromotionDescription { get; set; }
        public List<long> ListProduct { get; set; }
        public string ProductCode { get; set; }
        public int BusinessId { get; set; }
        public DateTime StartDateImport { get; set; }
        public DateTime EndDateImport { get; set; }
        public string Brand { get; set; }
        public string Nhomhang { get; set; }
        public decimal Price { get; set; }
        public decimal NoInterestPrice { get; set; }
        public decimal InterestRatePrice { get; set; }


       
    }
}