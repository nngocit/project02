using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using WebComment.Data;

namespace WebComment.API.Models
{
    public class BannerModel
    {
        public int BannerId { get; set; }
        public string BannerUrl { get; set; }
        public string ImageUrl { get; set; }
        public string SeoContent { get; set; }
        public string BannerName { get; set; }

        /// <summary>
        /// home-top-1 | home-top-2 | home-top-3 | home-top-4  #dành cho banner ở top slideshow ở trang Home
        /// home-category   #banner nằm trong từng ngành hàng ở trang home
        /// home-menu  #banner nằm trong menu - theo ngành hàng hàng chính hoặc ngành hàng con
        /// category-top-1 | category-top-2    # dành cho banner ở vị trí top trong trang danh mục sản phẩm (hoặc search sản phẩm)
        /// category-block  #dành cho banner nằm ở trong block ở trang danh mục sản phẩm
        /// </summary>
        public string PositionOnPage { get; set; }

        /// <summary>
        /// Cho những banner nằm xem kẽ trong danh sách sản phẩm
        /// </summary>
        public int PositionInBlock { get; set; }  //Vị trí của banner sau sản phẩm thứ (0: đứng đầu tiên)

        public string BannerInCategory { get; set; }  //danh mục sản phẩm mà banner hiển thị

        public int BannerSize { get; set; }   // 1: default,  2: 2 cột 1 hàng,  3: 1 cột 2 hàng, 4: 2 cột 2 hàng

        public DateTime FromDate { get; set; }  //ngày hiển thị

        public DateTime ToDate { get; set; }  // ngày kết thúc

        public int DisplayStatus { get; set; }  //ẩn hiện thủ công:   0: ẩn, 1: hiện, 2: khác  //status

        public string BannerSizeCss
        {
            get
            {
                string css = "";
                if (BannerSize == 2)
                {
                    css = "wide";
                }
                else if (BannerSize == 3)
                {
                    css = "tall";
                }
                else if (BannerSize == 4)
                {
                    css = "wide-tall";
                }
                return css;
            }
        }
    }

    public class PositionModel
    {
        public string Position { get; set; }
        public string PositionName { get; set; }
        public PositionModel(P_Position data)
        {
            Position = data.Position;
            PositionName = data.PositionName;
        }
    }

    public class PBanneModel
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string ImageMobile { get; set; }
        
        public string Url { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Position { get; set; }
        public int ImagePosition { get; set; }
        public int ImageType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
       
        public PBanneModel()
        {
            

        }
        public PBanneModel(P_Banner model)
        {
            Id = model.Id;
            Image = model.Image;
            Url = model.Url;
            StartDate = model.StartDate;
            EndDate = model.EndDate;
            Position = model.Position;
            ImagePosition = model.ImagePosition;
            ImageType = model.ImageType;
            Title = model.Title;
            Description = model.Description;
            Status = model.Status;
            CampaignId = model.CampaignId;
            ImageMobile = model.ImageMobile;
        }
    }
}
