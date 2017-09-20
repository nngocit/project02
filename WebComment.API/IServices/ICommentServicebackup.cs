using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComment.API.Models;
using WebComment.Data;

namespace WebComment.API.IServices
{
    public interface ICommentService
    {
        AllConfigData GetAllConfigData();
        bool PostComment(CommentDataPost comment);
        long PostComment2(CommentDataPost comment);
        bool UpdateComment(long commentId, UpdateCommentDataPost comment);
        bool UpdateUser(string maNV, UpdateUserDataPost userInfo);
        bool DeleteComment(long commentId);
        string AutotoolProcess();
        bool VTAReplyComment(NVReplyCommentDataPost comment);
        bool VTAReplyCommentV2(NVReplyCommentDataPost comment);
        bool UpdateReplyComment(long replyId, UpdateReplyCommentDataPost comment);
        List<SearchCommentResult> SearchComment(SearchCommentParams param);
        SearchCommentResultV2 SearchCommentV2(SearchCommentParamsV2 param, bool isReport = false);
        List<ReportCommentResult> XuatReportAdmin(SearchCommentParamsV2 param);
        CommentDetailAdmin GetCommentDetail(long Id);
        CommentDetailAdmin GetCommentDetailVer2(long Id);
        CommentDetailAdmin GetListAllCommentDetail(long Id);
        bool ResetData(ResetDataParam phongBan);

        List<CM_UserLoginHistory> GetListLogInHistory(string MaNV);
        CommentVtaSite GetListCommentVtaSite(string url);
        CommentVtaSite GetListCommentVtaSiteV2(string pageDG, string pageBL, string url);
        CheckRepliedReponse CheckRepliedCommentVtaSite(string lstCommentId);
        List<EmployeeInfoModel> SearchEmployee(string maNhanVien);
        string RegisterUser(RegisterEmployeeInfo empInfo);
        List<SearchUserResult> SearchUser(SearchUserParams param);
        bool InsertLoginHistory(UserLoginHistoryDataPost history);
      
    }
}