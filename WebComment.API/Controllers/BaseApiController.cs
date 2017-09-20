using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebComment.API.Enums;
using WebComment.Commons;

namespace WebComment.API.Controllers
{
    /// <summary>
    /// BaseApiController
    /// </summary>
    public class BaseApiController : ApiController
    {
        protected StatusCodeResult NoContext()
        {
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected IHttpActionResult InvalidRequest()
        {
            var x = new ResponseMessageResult(
                Request.CreateErrorResponse(
                    (HttpStatusCode)400,
                    new HttpError(GlobalsEnum.GlobalStatus.INVALID_DATA.ToString())
                )

            );

            x.Response.Headers.Add(WebCommentApiHeaders.X_Status, GlobalsEnum.GlobalStatus.INVALID_DATA.ToString());
            return x;
        }

        protected IHttpActionResult Pagination<T>(T data, int totalItems = 0, int limit = 20, int page = 1)
        {
            var x = new ResponseMessageResult(
                Request.CreateResponse(HttpStatusCode.OK, data)
            );

            int totalPage = totalItems / limit;
            totalPage = totalItems % limit == 0 ? totalPage : (totalPage + 1);

            x.Response.Headers.Add(WebCommentApiHeaders.X_Status, HttpStatusCode.OK.ToString());
            x.Response.Headers.Add(WebCommentApiHeaders.X_Paging_TotalItems, totalItems.ToString());
            x.Response.Headers.Add(WebCommentApiHeaders.X_Paging_Limit, limit.ToString());
            x.Response.Headers.Add(WebCommentApiHeaders.X_Paging_Count, totalPage.ToString());
            x.Response.Headers.Add(WebCommentApiHeaders.X_Paging_Page, page.ToString());
            x.Response.Headers.Add(WebCommentApiHeaders.X_Status, HttpStatusCode.OK.ToString());
            return x;
        }

        protected IHttpActionResult NoOK()
        {
            return NoOK("Something goes wrong");
        }

        protected IHttpActionResult NoOK(GlobalsEnum.GlobalStatus statusCode)
        {
            return NoOK(statusCode.ToString());
        }

        protected IHttpActionResult NoOK(string statusCode)
        {
            var x = new ResponseMessageResult(
                Request.CreateErrorResponse(
                    (HttpStatusCode)422,
                    new HttpError(statusCode)
                )

            );

            x.Response.Headers.Add(WebCommentApiHeaders.X_Status, statusCode);
            return x;
        }



    }
}