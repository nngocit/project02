using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using vta.loyalty.common.Models;
using vta.loyalty.core.Account;
using vta.loyalty.service.Models;
using vta.loyalty.service.ViewModels;
using vta.loyalty.viewmodel.API.Domain;
using vta.loyalty.viewmodel.Domain;

namespace vta.loyalty.service
{
    public interface IUserManager
    {
        core.Account.User User { get; }
        bool IsExitsUser { get; }
        List<UserProfile> UserProfileCollection { get; }
        UserManager<core.Account.User> IdentityUserManager { get; }
        Claim GetClaim(string type);
        bool AddOrUpdateClaim(Claim claim);
        UserModel GetOrInitUserInfoDto();

        /// <summary>
        /// lấy thông tin sơ lược của user
        /// </summary>
        /// <returns></returns>
        UserShortInfoVM GetOrInitUserShortInfo();

        bool Update(UserShortInfoVM vm);
        bool Delete();
        bool RegisterUser(UserRegisterVM vm);
        VtaServiceResult<UserModel> RegisterUser(UserRegisterModel vm);
        bool RegisterUser(UserModel userModel);
        UserLoginTokenModel AddOrUpdateLoginToken(LoginTokenModel vm);
        LoginTokenModel GetOrInitLoginTokenDto(string domain);
        bool RemoveLoginToken(string domainToken);
        bool AddLogin(ExternalLoginInfoVM vm);
        bool AddLogin(ExternalLoginInfoModel vm);
        ClaimsIdentity CreateIdentity(string authenticationType);
        string GenerateEmailConfirmToken();
        bool ConfirmEmail(string token);
        bool IsEmailConfirmed();
        string GenerateResetPasswordToken();
        bool IsResetPasswordValidRequest(string token);
        bool CheckPassword(string password);
        bool CheckValidPhoneNumber(string phone);
        void RemovePassword();
        void AddPassword(string pass);
        void ChangePassword(string newPassword);
        void ExpiredPasswordReset();

        /// <summary>
        /// lấy thông tin đầy đủ của user (bao gồm cả profiles)
        /// </summary>
        /// <returns></returns>
        UserFullInfoVM GetUserFullInfo();

        /// <summary>
        /// lấy thông tin 1 profile của user
        /// </summary>
        /// <param name="userProfileId"></param>
        /// <returns></returns>
        ProfileUpdateInfoVM GetOrInitUserProfileUpdateInfo(int userProfileId);

        UserProfileModel GetOrInitUserProfileModel(string profileCode);
        ProfileUpdateInfoVM GetOrInitUserProfileUpdateInfo(string profilecode);
        List<ProfileUpdateInfoVM> GetAllUserProfileUpdateInfo();
        List<ProfileShortInfoVM> GetAllUserProfileShortInfo();
        bool UpdateUserInfo(UserModel userModel);

        /// <summary>
        /// thêm mới hoặc update thông tin profile của user
        /// </summary>
        /// <param name="profileInfo"></param>
        /// <returns></returns>
        bool CreateOrUpdateUserProfile(ProfileUpdateInfoVM profileInfo);
        VtaServiceResult CreateOrUpdateUserProfile(UserProfileModel profileModel);

        /// <summary>
        /// xóa 1 profile của user
        /// </summary>
        /// <param name="userProfileInfo"></param>
        /// <returns></returns>
        bool DeleteUserProfile(int profileId);

        /// <summary>
        /// xóa 1 profile của user
        /// </summary>
        /// <param name="userProfileInfo"></param>
        /// <returns></returns>
        bool DeleteUserProfile(ProfileUpdateInfoVM userProfileInfo);

        bool CreateDefaultUserProfile();
    }
}