using System;
using System.Collections.Generic;
using WebComment.Data;


namespace WebComment.API.ViewModels
{
    public class ProductViewModel
    {
        public string Id { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string Price { get; set; }

        public ProductViewModel()
        {
        }

        //baotnq: code thêm ở đây
        public ProductViewModel(P_Product model)
        {
            Id = model.ProductId.ToString();
            ProductCode = model.ProductCode;
            ProductName = model.ProductName;
            Price = model.PriceExpected.ToString();
        }
    }

    public class ProductDetailModels
    {
        public long ProductId { get; set; }
        public IEnumerable<ProductColor> ColorList { get; set; }// danh sach mau san pham
        public HappyCareModel HappyCare { get; set; }
    }
    public class ProductMobi
    {
        public ProductMobi() { }
        public long ProductId { get; set; }
        public long CategoryId { get; set; }
        public long RootCategoryId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ImageDetail { get; set; } // Image chinh san pham
        public decimal PriceHeadOffice { get; set; }
        public decimal PriceUnderHeadOffice { get; set; }
        public decimal PriceOnline { get; set; }
        public decimal PriceUnderOnline { get; set; }
        public decimal PriceExpected { get; set; }
        public string StatusExist { get; set; }// Tinh trang con hang het hang
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
    /// <summary>
    /// Thông tin một sản phẩm
    /// </summary>
    public class ProductModel
    {
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
        public string RootCategoryId { get; set; }
        public string ProductName { get; set; }
        public string ImageDetail { get; set; } // Image chinh san pham
        public string ProductCode { get; set; }
        public decimal PriceHeadOffice { get; set; }
        public decimal PriceOnline { get; set; }
        public decimal PriceExpected { get; set; }
        public int Amount { get; set; } // so luong san pham
        public string DescriptionHtml { get; set; }// chi tiet noi bat
        public string DescriptionHtmlFull { get; set; }// chi tiet full
        public string ProductAttach { get; set; }// Thong tin bo san pham chuan
        public string ImageAttach { get; set; }// Image bo san pham chuan
        public string StatusExist { get; set; }// Tinh trang con hang het hang
        public string VideoId { get; set; }
        public IEnumerable<ProductColor> ColorList { get; set; }// danh sach mau san pham
        public IEnumerable<ProductFeatureInfo> ListProductFeatureInfo { get; set; } // thong so ky thuat san pham
        public IEnumerable<ImageProductModel> ListImage { get; set; } // danh sach tat ca image phan loai theo TypeImage
        public HappyCareModel HappyCare { get; set; }
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
                        return InventoryStatusByAmount;
                    default:
                        return "[Liên hệ]";
                }
            }
        }
        public string InventoryStatusByAmount
        {
            get
            {
                var InventoryStatus = "Còn hàng";
                //if (IsCategory(Globals.MAY_TINH_BANG) || IsCategory(Globals.LAPTOP))
                //{
                //    if (_model.Amount < 2)
                //    {
                //        InventoryStatus = "Liên hệ";
                //    }
                //    else if (_model.Amount < 5)
                //    {
                //        InventoryStatus = "Hàng còn ít";
                //    }
                //}
                //else
                //{
                //    if (_model.Amount < 5)
                //    {
                //        InventoryStatus = "Liên hệ";
                //    }
                //    else if (_model.Amount < 10)
                //    {
                //        InventoryStatus = "Hàng còn ít";
                //    }
                //}
                return InventoryStatus;
            }
        }
    }
    public class ProductColor
    {
        public long ColorId { get; set; }
        public string ColorName { get; set; }
        public string ProductColorCode { get; set; }
        public string ColorCode { get; set; }
        public string Thumbs { get; set; }
        public long Productid { get; set; }
        public string ImageUrl
        {
            get
            {
               if(!string.IsNullOrEmpty(Thumbs))
                {
                      if (!Thumbs.Contains("~/Upload/"))
                {
                    return "~/Upload/2015/1/2/" + Thumbs;
                }

             
                }

               return Thumbs;
            }
        }
    }
    public class ProductFeatureInfo
    {
        public int ProductId { get; set; }
        public int FeatureId { get; set; }
        public string ProductFeatureDescription { get; set; }
        public int VariantId { get; set; }
        public string Value { get; set; }
        public string ProductFeatureVariantDescription { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; }
    }

    public class ImageProductModel
    {
        public string ImagePath { get; set; }
        public string TypeImage { get; set; }
        public string DescriptionDetails { get; set; }
    }
    public class HappyCareModel
    {
        public HappyCareDetailModel ghbh { get; set; }
        public HappyCareDetailModel bvtd { get; set; }

        public HappyCareModel()
        {
        }

        public HappyCareModel(HappyCareDetailModel modelGHBH, HappyCareDetailModel modelBVTD)
        {
            ghbh = modelGHBH;
            bvtd = modelBVTD;
        }
    }
    public class HappyCareDetailModel
    {
        public string product_id { get; set; }
        public int price { get; set; }
        public string HappyCareName { get; set; }
        public string ProductColorCode { get; set; }
        public int ParentId { get; set; }

    }

  

    public class CategoryModel
    {
        public long CategoryId { get; set; }
        public Nullable<long> RootCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoKeyword { get; set; }
        public CategoryModel(){}
        public CategoryModel(P_Category model)
        {
            CategoryId = model.CategoryId;
            RootCategoryId = model.ParentId;
            CategoryName = model.CategoryName;
            SeoDescription = model.SeoDescription;
            SeoTitle = model.SeoTitle;
            SeoKeyword = model.SeoKeyword;
        }
    }

    public class CheckPriceByListCodeInfo
    {
        public string ProductCode { get; set; }
        public string StoreCode { get; set; }
        public decimal PriceOnline { get; set; }
        public decimal PriceGachOnline { get; set; }
        public decimal PriceChiNhanh { get; set; }
        public decimal PriceGachChiNhanh { get; set; }
    }
    public class PriceModel
    {
        // gia chi nhanh
        public string StoreCode { get; set; }
        public string Productcode { get; set; }

        public decimal GiaChiNhanh { get; set; }   //list_price
        public decimal GiaWebSieuthi { get; set; }  //GiaWebST 
        public decimal GiaNiemYet { get; set; }   //GiaNiemYet

        public decimal GiaHeadOffice { get; set; }   //list_price
        public decimal GiaWebSieuthiHeadOffice { get; set; }  //GiaWebST 
        public decimal GiaNiemYetHeadOffice { get; set; }   //GiaNiemYet


        //tuong ung StoreCode = "CS_0000188" 
        //public decimal GiaThucOnline { get; set; }  //list_price
        //public decimal GiaOnline { get; set; }    // price
        //public decimal GiaGachOnline { get; set; }  //GiaWebST
    }
    public class ProductCodeModel
    {
        public List<string> productCodes { get; set; }
        public string StoreCode { get; set; }
    }

    public class PriceParamModel
    {
        public List<string> ProductCodes;
        public string StoreCode;
    }

    public class SponsorProductModel
    {
        public int SponsorId { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

    }
}