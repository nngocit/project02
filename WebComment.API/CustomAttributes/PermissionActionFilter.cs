using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using WebComment.API.DAL;
using WebComment.API.Helper;
using WebComment.API.Models;
using WebComment.Commons;
using WebGrease;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace WebComment.API.CustomAttributes
{
    public class PermissionActionFilter : ActionFilterAttribute
    {
        // private readonly IUserService _userService;

        public PermissionActionFilter()
        {
            // _userService = new UserService(new UnitOfWork(new SqlDbContext())); 
            //DependencyResolver.Current.GetService<IUserService>();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string currentActionName = string.Empty;
            string currentControllerName = string.Empty;
            var rd = actionContext.RequestContext.RouteData;
            try
            {
                currentActionName = rd.Values["action"].ToString().ToLower();
                currentControllerName = rd.Values["controller"].ToString().ToLower();
            }
            catch (Exception)
            {

            }

            if (Globals.ListControllerNameCheckPermisson.Contains(currentControllerName))
            {

                IEnumerable<string> headerValues;
                actionContext.Request.Headers.TryGetValues("SessionToken", out headerValues);
                if (headerValues == null)
                {
                    NLog.LogManager.GetCurrentClassLogger().Debug("++ headerValues = null");
                }
                else
                {
                    NLog.LogManager.GetCurrentClassLogger().Debug("++ headerValues != null, " + headerValues.FirstOrDefault());
                }
                var loginToken = headerValues != null ? headerValues.FirstOrDefault() : string.Empty;

                if (!UserCanDoByToken(loginToken, currentControllerName, currentActionName))
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        Content =
                            new StringContent(
                                JsonHelper.SerializeObject(new HttpContentResult<bool>
                                {
                                    Data = false,
                                    StatusCode = GlobalsEnum.GlobalStatus.ACCESS_DENIED.ToString(),
                                    Result = false,
                                    Message = "Bạn không có quyền truy cập"
                                }), Encoding.UTF8, "application/json"),
                        StatusCode = HttpStatusCode.OK

                    };
                }

            }
            //string currentAreaName = ((rd.Values["area"] as string) ?? "").ToLower();

        }

        private bool UserCanDoByToken(string token, string controllerName, string actionName)
        {

            controllerName = controllerName.ToLower();
            actionName = actionName.ToLower();


            var action =
                StaticDataHelper.GetCacheDataMenuRole()
                    .FirstOrDefault(x => x.Code.ToLower().Equals(actionName) && x.ControllerName.ToLower().Equals(controllerName));

            if (action == null)
            {

                var log = String.Format("UserCanDoByToken(): Token: {0}, controler {1}, action: {2}", token, controllerName, actionName);
                // NLog.LogManager.GetCurrentClassLogger().Debug(log);
                return true;
            }
            else
            {
                if (string.IsNullOrEmpty(token)) return false;
                var userCheck = StaticDataHelper.GetCacheDataUser().FirstOrDefault(x => x.SessionToken == token);
                //NLog.LogManager.GetCurrentClassLogger().Debug("UserCanDoByToken-userCheck:" + JsonHelper.SerializeObject(userCheck));



                if (userCheck != null)
                {
                    var allowRoles = "," + action.ListRole + ",";
                    //   NLog.LogManager.GetCurrentClassLogger().Debug("UserCanDoByToken-allowRoles" + JsonHelper.SerializeObject(allowRoles));
                    var rs = userCheck.Roles.Any(x => allowRoles.Contains("," + x.Code + ","));
                    return rs;
                }


            }

            return false;
        }

    }
}