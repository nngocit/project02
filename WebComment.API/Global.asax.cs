using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebComment.API.CustomAttributes;
using WebComment.API.Helper;
using WebComment.API.Models;
using VTA.Service.CoreCache;
using WebComment.Data;

namespace WebComment.API
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //init db
            var db = new InitEntitiesDb();
            db.InitMainDatabase();
            

        }

        //protected void Application_BeginRequest()
        //{
        //    if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
        //    {
        //        Response.Flush();
        //    }
        //}

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            try
            {
                HttpContext.Current.Response.Headers.Remove("X-Powered-By");
                HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
                HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
                HttpContext.Current.Response.Headers.Remove("Server");
                HttpContext.Current.Response.Headers.Remove("Content-Security-Policy");
            
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        public static void RegisterWebApiFilters(HttpFilterCollection filters)
        {
            //filters.Add(new VerifySessionToken());
            //filters.Add(new VTALogRequestHandler());
            //filters.Add(new AuthorizedActionFilter());
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            //log error
            LogException(exception);
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;

            //ignore 404 HTTP errors
            var httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
                return;

            try
            {
                //log
                NLog.LogManager.GetCurrentClassLogger().Error(exc, exc.Message, null);
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }
       
       
    }
}
