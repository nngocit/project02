using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using WebComment.Data;

namespace WebComment.API.Models
{
    public class BusinessOrderModel : B_Order
    {
        public string StatusName { get; set; }
        public List<DM_CommonData> ListStatus { get; set; }
        public List<B_OrderDetail> ListDetails { get; set; }
        public List<BOrderDetailModel> ListOrderDetails { get; set; }

        public BusinessOrderModel()
        {
            
        }
        public BusinessOrderModel(B_Order order, string statusName)
        {
            OrderId = order.OrderId;
            UserId = order.UserId;
            Sex = order.Sex;
            Name = order.Name;
            Birthday = order.Birthday;
            Address = order.Address;
            AddressShipping = order.AddressShipping;
            Phone = order.Phone;
            Email = order.Email;
            Cmnd = order.Cmnd;
            TenCongty = order.TenCongty;
            MaNv = order.MaNv;
        
            OrderTypeId = order.OrderTypeId;
            Total = order.Total;
            Discount = order.Discount;
            DiscountDetail = order.DiscountDetail;
            Status = order.Status;
            CreatedDate = order.CreatedDate;
            Note = order.Note;
            PaymentType = order.PaymentType;
            DiachiHokhau = order.DiachiHokhau;
            NoioHientai = order.NoioHientai;
          
            StatusName = statusName;
            MaPhieuxuat = order.MaPhieuxuat;
            Sohoadon = order.Sohoadon;
            OrderPos = order.OrderPos;
            BusinessCode = order.BusinessCode;
            DistictCode = order.DistictCode;


        }
    }

    public class BusinessOrderDetailModel
    {
        public B_Order Order { get; set; }
        public List<B_OrderDetail> ListDetails { get; set; }
        public List<DM_CommonData> ListStatus { get; set; }
    }
    public class BOrderDetailModel:B_OrderDetail
    {
        public string ImageDetail { get; set; }
        public BOrderDetailModel(B_OrderDetail model, P_Product getProduct)
        {
            OrderDetailId  = model.OrderDetailId;
            OrderId = model.OrderId;
            ProductId = model.ProductId;

            ProductColorCode = model.ProductColorCode;
            ProductCode = model.ProductCode;
            Price = model.Price;

            Amount = model.Amount;
            IsMain = model.IsMain;
            ProductName = model.ProductName;

            ColorName = model.ColorName;
            ImageDetail = getProduct.ProductImage;
            Discount = model.Discount;
            DiscountCode = model.DiscountCode;
            ParentId = model.ParentId;
            Imei = model.Imei;

        }
    }

    public class BusinessProductModel:B_BusinessProducts
    {
   
        public string ProductName { get; set; }
        public string BusinessName { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }

        public long CategoryId { get; set; }
        public long RootCategoryId { get; set; }
        public string ImageDetail { get; set; }
        public long ColorId{ get; set; }
        public string Thumbs { get; set; }
  

    }

    public class TragopModel 
    {

        public string Unit { get; set; }
        public object Cauhinh { get; set; }
        public decimal PriceTragopRange { get; set; }
        public float LaisuatTragop { get; set; }

        public TragopModel(DM_Tragop model)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Unit = model.Unit;
            Cauhinh = jss.Deserialize<object>(model.Cauhinh);
            PriceTragopRange = model.PriceTraGopRange;
            LaisuatTragop = model.LaisuatTragop;
        }

    }

    public class ComboBoxModel
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }

    public class Tinhtragop
    {
        public decimal TienTratruoc { get; set; }
        public double TienConlai { get; set; }
        public double Tientragop { get; set; }

        public Tinhtragop(decimal tongtien, int phantramtra, int sothangtra,float laisuat)
        {
            TienTratruoc = Math.Round((tongtien * phantramtra / 100) / 1000) * 1000;
             var tienConlaiChualaisuat = Math.Round(tongtien - TienTratruoc);
            if (Math.Abs(laisuat - 1.0) <= 0)
            {
                TienConlai = (double) tienConlaiChualaisuat;
            }
            else
            {
                TienConlai = (double)tienConlaiChualaisuat + Math.Round((double)tienConlaiChualaisuat * laisuat * sothangtra / 100);
            }
         
             Tientragop = Math.Round(TienConlai / (sothangtra * 1000)) * 1000;

        }
    }
    public class ProvinceModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<DistrictModel> ListDistrict { get; set; }

        public ProvinceModel(DM_Province model)
        {
            Code = model.Code;
            Name = model.Name;
            Type = model.Type;
            ListDistrict = model.DM_District.Where(x => x.DM_Province.Code.Equals(Code)).Select(x => new DistrictModel(x)).ToList();
        }
    }
    public class DistrictModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public DistrictModel(DM_District model)
        {
            Code = model.Code;
            Name = model.Name;
            Type = model.Type;
            
        }
    }

}