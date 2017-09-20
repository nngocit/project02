using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.Razor.Parser;
using VNPOST.Data;
//using VNPOST.Data.SqlContext;

namespace VNPOST.BackEnd.API.Models
{
    public class OrderModel : O_Order
    {
        public OrderModel()
        {
            Total = MoneyTotal;
            DiscountDetail = DiscountDesc;
            Discount = DiscountTotal;
            O_OrderDetail = ListOrder;


        }

        public OrderModel(PaymentPage model)
        {
           
            Sex = model.Gender;
            Name = model.Fullname;
            Address = model.Address;
            Phone = model.Phone;
            Email = model.Email;
            SystemSource = "WEB";
            PaymentType = model.PaymentType;
            RecievedType = model.RecieveType;
            CompanyId = Convert.ToInt32(model.DistrictId);
            OrderTypeId = model.TypeOrder;
            Status = 1;
            ListOrder = model.ListProduct;
            PaymentDeferred = new O_Payment_deferred()
            {
                BirthDay = model.BirthdayTime,
                Old = model.Old,
                Job = model.NgheNghiepName,
                Noio = model.CityNoiOName,
                HoKhau = model.CityHoKhauName,
                Unit = model.DonViTraGop,
                Tratruoc = model.PhanTramTraTruoc,
                Cmnd = model.Cmnd,
                District = model.DistrictName,
                Status = 18,
                Thoihanvay = model.ThoiGianTraGop,
                Sotientratruoc = model.Sotientratruoc,
                Sotientragop = model.Sotientragop,
                Note = model.GiayToCanCo
            };

        }
        public string StatusName { get; set; }
        private List<O_OrderDetail> _listOrder;

        public List<O_OrderDetail> ListOrder
        {
            get { return _listOrder ?? (_listOrder = new List<O_OrderDetail>()); }
            set { _listOrder = value; }
        }
        public string TuserId { get; set; }
        public decimal DiscountTotal
        {
            get { return ListOrder.Sum(x => x.Discount ?? 0); }
        }
        public decimal MoneyTotal
        {
            get { return ListOrder.Sum(x => x.Price * x.Amount); }
        }
        public string DiscountDesc
        {
            get { return String.Join(",", ListOrder.Select(x => x.DiscountCode)); }
        }
        public O_Payment_deferred PaymentDeferred { get; set; }


    }
    public class OrderParameter
    {
        public long OrderId { get; set; }
        public string UserId { get; set; }
        public string StoreCode { get; set; }
        public int OrderTypeId { get; set; }
        public long OrderSoId { get; set; }
        public int Status { get; set; }
        public string ShippingUserId { get; set; }
        public string Note { get; set; }
        public List<long> Listorder { get; set; }
        public int  FromOrder { get; set; }
        public int ToOrder { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public OrderParameter()
        {
            
        }

    }
    public class OrderDetailModel : O_OrderDetail
    {
        public string ColorId { get; set; }
        public string ColorName { get; set; }
        public string ProductName { get; set; }
     

    }
    #region OrderVta

    public class ShopOrderModel
    {
        /// <summary>
        /// tổng giá trị đơn hàng (subtotal+shipping_cost-discount)
        /// </summary>
        /// 
        public decimal total
        {
            get { return subtotal + shipping_cost - discount; }
            set { total = value; }
        }
        /// <summary>
        /// Tổng giá sản phẩm cho đơn hàng
        /// </summary>
        public decimal subtotal { get; set; }
        /// <summary>
        /// 6 (Nhận hàng tại nhà) hoặc 8 (Nhận hàng tại siêu thị)
        /// </summary>
        public int shipping_ids { get; set; }
        /// <summary>
        /// phí vận chuyển
        /// </summary>
        public decimal shipping_cost { get; set; }
        /// <summary>
        /// mã chi nhánh nhận hàng(nếu shiping_id=6 thì cho nó giá trị rỗng)
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// giảm giá
        /// </summary>
        public decimal discount { get; set; }
        /// <summary>
        /// mã giảm giá tai chinh
        /// </summary>
        public string coupon_code { get; set; }
        /// <summary>
        ///  mã hình thức thanh toán
        /// </summary>
        public string payment_id { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string phone { get; set; }
        public string sex { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string notes { get; set; }
        public string ip_address { get; set; }
        public int affiliate_branch_id { get; set; }
        //Ma 7 so
        public string confirm_code { get; set; }
        /// <summary>
        /// loại đơn hàng(mặc định là D)
        /// </summary>
        public string order_type { get; set; }
        public List<ShopOrderProductModel> product { get; set; }

        public ShopOrderModel(O_Order o, string confirmCode)
        {
            order_type = "V";
            subtotal = o.Total;
            shipping_ids = 8;
            shipping_cost = 0;
            area = o.StoreCode;
            discount = o.Discount;
            coupon_code = "";
            payment_id = "8";
            lastname = "";
            firstname = o.Name;
            phone = o.Phone;
            sex = o.Sex;
            address = o.Address ?? "";
            email = o.Email;
            notes = "Đơn hàng VnPost";
            ip_address = "10.10.40.171";
            affiliate_branch_id = 13;
            confirm_code = confirmCode;
        }
        public ShopOrderModel() { }
    }

    public class ShopOrderProductModel
    {
        public string product { get; set; }
        public int amount { get; set; }
        public string color { get; set; }
        public decimal price { get; set; }
        public int product_id { get; set; }
        public string product_code { get; set; }

        public ShopOrderProductModel(OrderDetailModel model)
        {
            product = model.ProductName;
            amount = model.Amount;
            color = model.ColorId;
            price = model.Price;
            product_id = (int)model.ProductId;
            product_code = model.ProductCode;
        }

    }
    #endregion
    #region APIGeneralCode

    public class GenerateCodeRequest
    {
        public string CampaignGroupName { get; set; }
        public string CampaignKey { get; set; }
        public string CampaignValue { get; set; }
        public string ApplierKey { get; set; }
        public string ApplierValue { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }

        public string FirstCharacter { get; set; }
        public string Status { get; set; }
        public string SystemSource { get; set; }
    }
    #endregion
}