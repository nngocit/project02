using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using VNPOST.BackEnd.API.DAL;
using VNPOST.BackEnd.API.IServices;
using VNPOST.BackEnd.API.Models;
using VNPOST.BackEnd.API.ViewModels;
using VNPOST.Commons;
using VNPOST.Data;
//using VNPOST.Data.SqlContext;
using VTA.Service.CoreCache;

namespace VNPOST.BackEnd.API.Services
{
    public class HomeService : IHomeService
    {
        private IUnitOfWork _unitOfWork;
        private List<ProductHomeModel> ListProducts
        {
            get
            {
                var keyCacheProductList = "AllProduct";
                var cacheValues = DataCache.GetCache<List<ProductHomeModel>>(keyCacheProductList);
                if (cacheValues != null)
                {
                    return cacheValues;
                }
                var rs = ListAllProduct();
                DataCache.SetCache(keyCacheProductList, rs,
                    DateTime.Now.Add(TimeSpan.FromMinutes(Globals.TimeCache)));
                return rs;
            }
        }

        public HomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public DetailModel ProductDetail(int productId)
        {
            var product = ListProducts.FirstOrDefault(c => c.ProductId == productId);

            var listColorProduct = GetColorProduct(productId).ToList();

            var happyCare = GetProductHappyCareModel(Convert.ToInt32(productId), product == null ? -1 : product.PriceHeadOffice, Globals.StoreCodeConfig.Vnpost);

            var listImage = GetImageByProductId(productId);

            var ListSanphamMuonmua = ListProducts.Where(c => GetProductIdMuonmua(productId).ListProductId.Contains(c.ProductId)).ToList();

            return new DetailModel()
            {
                Product = product, 
                ListColor = listColorProduct, 
                HappyCare = happyCare,
                ListImage = listImage,
                SanPhamMuonMua = ListSanphamMuonmua.ToList()
            };
        }

        public IEnumerable<ProductHomeModel> SearchProduct(string keySearch, int skip, int take)
        {
            var rs = ListProducts.Where(c => 
                c.RootCategoryId != 171736 &&
                c.ProductName.ToUpper().Contains(keySearch.ToUpper()) || c.ProductCode.ToUpper().Contains(keySearch.ToUpper())
                ).Skip(skip).Take(take).ToList();
            return rs; 
        }

        #region all product
        public List<ProductHomeModel> ListAllProduct()
        {
            var pProduct = _unitOfWork.Repository<P_Product>().Queryable().Where(c=> c.Status=="A");
            var pCategory = _unitOfWork.Repository<P_Category>().Queryable().Where(c => c.Status == "A");
            var BusinessId = _unitOfWork.Repository<A_Business_products>().Queryable().Where(c => c.Status == "A" && c.Business_campaign_id==Globals.BusinessConfig.Vnpost).Select(s=> s.Product_code).Distinct();

            var query = (from p in pProduct
                    join c in pCategory on p.CategoryId equals c.CategoryId
                         where BusinessId.Contains(p.ProductCode)
                     select new 
                    {
                        p.ProductId,
                        p.CategoryId,
                        c.ParentId,
                        p.ProductCode,
                        p.ProductName,
                        p.ProductImage,
                        p.PriceExpected,
                        p.StatusExsit,
                        p.ProductFeatures,
                        p.ProductShortDesc,
                        p.ProductDesc,
                        p.ProductStandardDesc,
                        p.ProductStandardImage,
                        p.SeoKeyword,
                        p.SeoTitle,
                        p.SeoDescription,
                        p.Slug
                    }).OrderBy(c => c.ProductId).ToList();



            var pr = new PriceParamModel() { ProductCodes = query.Select(c => c.ProductCode).ToList(), StoreCode = Globals.StoreCodeConfig.Vnpost };
            var listGia = PriceService.GetGia_Api(pr);
            var rs = new List<ProductHomeModel>();

            foreach (var item in ListProducts)
            {
                var itemGia = listGia.FirstOrDefault(c => c.Productcode == item.ProductCode);
                rs.Add(new ProductHomeModel()
                {
                    ProductId = Convert.ToInt32(item.ProductId),
                    CategoryId = Convert.ToInt32(item.CategoryId),
                    RootCategoryId = Convert.ToInt32(item.RootCategoryId),
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    ImageDetail = item.ImageDetail,
                    PriceExpected = Convert.ToDecimal(item.PriceExpected),
                    StatusExist = item.StatusExist,
                    PriceOnline = itemGia == null ? -1 : itemGia.GiaChiNhanh,
                    PriceUnderOnline = itemGia == null ? -1 : itemGia.GiaNiemYet,
                    PriceHeadOffice = itemGia == null ? -1 : itemGia.GiaHeadOffice,
                    PriceUnderHeadOffice = itemGia == null ? -1 : itemGia.GiaNiemYetHeadOffice,
                    PromotionProduct = "",
                    ProductFeatures = item.ProductFeatures,
                    ProductShortDesc = item.ProductShortDesc,
                    ProductDesc = item.ProductDesc,
                    ProductStandardDesc = item.ProductStandardDesc,
                    SeoKeyword = item.SeoKeyword,
                    SeoDescription = item.SeoDescription,
                    SeoTitle = item.SeoTitle,
                    Slug = item.Slug,
                    ProductStandardImage = item.ProductStandardImage
                });
            }
            return rs;
            //return query.Select(item => new ProdectCode()
            //{
            //    ProductId = Convert.ToInt32(item.ProductId), 
            //    CategoryId = Convert.ToInt32(item.CategoryId), 
            //    ParentId = Convert.ToInt32(item.ParentId), 
            //    ProductCode = item.ProductCode, 
            //    ProductName = item.ProductName, 
            //    ImageDetail = item.ProductImage, 
            //    PriceExpected = Convert.ToDecimal(item.PriceExpected), 
            //    StatusExist = item.StatusExsit,
            //    ProductFeatures = item.ProductFeatures,
            //    ProductShortDesc = item.ProductShortDesc,
            //    ProductDesc = item.ProductDesc,
            //    ProductStandardDesc = item.ProductStandardDesc,
            //    SeoKeyword = item.SeoKeyword,
            //    SeoTitle = item.SeoTitle,
            //    SeoDescription = item.SeoDescription,
            //    Slug = item.Slug,
            //    ProductStandardImage = item.ProductStandardImage
            //}).ToList();


        }
        #endregion

        #region lay san pham trang home

        public List<SponsortModel> ListProductSponsort()
        {
            var keyCache = "ListProductSponsort";
            var cacheValues = DataCache.GetCache<List<SponsortModel>>(keyCache);
            if (cacheValues != null)
            {
                return cacheValues;
            }
            var pSponsor = _unitOfWork.Repository<P_Sponsor>().Queryable().Where(c => c.Status == "A");
            var pSponsorProduct = _unitOfWork.Repository<P_SponsorProduct>().Queryable().Where(c => c.Status == "A");
            var query = (from s in pSponsor
                         join sp in pSponsorProduct on s.Id equals sp.SponsorId
                         //where sp.Start_date >= DateTime.Now && sp.End_date <= DateTime.Now
                         select new
                         {
                             sp.SponsorId,
                             s.Name,
                             sp.ProductId,
                             sp.Orderby,
                             sp.Start_date,
                             sp.End_date
                         }).OrderBy(c => c.Orderby).ToList();
            var rs = query.Select(c => new SponsortModel() { ProductId = Convert.ToInt32(c.ProductId), Name = c.Name, SponsorId = c.SponsorId, Orderby = c.Orderby, Start_date = c.Start_date, End_date = c.End_date }).ToList();
            DataCache.SetCache(keyCache, rs, DateTime.Now.Add(TimeSpan.FromMinutes(Globals.TimeCache)));
            return rs;
        }

        public IEnumerable<ProductHomeModel> HomeProduct(int rootCategory, int skip, int take)
        {
            var rs = ListProducts.Where(c =>
                (rootCategory == 0 || c.RootCategoryId == rootCategory)
                ).Skip(skip).Take(take).ToList();
            return rs;
        }
        public List<HomeProductVM> HomeProductVM(int skip, int take)
        {
            var data = ListProducts;
            return GetHomeProductVM(data, skip, take);
        }
        public List<CampaignModel> ProductBusiness(int businessId)
        {
            var keyCache = "ProductBusiness"+ businessId;
            var cacheValues = DataCache.GetCache<List<CampaignModel>>(keyCache);
            if (cacheValues != null)
            {
                return cacheValues;
            }
            var aBusiness = _unitOfWork.Repository<A_Business>().Queryable().Where(c => c.Status == "A");
            var aBusinessCampaign = _unitOfWork.Repository<A_Business_campaign>().Queryable().Where(c => c.Status == "A");
            var aBusinessProducts = _unitOfWork.Repository<A_Business_products>().Queryable().Where(c => c.Status == "A");
            var query = (from b in aBusiness
                         join bc in aBusinessCampaign on b.Business_id equals bc.Business_id
                         join bp in aBusinessProducts on bc.Business_campaign_id equals bp.Business_campaign_id
                         where bp.Business_campaign_id == businessId 
                         select new
                         {
                             bp.Business_campaign_id,
                             bp.Product_id,
                             bp.Product_code,
                             bp.Product_color_code
                             
                         }).ToList();
            var rs = query.Select(c => new CampaignModel() { BusinessCampaignId = c.Business_campaign_id, ProductId = c.Product_id, ProductCode = c.Product_code, ProductColorCode = c.Product_color_code }).ToList();
            DataCache.SetCache(keyCache, rs, DateTime.Now.Add(TimeSpan.FromMinutes(Globals.TimeCache)));
            return rs;
        }
        public IEnumerable<ProductHomeModel> HomeSanphammoi(int skip, int take)
        {
            var idsp_moi = ListProductSponsort().Where(c => c.SponsorId == Globals.CoreCategory.Sanphammoi).Select(s => s.ProductId);
            var rs = ListProducts.Where(c => idsp_moi.Contains(c.ProductId)
                ).Skip(skip).Take(take).ToList();
            return rs;
        }
        public IEnumerable<ProductHomeModel> HomeSanphambanchay(int skip, int take)
        {
            var idsp_banchay = ListProductSponsort().Where(c => c.SponsorId == Globals.CoreCategory.Sanphambanchay).Select(s => s.ProductId);
            var rs = ListProducts.Where(c => idsp_banchay.Contains(c.ProductId)
                ).Skip(skip).Take(take).ToList();
            return rs; 
        }
        #endregion

        #region Promotion item product
        public List<ProductHomeModel> GetPromotionProdut(List<ProductHomeModel> lstProducts)
        {
            var CodesId = "";
            var numItem = 0;
            var StoreId = Globals.PriceStoreDefault;
            if (lstProducts.Any())
            {
                foreach (var item in lstProducts)
                {
                    numItem = numItem + 1;
                    if (numItem == 1)
                    {
                        CodesId = item.ProductCode;
                    }
                    else
                    {
                        CodesId = CodesId + "," + item.ProductCode;
                    }
                }
                var listPromotion = ListPromotion(CodesId, StoreId);
                foreach (var itemPromotion in listPromotion)
                {
                    foreach (var productItem in lstProducts)
                    {
                        var checkPromotionItem = lstProducts.FirstOrDefault(c => c.ProductCode == productItem.ProductCode);
                        if (checkPromotionItem != null)
                        {
                            productItem.PromotionProduct = itemPromotion.HtmlProductPromotion;
                        }
                    }
                }
            }

            return lstProducts;

        }
        public static List<PromotionProduct> ListPromotion(string CodesId, string StoreId)
        {
            var client = new RestClient(Globals.VTAWebAPIPublicUrl);
            var req = new RestRequest("/promotion-api/Product?CodesId=" + CodesId + "&StoreId=" + StoreId, Method.GET) { RequestFormat = DataFormat.Json };

            var rs = client.Execute<HttpContentResult<List<PromotionProduct>>>(req).Data;

            if (rs.Data == null && rs.StatusCode != "200")
                return null;
            return rs.Data;
        }
        #endregion

        #region function public
        //public IEnumerable<ProductHomeModel> GetListProduct(List<ProdectCode> ListProducts)
        //{
        //    var pr = new PriceParamModel() { ProductCodes = ListProducts.Select(c => c.ProductCode).ToList(), StoreCode = Globals.StoreCodeConfig.Vnpost };
        //    var listGia = PriceService.GetGia_Api(pr);
        //    var rs = new List<ProductHomeModel>();

        //    foreach (var item in ListProducts)
        //    {
        //        var itemGia = listGia.FirstOrDefault(c => c.Productcode == item.ProductCode);
        //        rs.Add(new ProductHomeModel()
        //        {
        //            ProductId = Convert.ToInt32(item.ProductId),
        //            CategoryId = Convert.ToInt32(item.CategoryId),
        //            RootCategoryId = Convert.ToInt32(item.ParentId),
        //            ProductCode = item.ProductCode,
        //            ProductName = item.ProductName,
        //            ImageDetail = item.ImageDetail,
        //            PriceExpected = Convert.ToDecimal(item.PriceExpected),
        //            StatusExist = item.StatusExist,
        //            PriceOnline = itemGia == null ? -1 : itemGia.GiaChiNhanh,
        //            PriceUnderOnline = itemGia == null ? -1 : itemGia.GiaNiemYet,
        //            PriceHeadOffice = itemGia == null ? -1 : itemGia.GiaHeadOffice,
        //            PriceUnderHeadOffice = itemGia == null ? -1 : itemGia.GiaNiemYetHeadOffice,
        //            PromotionProduct = "",
        //            ProductFeatures = item.ProductFeatures,
        //            ProductShortDesc = item.ProductShortDesc,
        //            ProductDesc = item.ProductDesc,
        //            ProductStandardDesc = item.ProductStandardDesc,
        //            SeoKeyword = item.SeoKeyword,
        //            SeoDescription = item.SeoDescription,
        //            SeoTitle = item.SeoTitle,
        //            Slug = item.Slug,
        //            ProductStandardImage = item.ProductStandardImage
        //        });
        //    }
        //    return GetPromotionProdut(rs);
        //}
        public List<HomeProductVM> GetHomeProductVM(List<ProductHomeModel> ListProduct, int skip, int take)
        {
            var rs = new List<HomeProductVM>();
            foreach (var itemCate in Globals.AllCategory)
            {
                var cate = itemCate;
                var data = ListProduct.Where(c => cate != null && c.RootCategoryId == Globals.GetCategoryId(cate.CategoryName)).Skip(skip).Take(take);
                if (data.Any())
                {
                    rs.Add(new HomeProductVM() { CategoryName = itemCate.CategoryName, ListProduct = data });
                }
            }

            return rs;
        }
        private IEnumerable<ProductColor> GetColorProduct(long productId)
        {
            var colors = _unitOfWork.Repository<P_ProductColor>().Queryable();

            var query = (from c in colors
                         where c.ProductId == productId
                         select new ProductColor()
                         {
                             Thumbs = c.ColorImage,
                             ColorCode = c.ColorCode,
                             ColorId = c.ColorId,
                             ColorName = c.ColorName,
                             ProductColorCode = c.ProductColorCode,
                             Productid = c.ProductId
                         }
                          ).ToList();

            return query;
        }
        public HappyCareModel GetProductHappyCareModel(int productId, decimal price, string storeCode)
        {
            HappyCareDetailModel modelGhbh = new HappyCareDetailModel();
            HappyCareDetailModel modelBvtd = new HappyCareDetailModel();

            var tblCategory = _unitOfWork.Repository<P_Category>().Queryable();
            var tblHappyCare = _unitOfWork.Repository<P_Happycare>().Queryable();
            var tblProduct = _unitOfWork.Repository<P_Product>().Queryable();

            var firstOrDefault = (from p in tblProduct
                                  join c in tblCategory on p.CategoryId equals c.CategoryId
                                  where p.ProductId == productId
                                  select new
                                  {
                                      idPath = c.IdPath
                                  }).FirstOrDefault();
            if (firstOrDefault != null)
            {
                var idPath = firstOrDefault.idPath;
                var rsCategory = 0;
                //Nếu là Iphone
                rsCategory = idPath.Contains("152708") ? 152708 : Convert.ToInt32(idPath.Split('/').Any() ? idPath.Split('/')[0] : idPath);

                var happycare =
                    tblHappyCare.FirstOrDefault(
                        c =>
                            c.MinPrice <= price && c.MaxPrice >= price && c.CategoryId == rsCategory);

                if (happycare != null)
                {
                    var lstHappyCare = new List<string>() { happycare.GHBHCode, happycare.BVTDCode };
                    var reqPriceModel = new PriceParamModel
                    {
                        ProductCodes = lstHappyCare,
                        StoreCode = storeCode
                    };
                    var qr = tblProduct.Where(c => lstHappyCare.Contains(c.ProductCode)).ToList();
                    //Lay Gia
                    var rsApi = PriceService.GetGia_Api(reqPriceModel);

                    var checkPriceByListCodeInfoBv = rsApi.FirstOrDefault(c => c.Productcode == happycare.BVTDCode);
                    if (checkPriceByListCodeInfoBv != null)
                    {
                        var pProduct = qr.FirstOrDefault(c => c.ProductCode == happycare.BVTDCode);
                        if (pProduct != null)
                            modelBvtd = new HappyCareDetailModel
                            {
                                price = Convert.ToInt32(checkPriceByListCodeInfoBv.GiaChiNhanh),
                                product_id = pProduct.ProductId.ToString(),
                                HappyCareName = "Bảo vệ toàn diện",
                                ParentId = productId,
                                ProductColorCode = pProduct.ProductCode
                            };
                    }
                    var checkPriceByListCodeInfoGh = rsApi.FirstOrDefault(c => c.Productcode == happycare.GHBHCode);
                    if (checkPriceByListCodeInfoGh != null)
                    {
                        var pProduct = qr.FirstOrDefault(c => c.ProductCode == happycare.GHBHCode);
                        if (pProduct != null)
                            modelGhbh = new HappyCareDetailModel
                            {
                                price = Convert.ToInt32(checkPriceByListCodeInfoGh.GiaChiNhanh),
                                product_id = pProduct.ProductId.ToString(),
                                HappyCareName = "Gia hạn bảo hành",
                                ParentId = productId,
                                ProductColorCode = pProduct.ProductCode
                            };
                    }
                }
            }

            return new HappyCareModel(modelGhbh, modelBvtd);
        }

        public int GetProductId(string url)
        {
            var nameResolve = url.Replace(".html", string.Empty);
            var rs = ListProducts.FirstOrDefault(c => c.Slug == nameResolve);
            return  rs != null ? rs.ProductId : 0;
        }

        public List<ImageModel> GetImageByProductId(int productId)
        {
            return _unitOfWork.Repository<P_ProductImage>().Queryable().Where(c => c.ProductId == productId)
                .Select(c=> new ImageModel()
                {
                    ImageDesicription = c.ImageDescription,
                    ImageName = c.ImageName,
                    ImageUrl = c.ImageUrl,
                    Type = c.Type
                }).ToList();
        }

        public static BlockModel GetProductIdMuonmua(int productId)
        {
            var client = new RestClient(Globals.VTAWebAPIPublicUrl);
            var req = new RestRequest("/product-api/GetProductIdMuonmua?productId=" + productId, Method.GET) { RequestFormat = DataFormat.Json };

            var rs = client.Execute<HttpContentResult<BlockModel>>(req).Data;

            if (rs.Data == null && rs.StatusCode != "200")
                return null;
            return rs.Data;
        }
        #endregion
        
    }
}