using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComment.API.Models
{
    public class HttpContentResult<T>
    {
        public bool Result { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public string SysMessage { get; set; }
        public T Data { get; set; }

    }
}