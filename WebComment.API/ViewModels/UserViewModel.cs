using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebComment.API.ViewModels
{
    public class UserViewModel
    {
    }
    public class LoginModel
    {
        public bool status { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
    public class ChangePassModel
    {
        [Required]
        public string userId { get; set; }
        public string oldPassword { get; set; }
      [StringLength(30, MinimumLength = 6, ErrorMessage = "Không hợp lệ !!!")]
        public string usenewPasswordrId { get; set; }
    
    }
}