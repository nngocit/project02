using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComment.API.DAL;
using WebComment.API.IServices;
using WebComment.API.Models;
using WebComment.Data;

namespace WebComment.API.Services
{
    public class BannerServices : IBannerServices
    {
        private IUnitOfWork _unitOfWork;
        private List<BannerModel> cachedListBanner = new List<BannerModel>();

        public BannerServices(IUnitOfWork unitOfWork)
        {
            //_unitOfWork = unitOfWork;
            _unitOfWork = new UnitOfWork(new SqlDbContext());
        }
        public List<BannerModel> GetBanner()
        {

            var P_Bannner = _unitOfWork.Repository<P_Banner>().Queryable();
            var campaign = _unitOfWork.Repository<A_Business_campaign>().Queryable();


            var rs = new List<BannerModel>();
            rs = (from b in P_Bannner
                  join c in campaign on b.CampaignId equals c.Business_campaign_id

                  where b.Status == "A" && c.Status == "A"
                  && b.StartDate <= DateTime.Now
                  && b.EndDate >= DateTime.Now
                  /*       && c.Date_start <= DateTime.Now
                         && c.Date_end >= DateTime.Now*/ // TAM IT BUA CHINH SUA LAI
                  && c.Name == "VNPOST"
                  select new BannerModel()
                  {
                      BannerName = b.Title,
                      BannerUrl = b.Url,
                      ImageUrl = b.Image,
                      SeoContent = b.Description,
                      FromDate = b.StartDate,
                      ToDate = b.EndDate,
                      BannerInCategory = b.Position,
                      PositionInBlock = b.ImagePosition,
                      BannerSize = b.ImageType
                  }).OrderBy(x => x.PositionInBlock).ToList();

            return rs;

        }
        public List<BannerModel> GetBannerFromSlideShow()
        {
            var BannerType = new List<string>() { "home-top-1", "home-top-2", "home-top-3" };
            return GetBanner().Where(c => BannerType.Contains(c.BannerInCategory)).ToList();
        }
        public List<BannerModel> GetBannerHomeCategory()
        {
            var ArrayBannerHome = new List<string>() { "home-category-152536", "home-category-171730", "home-category-152537", "home-category-152535" };
            return GetBanner().Where(c => ArrayBannerHome.Contains(c.BannerInCategory)).ToList();
        }
        public List<BannerModel> GetBannerCampain(string campaingnName)
        {
            var ArrayBannerHome = new List<string>() { "home-top-1", "home-top-2", "home-top-3", "home-category-152536", "home-category-171730", "home-category-152537", "home-category-152535" };
            return GetBanner().Where(c => ArrayBannerHome.Contains(c.BannerInCategory)).ToList();
        }

        public List<PositionModel> GetPosition()
        {
            var rs = _unitOfWork.Repository<P_Position>().Queryable().ToList().Select(c => new PositionModel(c));
            return rs.ToList();
        }

        public bool InsertBanner(PBanneModel model)
        {

            try
            {
                P_Banner data = new P_Banner()
                {
                    //Id = model.Id,
                    StartDate = model.StartDate,
                    Url = model.Url,
                    Image = model.Image,
                    Status = model.Status,
                    EndDate = model.EndDate,
                    Position = model.Position,
                    ImageType = model.ImageType,
                    ImagePosition = model.ImagePosition,
                    Description = model.Description,
                    Title = model.Title,
                    CampaignId = model.CampaignId,
                    ImageMobile = model.ImageMobile

                };
                _unitOfWork.Repository<P_Banner>().Insert(data);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug(" InsertBanner() - ex {0} giay ", ex.ToString());
                return false;
            }
        }
        public bool UpdateBanner(PBanneModel model)
        {
            try
            {
                P_Banner data = new P_Banner()
                {
                    Id = model.Id,
                    StartDate = model.StartDate,
                    Url = model.Url,
                    Image = model.Image,
                    Status = model.Status,
                    EndDate = model.EndDate,
                    Position = model.Position,
                    ImageType = model.ImageType,
                    ImagePosition = model.ImagePosition,
                    Description = model.Description,
                    Title = model.Title,
                    CampaignId = model.CampaignId,
                    ImageMobile = model.ImageMobile


                };
                _unitOfWork.Repository<P_Banner>().Update(data);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug(" UpdateBanner() - ex {0} giay ", ex.ToString());
                return false;
            }
        }
        public List<PBanneModel> GetListBanner()
        {
            var banner = _unitOfWork.Repository<P_Banner>().Queryable();
            var campaign = _unitOfWork.Repository<A_Business_campaign>().Queryable();
            var rs = (from model in banner
                      join c in campaign on model.CampaignId equals c.Business_campaign_id
                      /*   where 
                          model.StartDate <= DateTime.Now
                         && model.EndDate >= DateTime.Now*/
                      select new PBanneModel()
                      {
                          Id = model.Id,
                          Image = model.Image,
                          Url = model.Url,
                          StartDate = model.StartDate,
                          EndDate = model.EndDate,
                          Position = model.Position,
                          ImagePosition = model.ImagePosition,
                          ImageType = model.ImageType,
                          Title = model.Title,
                          Description = model.Description,
                          Status = model.Status,
                          CampaignId = model.CampaignId,
                          CampaignName = c.Name,
                          ImageMobile = model.ImageMobile
                      }
                ).OrderBy(x => x.Status).ToList();


            return rs.ToList();
        }

        public PBanneModel GetBannerDetail(int id)
        {
            var banner = _unitOfWork.Repository<P_Banner>().Queryable();

            var rs = (from model in banner
                      select new PBanneModel()
                      {
                          Id = model.Id,
                          Image = model.Image,
                          Url = model.Url,
                          StartDate = model.StartDate,
                          EndDate = model.EndDate,
                          Position = model.Position,
                          ImagePosition = model.ImagePosition,
                          ImageType = model.ImageType,
                          Title = model.Title,
                          Description = model.Description,
                          Status = model.Status,
                          CampaignId = model.CampaignId,
                          ImageMobile = model.ImageMobile

                      }
                ).FirstOrDefault(x => x.Id == id);


            return rs;
        }
        public List<PBanneModel> GetListBannerbyCampaignName(string campaignName)
        {

            var banner = _unitOfWork.Repository<P_Banner>().Queryable();
            var campaign = _unitOfWork.Repository<A_Business_campaign>().Queryable();
            var datalist = (from model in banner
                            join c in campaign on model.CampaignId equals c.Business_campaign_id
                            where
                             model.StartDate <= DateTime.Now
                            && model.EndDate >= DateTime.Now
                            select new PBanneModel()
                            {
                                Id = model.Id,
                                Image = model.Image,
                                Url = model.Url,
                                StartDate = model.StartDate,
                                EndDate = model.EndDate,
                                Position = model.Position,
                                ImagePosition = model.ImagePosition,
                                ImageType = model.ImageType,
                                Title = model.Title,
                                Description = model.Description,
                                Status = model.Status,
                                CampaignId = model.CampaignId,
                                CampaignName = c.Name,
                                ImageMobile = model.ImageMobile
                            }
                ).OrderBy(x => x.Status).ToList();
            var rs = datalist.Where(x => x.CampaignName.ToUpper() == campaignName.ToUpper()).ToList();
            return rs.ToList();
        }

    }
}