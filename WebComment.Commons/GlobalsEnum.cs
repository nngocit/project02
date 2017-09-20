using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebComment.Commons
{
    public class GlobalsEnum
    {
        public enum GlobalStatus
        {
            SUCCESS,
            FAILED,
            INVALID_DATA,
            ACCESS_DENIED,
            NOT_FOUND
        }
      
        public enum UserStatus
        {
            NotActiveYet,
            Active,
            Lock,
            Deleted
        }

        public enum RegisterUserStatus
        {
            SUCCESS,
            FAILED,
            INVALID_DATA,
            USER_EXISTED
        }
    }
}
