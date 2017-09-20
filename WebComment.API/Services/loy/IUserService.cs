using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using vta.loyalty.common.Models;
using vta.loyalty.core.Enum;
using vta.loyalty.service.Models;
using vta.loyalty.viewmodel.API.Domain;
using VNPOST.BackEnd.API.Services.loy;

namespace vta.loyalty.service
{
    public interface IUserService
    {
        List<core.Account.User> AllUser { get; }


        UserManager<core.Account.User> IdentityUserManager { get; }
        string GeneratePassword();
        bool CheckUserAuth(string email, string password);
        bool CheckUserAuthByLoginToken(string loginToken);

        /// <summary>
        /// lấy thông tin userlogin của 1 token
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        UserLoginTokenModel GetOrInitUserLoginTokenDto(string loginToken);

        UserManager GetUserManager(string userIdOrEmail);
        UserManager GetUserManagerByLoginToken(string loginToken);
        UserManager GetUserManager(string loginProvider, string providerKey);
        UserManager GetUserManager(UserLoginInfo userLoginInfo);

        List<UserModel> FindUserHasBirthday(DateTime birthday);

        List<UserShortInfoVM> GetAllUserShortInfo(int page = 0, int pageItems = int.MaxValue);
        List<UserModel> GetUserModels(int page = 0, int pageItems = int.MaxValue);
        List<UserShortInfoVM> GetAllUserShortInfo(out int totalItems, int page = 0, int pageItems = int.MaxValue);
        List<UserShortInfoVM> GetAllUserShortInfo(UserStatusCollection status, out int totalItems, int page = 0, int pageItems = int.MaxValue);


        VtaServiceResult<List<PosTransactionHistoryModel>> GetTransactionHistoriesFromPOS(string userId);
    }
}