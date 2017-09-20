using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using RestSharp;
using VTA.Service.CoreCache;
using WebComment.API.DAL;
using WebComment.API.IServices;
using WebComment.API.Models;
using WebComment.API.ViewModels;
using WebComment.Commons;
using WebComment.Data;

namespace WebComment.API.Services
{
    public class HomeService : IHomeService
    {
        private IUnitOfWork _unitOfWork;
        private IBannerServices _bannerServices;
        private List<ProductHomeModel> ListProducts;
        /* {
             get
             {

                 Stopwatch stopwatch = new Stopwatch();
                 stopwatch.Start();
                 var keyCacheProductList = "AllProduct";
                 var cacheValues = DataCache.GetCache<List<ProductHomeModel>>(keyCacheProductList);
                 if (cacheValues != null)
                 {
                     return cacheValues;
                 }
                 var rs = ListAllProduct();
                 NLog.LogManager.GetCurrentClassLogger().Debug(" ListAllProduct() - la {0} giay ", stopwatch.Elapsed.TotalSeconds);  // đã ghi log OK 
                 DataCache.SetCache(keyCacheProductList, rs,
                     DateTime.Now.Add(TimeSpan.FromMinutes(Globals.TimeCache)));
                 return rs;
             }
         }
 */
        public HomeService(IUnitOfWork unitOfWork, IBannerServices bannerServices)
        {
            _unitOfWork = unitOfWork;
            _bannerServices = bannerServices;
            ListProducts = GetCacheListProducts();
        }


        public DetailModel ProductDetail(int productId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var product = ListProducts.FirstOrDefault(c => c.ProductId == productId);
            NLog.LogManager.GetCurrentClassLogger().Debug(" ProductDetail() - product({0}) la {1} giay ", (object) productId, (object) stopwatch.Elapsed.TotalSeconds);  // đã ghi log OK 

            var listColorProduct = GetColorProduct(productId).ToList();
            NLog.LogManager.GetCurrentClassLogger().Debug(" ProductDetail() - listColorProduct({0}) la {1} giay ", (object) productId, (object) stopwatch.Elapsed.TotalSeconds);  // đã ghi log OK 

            var happyCare = new HappyCareModel();//GetProductHappyCareModel(Convert.ToInt32(productId), product == null ? -1 : product.PriceHeadOffice, Globals.StoreCodeConfig.Vnpost);
            NLog.LogManager.GetCurrentClassLogger().Debug(" ProductDetail() - happyCare({0}) la {1} giay ", (object) productId, (object) stopwatch.Elapsed.TotalSeconds);  // đã ghi log OK 

            var listImage = GetImageByProductId(productId);
            NLog.LogManager.GetCurrentClassLogger().Debug(" ProductDetail() - listImage({0}) la {1} giay ", (object) productId, (object) stopwatch.Elapsed.TotalSeconds);  // đã ghi log OK 

            var idmuonmua = GetProductIdMuonmua(productId);
            var ListSanphamMuonmua = new List<ProductHomeModel>();
            if (idmuonmua != null)
            {
                if (idmuonmua.ListProductId != null)
                {
                    ListSanphamMuonmua = ListProducts.Where(c => idmuonmua.ListProductId.Contains(c.ProductId)).ToList();
                }

            }

            NLog.LogManager.GetCurrentClassLogger().Debug(" ProductDetail() - ListSanphamMuonmua({0}) la {1} giay ", (object) productId, (object) stopwatch.Elapsed.TotalSeconds);  // đã ghi log OK 

            return new DetailModel()
            {
                Product = product,
                ListColor = listColorProduct,
                HappyCare = happyCare,
                ListImage = listImage,
                SanPhamMuonMua = ListSanphamMuonmua
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
            List<ProductHomeModel> result = new List<ProductHomeModel>();
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var client = new RestClient(Globals.VTAWebAPIPublicUrl);
                var req = new RestRequest("product-api/GetListProductByCampaign?name=" + Globals.BusinessConfig.VnpostName, Method.GET) { RequestFormat = DataFormat.Json };
                var rs = client.Execute<HttpContentResult<List<ProductHomeModel>>>(req).Data;
                NLog.LogManager.GetCurrentClassLogger().Debug(" ListAllProduct Api() - la {0} giay ", stopwatch.Elapsed.TotalSeconds);
                if (rs.Data == null && rs.StatusCode != "200")
                    return new List<ProductHomeModel>();
                //return GetPromotionProdut(rs.Data);
                return rs.Data;
            }
            catch (Exception ex)
            {
                return result;
                NLog.LogManager.GetCurrentClassLogger().Debug(" ListAllProduct() - Mes: ({0}) ", ex.ToString());
                throw;
            }
        }

        public SinhVienPage ListProductBanner(string campaignName)
        {
            try
            {
                var client = new RestClient(Globals.VTAWebAPIPublicUrl);
                var req = new RestRequest("/product-api/GetListProductByCampaign?name=" + campaignName, Method.GET) { RequestFormat = DataFormat.Json };
                var rs = client.Execute<HttpContentResult<List<ProductHomeModel>>>(req).Data;
                var result = new SinhVienPage();
                if (rs.Data == null && rs.StatusCode != "200")
                    return result;

                result.ListProduct = rs.Data;//GetPromotionProdut(rs.Data);
                result.ListBanner = _bannerServices.GetListBannerbyCampaignName(campaignName);
                return result;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug(" ListAllProduct() - Mes: ({0}) ", ex.ToString());
                throw;
            }
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
            var rs = new List<SponsortModel>();
            rs = query.Select(c => new SponsortModel()
            {
                ProductId = Convert.ToInt32(c.ProductId),
                Name = c.Name,
                SponsorId = c.SponsorId,
                Orderby = c.Orderby,
                Start_date = c.Start_date,
                End_date = c.End_date
            }).ToList();
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
            return GetHomeProductVM(ListProducts, skip, take);
        }
        public List<CampaignModel> ProductBusiness(int businessId)
        {
            var keyCache = "ProductBusiness" + businessId;
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
            var StoreId = Globals.StoreCodeConfig.Vnpost;
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
        public string GetImageByColor(int id, string color)
        {
            //var client = new RestClient(Globals.VTAWebAPIPublicUrl);
            //var req = new RestRequest("/product-api/GetImageByColor?id=" + id + "&color=" + color, Method.GET) { RequestFormat = DataFormat.Json };

            //var rs = client.Execute<HttpContentResult<string>>(req).Data;

            //if (rs.Data == null && rs.StatusCode != "200")
            //    return null;
            //return rs.Data;

            var rs = _unitOfWork.Repository<P_ProductColor>().Queryable().FirstOrDefault(c => c.ProductId == id && c.ColorCode == color);

            return rs != null ? Globals.GetGeneratedImage(ImageUrl(rs.ColorImage), 300) : "";
        }
        public string ImageUrl(string url)
        {

            if (!url.Contains("~/Upload/"))
            {
                return "~/Upload/2015/1/2/" + url;
            }
            return url;
        }
        #endregion

        #region function public
        public List<HomeProductVM> GetHomeProductVM(List<ProductHomeModel> ListProduct, int skip, int take)
        {
            var rs = new List<HomeProductVM>();
            foreach (var itemCate in Globals.AllCategory)
            {
                var cate = itemCate;
                var data = ListProduct.Where(c => cate != null && c.RootCategoryId == Globals.GetCategoryId(cate.CategoryName)).Skip(skip).Take(take);
                var nameBanner = "home-category-" + cate.CategoryId;
                var banner = _bannerServices.GetBanner().FirstOrDefault(c => c.BannerInCategory == nameBanner);
                if (data.Any())
                {
                    rs.Add(new HomeProductVM()
                    {
                        CategoryName = itemCate.CategoryName,
                        ListProduct = data,
                        CategoryId = cate.CategoryId,
                        Banner = banner
                    });
                }
            }

            return rs;
        }
        private IEnumerable<ProductColor> GetColorProduct(long productId)
        {
            // TỐI ƯU LẠI SAU
            var colors = _unitOfWork.Repository<P_ProductColor>().Queryable().Where(c => c.Status == "A");
            var aBusinessProducts =
                _unitOfWork.Repository<A_Business_products>()
                    .Queryable()
                    .Where(c => c.Status == "A");
            var listBussinessCampaign = _unitOfWork.Repository<A_Business_campaign>().Queryable();
            var listBussiness = _unitOfWork.Repository<A_Business>().Queryable();
            var data = (from c in listBussinessCampaign
                        join b in listBussiness on c.Business_id equals b.Business_id
                        where b.Name.ToUpper().Equals("VNPOST")
                              && c.Status.Equals("A") && c.Date_start <= DateTime.Now && c.Date_end >= DateTime.Now
                        select c);
            var rs = (from a in aBusinessProducts
                      join c in data on a.Business_campaign_id equals c.Business_campaign_id
                      select a);
            var query = (from c in colors
                         join bp in rs on c.ProductId equals bp.Product_id
                         where c.ProductId == productId && c.ProductColorCode == bp.Product_color_code
                         select new ProductColor()
                         {
                             Thumbs = c.ColorImage,
                             ColorCode = c.ColorCode,
                             ColorId = c.ColorId,
                             ColorName = c.ColorName,
                             ProductColorCode = c.ProductColorCode,
                             Productid = c.ProductId
                         }
                          ).Distinct().ToList();

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

                var happycare = tblHappyCare.FirstOrDefault(c => c.MinPrice <= price && c.MaxPrice >= price && c.CategoryId == rsCategory);

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
            return rs != null ? rs.ProductId : 0;
        }

        public List<ImageModel> GetImageByProductId(int productId)
        {
            var rs = _unitOfWork.Repository<P_ProductImage>().Queryable().Where(c => c.ProductId == productId && !string.IsNullOrEmpty(c.Type))
                .Select(c => new ImageModel()
                {
                    ImageDesicription = c.ImageDescription,
                    ImageName = c.ImageName,
                    ImageUrl = c.ImageUrl,
                    Type = c.Type
                }).ToList();
            return rs;
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
        #region Thincheck dataCache

        public List<ProductHomeModel> GetCacheListProducts()
        {

            var keyCacheProductList = "AllProduct";
            var cacheValues = DataCache.GetCache<List<ProductHomeModel>>(keyCacheProductList);
            if (cacheValues == null)
            {

                cacheValues = ListAllProduct();
                DataCache.SetCache(keyCacheProductList, cacheValues,
                    DateTime.Now.Add(TimeSpan.FromMinutes(Globals.TimeCache)));

            }
            //NLog.LogManager.GetCurrentClassLogger()
            //    .Debug("GetCacheDataMenuRole-" + JsonHelper.SerializeObject(cacheValues));
            return cacheValues;
        }

        #endregion

    }
}