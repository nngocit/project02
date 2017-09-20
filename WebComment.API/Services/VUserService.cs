using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VNPOST.BackEnd.API.DAL;
using VNPOST.BackEnd.API.Models;
using VNPOST.BackEnd.API.ViewModels;
using VNPOST.Commons;
using VNPOST.Data;

namespace VNPOST.BackEnd.API.Services
{
    
    public class VUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlDbContext _mainDbContext;

        public VUserService()
        {
            _mainDbContext = new SqlDbContext();
        }
        public VUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mainDbContext = new SqlDbContext();
        }

        #region properties
        private UserManager<User> _identityUserManager;
        public UserManager<User> IdentityUserManager
        {
            get
            {
                if (_identityUserManager == null)
                {
                    _identityUserManager = new UserManager<User>(new UserStore<User>(_mainDbContext));
                }
                return _identityUserManager;
            }
        }
        #endregion

        public VtaServiceResult<UserModel> CheckAuth(string username, string password)
        {
            var result = new VtaServiceResult<UserModel>();
            var user = IdentityUserManager.FindByName(username);
            if (user != null)
            {
                result.Successfully = IdentityUserManager.CheckPassword(user, password);
            }
            return result;
        }

        public bool CheckPassword(User user, string password)
        {
            return IdentityUserManager.CheckPassword(user, password);
        }


        public VtaServiceResult<UserModel> CreateUser(UserModel model)
        {
            var result = new VtaServiceResult<UserModel>();
            var user = new User();
            var identityResult = IdentityUserManager.Create(model.ToEntity(user), model.Password);
            IdentityUserManager.AddClaim(user.Id, new Claim(ClaimTypes.Country, "VN"));
            if (identityResult.Succeeded)
            {
                result.Successfully = true;
                result.Message = GlobalsEnum.GlobalStatus.Success.ToString();
                result.Data = new UserModel(user);
            }
            else
            {
                result.Message = string.Join(",", identityResult.Errors);
            }

            return result;
        }

        public VtaServiceResult<UserLoginTokenModel> CreateOrUpdateLoginToken(UserLoginTokenModel model)
        {
            var result = new VtaServiceResult<UserLoginTokenModel>();
            var utoken = _mainDbContext.UserLoginTokens.FirstOrDefault(x => x.UserId == model.UserId);
            if (utoken == null)
            {
                utoken = new UserLoginToken();
                _mainDbContext.UserLoginTokens.Add(utoken);
            }
            model.Token = Guid.NewGuid().ToString();
            model.ToEntity(utoken);
            try
            {
                _unitOfWork.Save();
                result.Successfully = true;
                result.Data = new UserLoginTokenModel(utoken);
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public VtaServiceResult CheckValidLoginToken(UserLoginTokenModel model)
        {
            var result = new VtaServiceResult();
            var uToken = _unitOfWork.Repository<Qtht_UserLoginTokens>().Get(x => x.Token == model.Token).FirstOrDefault();
            if (uToken != null)
            {
                result.Successfully = new UserLoginTokenModel(uToken).ValidToken;
            }
            return result;
        }



        public VtaServiceResult<UserModel> GetUser(string userId)
        {
            var result = new VtaServiceResult<UserModel>();
            var user = _unitOfWork.Repository<Qtht_AspNetUsers>().GetByID(userId);
            if (user != null)
            {
                result.Successfully = true;
                result.Data = new UserModel(user);
            }
            return result;
        }

        public VtaServiceResult<UserModel> GetUserByUsername(string username)
        {
            var result = new VtaServiceResult<UserModel>();
            var user = _unitOfWork.Repository<Qtht_AspNetUsers>().Get(x=>x.UserName==username).FirstOrDefault();
            if (user != null)
            {
                result.Successfully = true;
                result.Data = new UserModel(user);
            }
            return result;
        }

        public VtaServiceResult<UserModel> CheckLogin(string username, string password)
        {
            var result = new VtaServiceResult<UserModel>();
            var u = IdentityUserManager.FindByName(username);
            if (u != null)
            {
                if (IdentityUserManager.CheckPassword(u, password))
                {
                    if (u.Status == GlobalsEnum.UserStatus.Active.ToString())
                    {
                        result.Successfully = true;
                        result.Data = new UserModel(u);
                    }
                    else
                    {
                        result.StatusCode = GlobalsEnum.GlobalStatus.Account_Login_NotAllow.ToString();
                    }
                }
                else
                {
                    result.StatusCode = GlobalsEnum.GlobalStatus.Account_Login_WrongUsernameOrPassword.ToString();
                }
            }
            else
            {
                result.StatusCode = GlobalsEnum.GlobalStatus.Account_Login_Failed.ToString();
            }
            return result;
        }

        public void ChangeStatus(string status) //Status: active | banned | disable
        {

        }

        public void ImportListUser()
        {

        }

        public UserInfo LogIn(LoginRequestDto dto)
        {
            var rs = new UserInfo();
            if (dto.Username == "adminvta" && dto.Password == "123456")
            {
                rs.SessionToken = Guid.NewGuid().ToString();
                rs.Fullname = "Võ Hoàng Nhã";
                rs.Email = "vohoangnha@vienthonga.vn";
                rs.Username = "nhavh";
                rs.UserId = "NhaVH.Vnpost";
            }
            return rs;
        }

        public bool Logout(string userId)
        {
            var rs = new bool();
            if (userId == "NhaVH.Vnpost")
            {
                rs = true;
            }
            return rs;
        }

        public bool ChangePass(ChangePassModel model)
        {
            var rs = new bool();
            if (model.userId == "246Vnpost" && model.oldPassword == "123456")
            {
                rs = true;
            }
            return rs;
        }


        #region private methods

        public IQueryable<User> FindUser(UserQueryParams queryParams)
        {
            return null;
        }

        #endregion
    }
}