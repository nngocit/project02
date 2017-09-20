using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComment.API.Models;

namespace WebComment.API.IServices
{
    public interface IBannerServices
    {
        List<BannerModel> GetBannerFromSlideShow();
        List<BannerModel> GetBanner();
        List<PositionModel> GetPosition();
        bool InsertBanner(PBanneModel model);
        bool UpdateBanner(PBanneModel model);
        List<PBanneModel> GetListBanner();
        List<BannerModel> GetBannerCampain(string campaingnName);
        List<PBanneModel> GetListBannerbyCampaignName(string campaignName);
        PBanneModel GetBannerDetail(int id);
    }
}