using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebComment.API.IServices;

namespace WebComment.API.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IBannerServices _bannerServices;
        public HomeController(IHomeService homeService, IBannerServices bannerServices)
        {
            _homeService = homeService;
            _bannerServices = bannerServices;
        }
        #region view api
        public ActionResult Index()
        {
            //NLog.LogManager.GetCurrentClassLogger().Debug("TEST NLOG: ");  // đã ghi log OK 
            //return View();
            return Redirect("swagger/ui/index");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "Dev")]
        public ActionResult HomeProductMobis()
        {

            return View("HomeProductMobis");
        }

        [Authorize(Roles = "Dev")]
        public ActionResult ListProductMobis()
        {

            return View("ListProductMobis");
        }

        [Authorize(Roles = "Dev")]
        public ActionResult ProductDetail()
        {

            return View("ProductDetail");
        }
        [Authorize(Roles = "Dev")]
        public ActionResult ListCategory()
        {

            return View("ListCategory");
        }

        [Authorize(Roles = "Dev")]
        public ActionResult SearchProduct()
        {

            return View("SearchProduct");
        }

        [Authorize(Roles = "Dev")]
        public ActionResult StoreVta()
        {

            return View("StoreVta");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #endregion

        #region view web

        public ActionResult HomeSponsor()
        {
            return View("HomeSponsor");
        }

        public ActionResult HomeSlideshow()
        {
            var model = _bannerServices.GetBannerFromSlideShow();
            return View("HomeSlideshow", model);
        }

        [OutputCache(Duration = 550)]
        public ActionResult HomeProduct()
        {
            return View();
        }

        public ActionResult HomeProductPage()
        {
            var model = _homeService.HomeProductVM();
            return View("HomeProductPage", model);
        }

        public ActionResult Sanphammoi()
        {
            var model = _homeService.HomeSanphammoi().ToList();
            return View("Partial/PartialHomeSponsor", model);
        }

        public ActionResult Sanphambanchay()
        {
            var model = _homeService.HomeSanphambanchay().ToList();
            return View("Partial/PartialHomeSponsor", model);
        }


        [OutputCache(Duration = 550)]
        public ActionResult SearchHome(string keyWord)
        {
            var model = _homeService.SearchProduct(keyWord).ToList();
            return View("Partial/PartialSearch", model);
        }
        #endregion
    }
}