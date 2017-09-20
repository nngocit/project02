using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebComment.Commons
{
    public class Globals
    {
        public static readonly string FrontEndUrl = ConfigurationManager.AppSettings["FrontEndUrl"];
        public static readonly string VTAWebAPILocal = ConfigurationManager.AppSettings["VTAWebAPILocal"]; //Co Huynh add to run notification
        public static readonly string VTAWebAPIUrl = ConfigurationManager.AppSettings["VTAWebAPIUrl"];
        public static readonly string VTAWebOrderAPIUrl = ConfigurationManager.AppSettings["VTAWebOrderAPIUrl"];
        public static readonly string VTALoyaltyAPIUrl = ConfigurationManager.AppSettings["VTALoyaltyAPIUrl"];
        public static readonly string Email_Manager = ConfigurationManager.AppSettings["Email_Manager"];
        public static readonly string ExpireTime = ConfigurationManager.AppSettings["ExpireTime"];
        public static readonly string StoreCode = ConfigurationManager.AppSettings["PriceStoreDefault"];
        public static readonly string PriceStoreDefault = ConfigurationManager.AppSettings["PriceStoreDefault"];
        public static readonly string SMSCampaignCode = ConfigurationManager.AppSettings["SMSCampaignCode"];
        public static readonly string SMSPassword = ConfigurationManager.AppSettings["SMSPassword"];
        public static readonly string EnableSendMail = ConfigurationManager.AppSettings["EnableSendMail"];
        public static readonly string EnableSMS = ConfigurationManager.AppSettings["EnableSMS"];
        public static readonly string UrlApi = ConfigurationManager.AppSettings["UrlApi"];
        public static readonly string CookiePM = "VTAMobilePaymentCK";
        public static readonly string StoreCodePrivateSale = ConfigurationManager.AppSettings["PrivateSaleStore"];
        public static readonly List<string> ListControllerNameCheckPermisson = ConfigurationManager.AppSettings["ControllersCheckPermission"].Split(',').ToList();
       
        public static readonly ConnectionStringSettings SqlConnectionString = ConfigurationManager.ConnectionStrings["SqlDbContext"];

        public static string StaticUrl = ConfigurationManager.AppSettings["StaticUrl"];
        public static readonly int TimeCache = 15;
        public static readonly int TimeCacheUser = 120;

        public static readonly string VTAWebAPIPublicUrl = ConfigurationManager.AppSettings["VTAWebAPIPublicUrl"];
        public static readonly List<long> ListStatusTragopSO = new List<long>() { 18, 12, 14, 17 };
        public static readonly List<long> ListStatusTragopUnit = new List<long>() { 14, 17, 15, 19 };
        public static readonly List<string> ListTragopUnit = new List<string>() {"PPF" };
        public static readonly string VTAWebAdmincpWebComment = ConfigurationManager.AppSettings["VTAWebAdmincpWebComment"];
        public static readonly string VTAWebAdmincp = ConfigurationManager.AppSettings["AdmincpVtaUrl"];
      //  public static readonly string PricePrivateSale = ConfigurationManager.AppSettings["PricePrivateSale"];
        
        public class CoreCategoryConfig
        {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string CategoryUrl { get; set; }
        
        }
        #region Function
        public static bool ValidateEmail(string email)
        {
            return Validate(email, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        public static bool Validate(string text, string pattern)
        {
            Regex regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            return regex.IsMatch(text);
        }
        #endregion
        public static class StoreCodeConfig
        {
            public static readonly string SinhVien = "CS_0000241";
            public static readonly string HeadOffice = "CS_0000042";
            public static readonly string SaleOnline = "CS_0000188";
            public static readonly string Vnpost = "CS_0000337";
            public static readonly string Affiliate = "CS_0000275";
        }
        public static class BusinessConfig
        {
            public static readonly int Vnpost = 40;
            public static readonly string VnpostName = "Vnpost";
        }
        public static class CoreCategory
        {
            public static readonly List<long> ListProductId = new List<long>() { 
                184660, 184469, 184664, 184470, 184634, 183520, 184568, 184706, 184682, 184634, 
                184470, 184451, 184398, 184248, 184297, 184702, 184364, 184572, 183113, 183956,
                183182, 184469, 184332, 181097, 184389, 183778, 184050, 184424
            };

            public static readonly int Sanphammoi = 1;
            public static readonly int Sanphambanchay = 2;
        }

        public static readonly string SAN_PHAM_BAN_CHAY = "Sản Phẩm Bán Chạy";
        public static readonly string SAN_PHAM_MOI = "Sản Phẩm Mới";
        public static readonly string DIEN_THOAI = "Điện Thoại";
        public static readonly string MAY_TINH_BANG = "Máy Tính Bảng";
        public static readonly string LAPTOP = "Máy Tính Xách Tay";
        public static readonly string LINH_PHU_KIEN = "Linh Phụ Kiện";
        public static readonly string NOI_DUNG_SO = "Nội dung số";
        public static readonly string GIFT_CARD = "Gift Card";
        public static List<CoreCategoryConfig> AllCategory = new List<CoreCategoryConfig>()
        {
            new CoreCategoryConfig() { CategoryId = 152535, CategoryName = DIEN_THOAI, CategoryUrl = "dien-thoai-smartphones"},
            new CoreCategoryConfig() { CategoryId = 171730, CategoryName = MAY_TINH_BANG, CategoryUrl = "may-tinh-bang" },
            new CoreCategoryConfig() { CategoryId = 152537,  CategoryName = LAPTOP, CategoryUrl = "laptop"},
            new CoreCategoryConfig() { CategoryId = 152536, CategoryName = LINH_PHU_KIEN, CategoryUrl = "linh-phu-kien"},
            //new CoreCategoryConfig() { CategoryId = 152859, CategoryName = NOI_DUNG_SO, CategoryUrl = "noi-dung-so"},
            //new CoreCategoryConfig() { CategoryId = 180299, CategoryName = GIFT_CARD, CategoryUrl = "gift-card"},
            
        };
        public static string GetGeneratedImage(string originalImagePath, int imageSize, bool hasWaterMark = false)
        {
            return StaticHelper.ImageUrl(originalImagePath, imageSize, hasWaterMark);
        }
        public static string GetCategoryName(int categoryId)
        {
            var category = AllCategory.Find(c => c.CategoryId == categoryId);
            if (category != null)
                return category.CategoryName;
            return "";
        }
        public static string GetCategoryUrl(int categoryId)
        {
            var category = AllCategory.Find(c => c.CategoryId == categoryId);
            if (category != null)
                return category.CategoryUrl;
            return "";
        }

        public static int GetCategoryId(string nameCategory)
        {
            var category = AllCategory.Find(c => c.CategoryName == nameCategory);
            if (category != null)
                return category.CategoryId;
            return 0;
        }
        public static string FormatMoney(object money)
        {
            return Convert.ToInt32(money) > 0 ? string.Format("{0:0,0}{1}", money, "đ") : "0đ";
        }
        public enum OrderType
        {
            Online,
            TraGop
        }
        public static class MainCategory
        {
            public static string Smartphone = "152535";
            public static string LinhPhuKien = "152536";
            public static string Tablet = "171730";
            public static string Laptop = "152537";
            public static string GiftCard = "180299";
            public static string Noidungso = "152859";
            public static string TheCaoKCong = "180282";
        }
        public static class GiayToCanCo
        {
            public static string Fast = "CMND + Hộ khẩu hoặc Bằng lái (Duyệt nhanh, lãi suất cao)";
            public static string Slow = "CMND + Hộ khẩu + Hóa đơn điện nước (Lãi suất thấp)";
        }
        public enum PriceTraGopRange
        {
            HomeCredit = 1250000,
            HdFinance = 3000000,
            Asc = 3300000
        }
        public static class DonViTraGop
        {
            public static string HomeCredit = "PPF";
        }
        public enum MinPricePayment
        {
            MinPricePayment = 10000
        }
        public static class RecievedType
        {
            public static string VTA = "Tại chi nhánh WebComment";
            public static string TRANSFER = "Tại nhà";
        }
        public enum PaymentType
        {
            ThanhToanKhiNhanHang = 14,
            ThanhToanTraGop = 18,
            ChuyenKhoan = 16,
            ThanhToanOnline = 21
        }
        public static class PaymentMethod
        {
            public static string ATM = "123PAY";
            public static string VISA = "123PCC";
        }
        public enum ShippingType
        {
            NhanHangTrucTiep = 6,
            NhanTaiSieuThi = 8
        }
    }
}
