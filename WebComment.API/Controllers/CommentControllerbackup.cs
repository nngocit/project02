using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebComment.API.IServices;
using WebComment.API.Models;
using WebComment.Commons;

namespace WebComment.API.Controllers
{
    /// <summary>
    /// CommentController
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("comments")]
    public class CommentController : BaseApiController
    {
        private readonly ICommentService _commentServices;

        /// <summary>
        /// Initial CommentController
        /// </summary>
        /// <param name="commentService"></param>
        public CommentController(ICommentService commentService)
        {
            _commentServices = commentService;
        }

        /// <summary>
        /// Search comment based on params inputed on AdminCp filter
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Search([FromUri]SearchCommentParams param)
        {
            if (param == null)
                param = new SearchCommentParams();
            try
            {
                var data = _commentServices.SearchComment(param);
                if (!data.Any())
                    return Ok(GlobalsEnum.GlobalStatus.NOT_FOUND.ToString());
                return Ok(data);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("Search(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        /// <summary>
        /// Search comment Version 2, include pagination based on params inputed on AdminCp filter
        /// </summary>
        [HttpGet]
        [Route("searchv2")]
        public IHttpActionResult SearchV2([FromUri]SearchCommentParamsV2 param)
        {
            if (param == null)
                param = new SearchCommentParamsV2();
            try
            {
                var data = _commentServices.SearchCommentV2(param, false);
                if (data != null)
                    return Ok(data);
                return Ok(GlobalsEnum.GlobalStatus.NOT_FOUND.ToString());
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("Search(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        /// <summary>
        /// Xuat Excel report, based on params inputed on AdminCp filter
        /// </summary>
        [HttpGet]
        [Route("report")]
        public IHttpActionResult XuatReportAdmin([FromUri]SearchCommentParamsV2 param)
        {
            if (param == null)
                param = new SearchCommentParamsV2();
            try
            {
                var data = _commentServices.XuatReportAdmin(param);
                if (data != null)
                    return Ok(data);
                return Ok(GlobalsEnum.GlobalStatus.NOT_FOUND.ToString());
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("XuatReportAdmin(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        /// <summary>
        /// Reset data chia Comment
        /// </summary>
        [HttpGet]
        [Route("reset")]
        public IHttpActionResult ResetDataChiaComment([FromUri]ResetDataParam param)
        {
            if (param == null)
                param = new ResetDataParam();
            try
            {
                var data = _commentServices.ResetData(param);
                if (data != null)
                    return Ok(data);
                return Ok(GlobalsEnum.GlobalStatus.NOT_FOUND.ToString());
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("ResetDataChiaComment(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        /// <summary>
        /// Create comment from VTAWeb
        /// </summary>
        /// <returns>commentId has created</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateComment(CommentDataPost comment)
        {
            try
            {
                var result = _commentServices.PostComment2(comment);
                if (result != 0)
                {
                    TimeSpan start = new TimeSpan(8, 0, 0); //10 o'clock
                    TimeSpan end = new TimeSpan(22, 0, 0); //12 o'clock
                    TimeSpan now = DateTime.Now.TimeOfDay;

                    if ((now > start) && (now < end))
                    {
                        return Ok(result.ToString() + "-1");
                    }
                    return Ok(result.ToString() + "-2");
                }
                //if (result != 0)
                //{
                //    return Ok(result.ToString());
                //}
                else
                    return Ok(GlobalsEnum.GlobalStatus.FAILED.ToString());

            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("CreateComment(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        /// <summary>
        /// Update comment (use for AdminCP)
        /// </summary>
        [HttpPut, HttpPatch]
        [Route("{commentId}")]
        public IHttpActionResult UpdateComment(long commentId, UpdateCommentDataPost comment)
        {
            try
            {
                var result = _commentServices.UpdateComment(commentId, comment);
                if (result)
                    return Ok(GlobalsEnum.GlobalStatus.SUCCESS.ToString());

                return Ok(GlobalsEnum.GlobalStatus.FAILED.ToString());

            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("UpdateComment(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        /// <summary>
        /// Delete Comment (use for AdminCP)
        /// </summary>
        [HttpDelete]
        [Route("{commentId}")]
        public IHttpActionResult DeleteComment(long commentId)
        {
            try
            {
                var result = _commentServices.DeleteComment(commentId);
                if (result)
                    return Ok(GlobalsEnum.GlobalStatus.SUCCESS.ToString());

                return Ok(GlobalsEnum.GlobalStatus.FAILED.ToString());

            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("DeleteComment(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        /// <summary>
        /// Get comment details based on comment Id 
        /// </summary>
        [HttpGet]
        [Route("{commentId}")]
        public IHttpActionResult GetCommentDetail(long commentId)
        {
            try
            {
                var data = _commentServices.GetCommentDetail(commentId);
                if (data == null)
                    return Ok(GlobalsEnum.GlobalStatus.NOT_FOUND.ToString());
                return Ok(data);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetCommentDetail(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }
        /// <summary>
        /// Get comment Ver2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("detailv2/{commentId}")]
        public IHttpActionResult GetCommentDetailVer2(long commentId)
        {
            try
            {
                var data = _commentServices.GetCommentDetailVer2(commentId);
                if (data == null)
                    return Ok(GlobalsEnum.GlobalStatus.NOT_FOUND.ToString());
                return Ok(data);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetCommentDetailVer2(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }
        /// <summary>
        /// Get comment Ver2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Alldetailv2/{commentId}")]
        public IHttpActionResult GetAllCommentDetailVer2(long commentId)
        {
            try
            {
                var data = _commentServices.GetListAllCommentDetail(commentId);
                if (data == null)
                    return Ok(GlobalsEnum.GlobalStatus.NOT_FOUND.ToString());
                return Ok(data);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetListAllCommentDetailVer2(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        /// <summary>
        /// Get all configuration info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("configs")]
        public IHttpActionResult GetAllConfigData()
        {
            try
            {
                var data = _commentServices.GetAllConfigData();
                return Ok(data);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetAllConfigData(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        /// <summary>
        /// Create Reply comment (Nhan vien VTA)
        /// </summary>
        [HttpPost]
        [Route("reply")]
        public IHttpActionResult CreateReply(NVReplyCommentDataPost reply)
        {
            try
            {
                var result = _commentServices.VTAReplyComment(reply);
                if (result)
                    return Ok(GlobalsEnum.GlobalStatus.SUCCESS.ToString());

                return Ok(GlobalsEnum.GlobalStatus.FAILED.ToString());
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("CreateReply(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }
        }
        /// <summary>
        /// Create Reply comment (Nhan vien VTA)
        /// </summary>
        [HttpPost]
        [Route("replyv2")]
        public IHttpActionResult CreateReplyV2(NVReplyCommentDataPost reply)
        {
            try
            {
                var result = _commentServices.VTAReplyCommentV2(reply);
                if (result)
                    return Ok(GlobalsEnum.GlobalStatus.SUCCESS.ToString());

                return Ok(GlobalsEnum.GlobalStatus.FAILED.ToString());
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("CreateReply(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }
        }


        /// <summary>
        /// Update a Reply comment
        /// </summary>
        [HttpPut, HttpPatch]
        [Route("reply/{replyId}")]
        public IHttpActionResult UpdateReply(long replyId, UpdateReplyCommentDataPost reply)
        {
            try
            {
                var result = _commentServices.UpdateReplyComment(replyId, reply);
                if (result)
                    return Ok(GlobalsEnum.GlobalStatus.SUCCESS.ToString());

                return Ok(GlobalsEnum.GlobalStatus.FAILED.ToString());
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("UpdateReply(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }
        }

        /// <summary>
        /// Get list comments for displaying in main VTA website
        /// </summary>
        [HttpGet]
        [Route("web/{lastSlug}")]
        public IHttpActionResult GetListCommentVtaSite(string lastSlug)
        {
            try
            {
                lastSlug = HttpUtility.UrlDecode(lastSlug);
                var data = _commentServices.GetListCommentVtaSite(lastSlug);
                if (data == null)
                    return Ok(GlobalsEnum.GlobalStatus.NOT_FOUND.ToString());
                return Ok(data);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetListCommentVtaSite(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }
        }

        /// <summary>
        /// Get list comments for displaying in main VTA website
        /// </summary>
        [HttpGet]
        [Route("web/{pageDG}/{pageBL}/{lastSlug}/")]
        public IHttpActionResult GetListCommentVtaSiteV2(string pageDG, string pageBL, string lastSlug)
        {
            try
            {
                lastSlug = HttpUtility.UrlDecode(lastSlug);
                var data = _commentServices.GetListCommentVtaSiteV2(pageDG, pageBL, lastSlug);
                if (data == null)
                    return Ok(GlobalsEnum.GlobalStatus.NOT_FOUND.ToString());
                return Ok(data);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetListCommentVtaSite(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }
        }

        /// <summary>
        ///Search employee by MaNV
        /// </summary>
        [HttpGet]
        [Route("user/{term}")]
        public IHttpActionResult SearchEmployee(string term)
        {
            try
            {
                var data = _commentServices.SearchEmployee(term);
                if (data == null)
                    return Ok(GlobalsEnum.GlobalStatus.NOT_FOUND.ToString());
                return Ok(data);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("SearchEmployee(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }
        }

        /// <summary>
        ///Search employee by MaNV
        /// </summary>
        [HttpPost]
        [Route("user")]
        public IHttpActionResult CreateEmployee(RegisterEmployeeInfo empInfo)
        {
            try
            {
                var data = _commentServices.RegisterUser(empInfo);
                if (data.Contains("SUCCESS:"))
                    return Ok(GlobalsEnum.GlobalStatus.SUCCESS.ToString());
                return Ok(GlobalsEnum.RegisterUserStatus.USER_EXISTED.ToString());
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("CreateEmployee(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }
        }

        /// <summary>
        /// Search user based on params inputed on AdminCp filter
        /// </summary>
        [HttpGet]
        [Route("users")]
        public IHttpActionResult SearchUser([FromUri]SearchUserParams param)
        {
            if (param == null)
                param = new SearchUserParams();
            try
            {
                var data = _commentServices.SearchUser(param);
                if (!data.Any())
                    return Ok(GlobalsEnum.GlobalStatus.NOT_FOUND.ToString());
                return Ok(data);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("Search(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        /// <summary>
        /// Update user information from Admin page
        /// </summary> 
        [HttpPut, HttpPatch]
        [Route("user/{maNV}")]
        public IHttpActionResult UpdateUser(string maNV, UpdateUserDataPost userInfo)
        {
            try
            {
                var result = _commentServices.UpdateUser(maNV, userInfo);
                if (result)
                    return Ok(GlobalsEnum.GlobalStatus.SUCCESS.ToString());

                return Ok(GlobalsEnum.GlobalStatus.FAILED.ToString());

            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("UpdateComment(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        // <summary>
        /// Log logIn and logOut time for each user
        /// </summary> 
        [HttpPost]
        [Route("history")]
        public IHttpActionResult InsertLoginHistory(UserLoginHistoryDataPost history)
        {
            try
            {
                var result = _commentServices.InsertLoginHistory(history);
                if (result)
                    return Ok(GlobalsEnum.GlobalStatus.SUCCESS.ToString());

                return Ok(GlobalsEnum.GlobalStatus.FAILED.ToString());

            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("InsertLoginHistory(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        // <summary>
        /// Get log in - log out information for employee - by MaNV
        /// </summary> 
        [HttpGet]
        [Route("history")]
        public IHttpActionResult GetLoginHistory(string MaNV)
        {
            try
            {
                var result = _commentServices.GetListLogInHistory(MaNV);
                if (result != null)
                    return Ok(result);

                return Ok(GlobalsEnum.GlobalStatus.FAILED.ToString());

            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("UpdateComment(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        // <summary>
        /// Check the comment has replied or not
        /// </summary> 
        [HttpGet]
        [Route("web/checkreplied/{lstCommentId}")]
        public IHttpActionResult WebCheckReplied(string lstCommentId)
        {
            try
            {
                var result = _commentServices.CheckRepliedCommentVtaSite(lstCommentId);
                if (result != null)
                    return Ok(result);

                return Ok(GlobalsEnum.GlobalStatus.INVALID_DATA);

            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("WebCheckReplied(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }

        // <summary>
        /// Autotool assign list comment for available users
        /// </summary> 
        [HttpGet]
        [Route("autotool")]
        public IHttpActionResult AutotoolProcess()
        {
            try
            {
                var result = _commentServices.AutotoolProcess();

                if (result.Contains("ERROR"))
                    return NoOK(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("AutotoolProcess(): {0} - {1}" + ex.Message, ex.StackTrace);
                return NoOK(GlobalsEnum.GlobalStatus.INVALID_DATA);
            }

        }
    }
}