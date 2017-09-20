using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComment.API.Models;

namespace WebComment.API.IServices
{
    public interface IHomeService
    {
        IEnumerable<ProductHomeModel> HomeProduct(int rootCategory, int skip = 0, int take = 10);
        IEnumerable<ProductHomeModel> HomeSanphammoi(int skip = 0, int take = 10);
        IEnumerable<ProductHomeModel> HomeSanphambanchay(int skip = 0, int take = 10);
        List<HomeProductVM> HomeProductVM(int skip = 0, int take = 10);
        List<ProductHomeModel> ListAllProduct();
        List<CampaignModel> ProductBusiness(int businessId);
        //IEnumerable<ProductHomeModel> GetListProduct(List<ProdectCode> ListProducts);
        DetailModel ProductDetail(int productId);
        int GetProductId(string url);
        IEnumerable<ProductHomeModel> SearchProduct(string keySearch, int skip = 0, int take = 10);
        string GetImageByColor(int id, string color);
        SinhVienPage ListProductBanner(string campaignName);
    }
}