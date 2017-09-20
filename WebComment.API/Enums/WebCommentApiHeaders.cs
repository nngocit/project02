using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComment.API.Enums
{
    public class WebCommentApiHeaders
    {
        public static string X_Paging_TotalItems = "X-Pagination-TotalItems";
        public static string X_Paging_Count = "X-Pagination-Count";
        public static string X_Paging_Page = "X-Pagination-Page";
        public static string X_Paging_Limit = "X-Pagination-Limit";
        public static string X_Paging_Next = "X-Pagination-Next";
        public static string X_Paging_Prev = "X-Pagination-Prev";
        public static string X_Paging_First = "X-Pagination-First";
        public static string X_Paging_Last = "X-Pagination-Last";
        public static string X_Status = "X-EW-Status";
        public static string X_Errors = "X-EW-Errors";
        public static string X_Message = "X-EW-Message";
        public static string X_Error_Message = "X-EW-Error-Messages";
    }
}