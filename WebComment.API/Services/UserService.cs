using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VTA.Service.CoreCache;
using WebComment.API.DAL;
using WebComment.API.Models;
using WebComment.API.ViewModels;
using WebComment.Commons;
using WebComment.Data;

namespace WebComment.API.Services
{

    public interface IUserService
    {
        VtaServiceResult<UserModel> CreateUser(UserModel model);
        
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static string roleAdmin = "ADMIN";

        public UserService(IUnitOfWork unitOfWork)
        {
            //_unitOfWork = unitOfWork;
            _unitOfWork = new UnitOfWork(new SqlDbContext());
        }


        private UserManager<User> _identityUserManager;

        public UserManager<User> IdentityUserManager
        {
            get
            {
                if (_identityUserManager == null)
                {
                    _identityUserManager = new UserManager<User>(new UserStore<User>(_unitOfWork.GetDbContext()));
                }
                return _identityUserManager;
            }
        }

      
        public VtaServiceResult<UserModel> CreateUser(UserModel model)
        {
            var result = new VtaServiceResult<UserModel>();
            var user = new User();
            var identityResult = IdentityUserManager.Create(model.ToEntity(user), model.Password);
            if (identityResult.Succeeded)
            {
                result.Successfully = true;
                result.Message = GlobalsEnum.GlobalStatus.SUCCESS.ToString();
                result.Data = new UserModel(user);
            }
            else
            {
                result.Message = string.Join(",", identityResult.Errors);
            }

            return result;
        }
    }
}
