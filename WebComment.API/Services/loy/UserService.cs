using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using vta.loyalty.service;
using VNPOST.BackEnd.API.DAL;
using VNPOST.BackEnd.API.Models;
using VNPOST.Commons;
using VNPOST.Data.SqlContext;

namespace VNPOST.BackEnd.API.Services.loy
{
    public class UserManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenericRepository<Qtht_AspNetUsers> _userRepository;
        private readonly GenericRepository<Qtht_UserLoginTokens> _userLoginTokenRepository;
        private readonly GenericRepository<Qtht_AspNetRoles> _roleRepository;
        private readonly GenericRepository<Qtht_AspNetUserRoles> _userInRoleRepository;


        private Qtht_AspNetUsers GetUserByIdOrUsername(string id)
        {
            var user = _userRepository.GetByID(_userId);
            if (user == null)
            {
                user = _userRepository.Get(x => x.UserName == _userId).FirstOrDefault();
            }
            return user;
        }

        #region properties
        private string _userId { get; set; }
        private Qtht_AspNetUsers _user;
        public Qtht_AspNetUsers User
        {
            get
            {
                if (_user == null)
                {
                    _user = GetUserByIdOrUsername(_userId);
                    if (_user != null) _userId = _user.Id;
                }
                return _user;
            }
            private set
            {
                _user = value;
            }
        }

        private UserModel _userModel;
        public UserModel UserModel
        {
            get
            {
                if (_userModel == null) _userModel = User != null ? new UserModel(User) : new UserModel();
                return _userModel;
            }
            private set { _userModel = value; }
        }

        public bool IsExitsUser
        {
            get
            {
                return User != null;
            }
        }


        private UserManager<Qtht_AspNetUsers> _identityUserManager;
        public UserManager<Qtht_AspNetUsers> IdentityUserManager
        {
            get
            {
                if (_identityUserManager == null)
                {
                    _identityUserManager = new UserManager<Qtht_AspNetUsers>(new UserStore<Qtht_AspNetUsers>(_unitOfWork.GetDbContext()));
                    //identityUserManager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<Qtht_AspNetUsers>
                    //{
                    //    Subject = "Security Code",
                    //    BodyFormat = "Your security code is {0}"
                    //});
                    //new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));
                }
                return _identityUserManager;
            }
        }

        private List<Claim> _claims;
        public List<Claim> Claims
        {
            get
            {
                if (_claims == null)
                {
                    _claims = IdentityUserManager.GetClaims(_userId).ToList();
                }
                return _claims;
            }
        }

        public Claim GetClaim(string type)
        {
            return Claims.FirstOrDefault(x => x.Type == type);
        }

        public bool AddOrUpdateClaim(Claim claim)
        {
            var uClaim = GetClaim(claim.Type);
            if (uClaim != null)
            {
                IdentityUserManager.RemoveClaim(_userId, uClaim);
            }
            var ressult = IdentityUserManager.AddClaim(_userId, claim);
            _claims = null;
            return ressult.Succeeded;
        }

        #endregion

        public UserManager(string useridOrEmail)
        {
            useridOrEmail = useridOrEmail.ToLower();
            _userRepository = _unitOfWork.Repository<Qtht_AspNetUsers>();
            _userLoginTokenRepository = _unitOfWork.Repository<Qtht_UserLoginTokens>();
            _roleRepository = _unitOfWork.Repository<Qtht_AspNetRoles>();
            _userInRoleRepository = _unitOfWork.Repository<Qtht_AspNetUserRoles>();

            _user = GetUserByIdOrUsername(useridOrEmail);
            if(_user!=null) _userId = useridOrEmail;
        }

        #region private methods

        private void ResetDataCache()
        {
            User = null;
            UserModel = null;
        }

        #endregion

        #region public methods

        public bool Delete()
        {
            if (IsExitsUser)
            {
                _userRepository.Delete(User);
                try
                {
                    _unitOfWork.Save();
                    ResetDataCache();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }


        public bool CreateDefaultUserRoles()
        {
            if (!IsExitsUser) return false;
            var userRoleManager = new UserRoleManager(User.Id, _roleRepository, _userInRoleRepository, _userRepository);
            if (userRoleManager.IsExitsUser) return userRoleManager.AddRole(RoleCollection.Member.ToString());
            return false;
        }

        private void InitDefaultUserData()
        {
            CreateDefaultUserProfile();
            CreateDefaultUserRoles();
        }

        public ClaimsIdentity CreateIdentity(string authenticationType)
        {
            return IdentityUserManager.CreateIdentity(User, authenticationType);
        }

        public UserLoginTokenModel AddOrUpdateLoginToken(LoginTokenModel vm)
        {
            if (!IsExitsUser) return new UserLoginTokenModel();
            var uLoginToken = _userLoginTokenRepository.GetByDomain(vm.Domain, _userId);
            if (uLoginToken == null)
            {
                uLoginToken = new UserLoginToken();
                _userLoginTokenRepository.Add(uLoginToken);
            }
            vm.ResetLoginToken();
            //automap
            //ServiceAutoMapModelMapper.MapExits(vm, uLoginToken);
            uLoginToken.Domain = vm.Domain;
            uLoginToken.Token = vm.Token;
            uLoginToken.UserId = vm.UserId;
            uLoginToken.ExpireDate = DateTime.Now.AddHours(12);
            try
            {
                _userLoginTokenRepository.Save();
                //automap
                //return ServiceAutoMapModelMapper.CreateUserLoginTokenDto(uLoginToken);
                return new UserLoginTokenModel(uLoginToken);
            }
            catch (Exception ex) { }
            return new UserLoginTokenModel();
        }

        public LoginTokenModel GetOrInitLoginTokenDto(string domain)
        {
            if (!IsExitsUser) return new LoginTokenModel();
            var uLoginToken = _userLoginTokenRepository.GetByDomain(domain, _userId) ?? new UserLoginToken();
            return ServiceAutoMapModelMapper.CreateLoginTokenDto(uLoginToken);
        }

        public bool RemoveLoginToken(string domainToken)
        {
            if (!IsExitsUser) return false;
            var uLoginToken = _userLoginTokenRepository.GetByToken(domainToken);
            if (uLoginToken != null)
            {
                _userLoginTokenRepository.Delete(uLoginToken);
                try
                {
                    _userLoginTokenRepository.Save();
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }

        public bool AddLogin(ExternalLoginInfoVM vm)
        {
            var result = IdentityUserManager.AddLogin(_userId, new UserLoginInfo(vm.LoginProvider, vm.ProviderKey));
            return result.Succeeded;
        }

        public bool AddLogin(ExternalLoginInfoModel vm)
        {
            var result = IdentityUserManager.AddLogin(_userId, new UserLoginInfo(vm.LoginProvider, vm.ProviderKey));
            return result.Succeeded;
        }

        public string GenerateEmailConfirmToken()
        {
            //return IdentityUserManager.GenerateEmailConfirmationToken(_userId);
            var token = StringHelper.GetRandomString(20);
            User.EmailConfirmToken = token;
            User.EmailConfirmTokenExpire = DateTime.Now.AddDays(1);
            _userRepository.Save();
            ResetDataCache();
            return token;
        }

        //public IdentityResult ConfirmEmail(string token)
        //{
        //    return IdentityUserManager.ConfirmEmail(_userId, token);
        //}

        public bool ConfirmEmail(string token)
        {
            if (User.EmailConfirmToken == token)
            {
                User.EmailConfirmed = true;
                User.Status = UserStatusCollection.Active.ToString();
                _userRepository.Save();
                ResetDataCache();
                return true;
            }
            return false;
        }

        public bool IsEmailConfirmed()
        {
            //return IdentityUserManager.IsEmailConfirmed(_userId);
            return User.EmailConfirmed;
        }

        public string GenerateResetPasswordToken()
        {
            //return IdentityUserManager.GenerateEmailConfirmationToken(_userId);
            var token = StringHelper.GetRandomString(20);
            User.PasswordResetToken = token;
            User.PasswordResetTokenExpire = DateTime.Now.AddDays(1);
            _userRepository.Save();
            ResetDataCache();
            return token;
        }

        public bool IsResetPasswordValidRequest(string token)
        {
            return User.PasswordResetToken == token && (User.PasswordResetTokenExpire == null || User.PasswordResetTokenExpire >= DateTime.Now);
        }

        public bool CheckPassword(string password)
        {
            return IdentityUserManager.CheckPassword(User, password);
        }

        public bool CheckValidPhoneNumber(string phone)
        {
            return _userRepository.GetAll().Any(x => x.PhoneNumber == phone && x.Id != _userId);
        }

        public void RemovePassword()
        {
            IdentityUserManager.RemovePassword(_userId);
        }

        public void AddPassword(string pass)
        {
            IdentityUserManager.AddPassword(_userId, pass);
        }

        public void ChangePassword(string newPassword)
        {
            IdentityUserManager.RemovePassword(_userId);
            IdentityUserManager.AddPassword(_userId, newPassword);

            // đánh dấu đã change pass, expired request reset pass hiện tại
            ExpiredPasswordReset();
        }

        public void ExpiredPasswordReset()
        {
            User.PasswordResetTokenExpire = DateTime.Now;
            _userRepository.Save();
            ResetDataCache();
        }

        /// <summary>
        /// lấy thông tin đầy đủ của user (bao gồm cả profiles)
        /// </summary>
        /// <returns></returns>
        public UserFullInfoVM GetUserFullInfo()
        {
            if (User != null)
            {
                return ServiceAutoMapModelMapper.CreateUserFullInfo(User, UserProfileCollection);// new UserFullInfoVM(User, UserProfileCollection);
            }
            return null;
        }

        /// <summary>
        /// lấy thông tin 1 profile của user
        /// </summary>
        /// <param name="userProfileId"></param>
        /// <returns></returns>
        public ProfileUpdateInfoVM GetOrInitUserProfileUpdateInfo(int userProfileId)
        {
            var userProfile = GetUserProfile(userProfileId);

            if (userProfile != null)
            {
                return ServiceAutoMapModelMapper.CreateProfileUpdateInfoVM(userProfile);// new ProfileUpdateInfoVM(userProfile);
            }
            return ServiceAutoMapModelMapper.CreateProfileUpdateInfoVM(_userId);// new ProfileUpdateInfoVM(_userId);
        }

        public ProfileUpdateInfoVM GetOrInitUserProfileUpdateInfo(string profilecode)
        {
            var userProfile = GetUserProfile(profilecode);

            if (userProfile != null)
            {
                return ServiceAutoMapModelMapper.CreateProfileUpdateInfoVM(userProfile); // new ProfileUpdateInfoVM(userProfile);
            }
            return ServiceAutoMapModelMapper.CreateProfileUpdateInfoVM(_userId); //new ProfileUpdateInfoVM(_userId);
        }

        public UserProfileModel GetOrInitUserProfileModel(string profilecode)
        {
            var userProfile = GetUserProfile(profilecode);

            if (userProfile != null)
            {
                return new UserProfileModel(userProfile);
            }
            return new UserProfileModel(_userId)
            {
                ProfileCode = profilecode
            };
        }

        public List<ProfileUpdateInfoVM> GetAllUserProfileUpdateInfo()
        {
            var result = new List<ProfileUpdateInfoVM>();
            if (UserProfileCollection.Any())
            {
                foreach (var item in UserProfileCollection)
                {
                    //result.Add(new ProfileUpdateInfoVM(item));
                    result.Add(ServiceAutoMapModelMapper.CreateProfileUpdateInfoVM(item));
                }
            }
            return result;
        }

        public List<ProfileShortInfoVM> GetAllUserProfileShortInfo()
        {
            var result = new List<ProfileShortInfoVM>();
            if (UserProfileCollection.Any())
            {
                foreach (var item in UserProfileCollection)
                {
                    result.Add(new ProfileShortInfoVM(item));
                }
            }
            return result;
        }

        public bool UpdateUserInfo(UserModel userModel)
        {
            if (IsExitsUser)
            {
                UserModel.ToEntity(User);
                try
                {
                    _userRepository.Save();
                    ResetDataCache();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }


        /// <summary>
        /// thêm mới hoặc update thông tin profile của user
        /// </summary>
        /// <param name="profileInfo"></param>
        /// <returns></returns>
        public bool CreateOrUpdateUserProfile(ProfileUpdateInfoVM profileInfo)
        {
            var userProfile = GetUserProfile(profileInfo.ProfileId);
            if (userProfile == null)
            {
                userProfile = new UserProfile();
                _userProfileRepository.Add(userProfile);
            }
            userProfile = ServiceAutoMapModelMapper.MapExits<ProfileUpdateInfoVM, UserProfile>(profileInfo, userProfile);
            //ProfileUpdateInfoVM.ToModel(profileInfo, ref userProfile);

            userProfile.UserId = _userId;
            _userProfileRepository.Save();
            ResetDataCache();
            return true;
        }

        public VtaServiceResult CreateOrUpdateUserProfile(UserProfileModel profileModel)
        {
            var response = new VtaServiceResult();

            //update user info
            if (profileModel.ProfileCode == UserProfileTypeCollection.general_profile.ToString())
            {
                User = profileModel.ToEntity(User);    
            }
            var checkData = CheckUserAsIdentity(User);
            if (checkData.Successfully)
            {
                var userProfile = GetUserProfile(profileModel.ProfileId);
                if (userProfile == null)
                {
                    userProfile = new UserProfile();
                    _userProfileRepository.Add(userProfile);
                }
                userProfile = profileModel.ToEntity(userProfile);
                userProfile.UserId = _userId;

                _userProfileRepository.Save();
                response.Message = "Thành công!";
                response.Successfully = true;
            }
            else
            {
                response.Successfully = false;
                response.Message = checkData.Message;
                response.Code = checkData.Code;
            }
            ResetDataCache();
            return response;
        }

        private VtaServiceResult CheckUserAsIdentity(Qtht_AspNetUsers user)
        {
            var response = new VtaServiceResult()
            {
                Successfully = true
            };
            if (_userRepository.GetAll().Any(x => x.Email != null && x.Email == user.Email && x.Id != user.Id))
            {
                response.Successfully = false;
                response.Message = "Địa chỉ email đã được đăng ký bởi tài khoản khác.";
                response.Code = "EMAIL_USED";
            }
            else if (_userRepository.GetAll().Any(x => x.PhoneNumber != null && x.PhoneNumber == user.PhoneNumber && x.Id != user.Id))
            {
                //var list = _userRepository.GetAll().Where(x => x.PhoneNumber == user.PhoneNumber && x.Id != user.Id).ToList();
                response.Successfully = false;
                response.Message = "Số điện thoại này đã được sử dụng bởi tài khoản khác.";
                response.Code = "PHONE_USED";
            }
            return response;
        }

        /// <summary>
        /// xóa 1 profile của user
        /// </summary>
        /// <param name="userProfileInfo"></param>
        /// <returns></returns>
        public bool DeleteUserProfile(int profileId)
        {
            var entity = GetUserProfile(profileId);
            if (entity != null)
            {
                _userProfileRepository.Delete(entity);
                try
                {
                    _userProfileRepository.Save();
                    ResetDataCache();
                    return true;
                }
                catch (Exception ex)
                {
                    //throw exception
                }
            }
            return false;
        }

        /// <summary>
        /// xóa 1 profile của user
        /// </summary>
        /// <param name="userProfileInfo"></param>
        /// <returns></returns>
        public bool DeleteUserProfile(ProfileUpdateInfoVM userProfileInfo)
        {
            return DeleteUserProfile(userProfileInfo.ProfileId);
        }

        public bool CreateDefaultUserProfile()
        {
            if (IsExitsUser)
            {
                var userProfile = _userProfileRepository.GetByCode(_userId, UserProfileTypeCollection.general_profile.ToString());
                if (userProfile == null)
                {
                    userProfile = new UserProfile()
                    {
                        ProfileName = "Basic Profile",
                        ProfileCode = UserProfileTypeCollection.general_profile.ToString(),
                        UserId = _userId,
                        PrimaryEmail = User.Email,
                        PrimaryPhone = User.PhoneNumber
                    };
                    _userProfileRepository.Add(userProfile);
                    try
                    {
                        _userProfileRepository.Save();
                        ResetDataCache();
                        return true;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return false;
        }
        #endregion
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUserLoginTokenRepository _userLoginTokenRepository;
        private readonly ICoinRepository _coinRepository;
        private readonly IUserCoinRefRepository _userCoinRefRepository;
        private readonly IStudentService _studentService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserInRoleRepository _userInRoleRepository;
        private readonly MainDbContext _db;

        public UserService(MainDbContext db, IUserRepository userRepository, IUserProfileRepository userProfileRepository, IUserLoginTokenRepository userLoginTokenRepository, IStudentService studentService, ICoinRepository coinRepository, IUserCoinRefRepository userCoinRefRepository, IRoleRepository roleRepository, IUserInRoleRepository userInRoleRepository)
        {
            _db = db;
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _userLoginTokenRepository = userLoginTokenRepository;
            _studentService = studentService;
            _coinRepository = coinRepository;
            _userCoinRefRepository = userCoinRefRepository;
            _roleRepository = roleRepository;
            _userInRoleRepository = userInRoleRepository;
        }

        public UserService(MainDbContext db)
            : this(db, new UserRepository(db), new UserProfileRepository(db), new UserLoginTokenRepository(db), new StudentService(db), new CoinRepository(db), new UserCoinRefRepository(db), new RoleRepository(db), new UserInRoleRepository(db))
        {
        }

        #region properties
        private UserManager<Qtht_AspNetUsers> _identityUserManager;
        public UserManager<Qtht_AspNetUsers> IdentityUserManager
        {
            get
            {
                if (_identityUserManager == null)
                {
                    _identityUserManager = new UserManager<Qtht_AspNetUsers>(new UserStore<Qtht_AspNetUsers>(_db));
                    //identityUserManager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<Qtht_AspNetUsers>
                    //{
                    //    Subject = "Security Code",
                    //    BodyFormat = "Your security code is {0}"
                    //});
                    //new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));
                }
                return _identityUserManager;
            }
        }
        #endregion

        #region public methods

        public List<Qtht_AspNetUsers> AllUser
        {
            get { return _db.Users.ToList(); }
        }

        public string GeneratePassword()
        {
            return StringHelper.GetRandomString(6);
        }

        public bool CheckUserAuth(string email, string password)
        {
            var userManager = GetUserManager(email);
            return userManager.IsExitsUser && userManager.CheckPassword(password);
        }

        public bool CheckUserAuthByLoginToken(string loginToken)
        {
            return _userLoginTokenRepository.GetByTokenAvailable(loginToken) != null;
        }

        /// <summary>
        /// lấy thông tin userlogin của 1 token
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        public UserLoginTokenModel GetOrInitUserLoginTokenDto(string loginToken)
        {
            var uLoginToken = _userLoginTokenRepository.GetByToken(loginToken) ?? new UserLoginToken();
            return ServiceAutoMapModelMapper.CreateUserLoginTokenDto(uLoginToken);
        }

        public UserManager GetUserManager(string userIdOrEmail)
        {
            return new UserManager(userIdOrEmail, _db, _userRepository, _userProfileRepository, _userLoginTokenRepository, _studentService, _coinRepository, _userCoinRefRepository, _roleRepository, _userInRoleRepository);
        }

        public UserManager GetUserManagerByUserName(string username)
        {
            var user = _userRepository.GetByUserName(username);
            return new UserManager(user != null ? user.Id : "", _db, _userRepository, _userProfileRepository, _userLoginTokenRepository, _studentService, _coinRepository, _userCoinRefRepository, _roleRepository, _userInRoleRepository);
        }

        public UserManager GetUserManagerByLoginToken(string loginToken)
        {
            var userLoginToken = _userLoginTokenRepository.GetByTokenAvailable(loginToken);
            return new UserManager(userLoginToken != null ? userLoginToken.UserId : "", _db, _userRepository, _userProfileRepository, _userLoginTokenRepository, _studentService, _coinRepository, _userCoinRefRepository, _roleRepository, _userInRoleRepository);
        }


        public UserManager GetUserManager(string loginProvider, string providerKey)
        {
            var user = IdentityUserManager.Find(new UserLoginInfo(loginProvider, providerKey));
            return new UserManager(user != null ? user.Id : "", _db, _userRepository, _userProfileRepository, _userLoginTokenRepository, _studentService, _coinRepository, _userCoinRefRepository, _roleRepository, _userInRoleRepository);
        }

        public UserManager GetUserManager(UserLoginInfo userLoginInfo)
        {
            var user = userLoginInfo != null ? IdentityUserManager.Find(userLoginInfo) : null;
            return new UserManager(user != null ? user.Id : "", _db, _userRepository, _userProfileRepository, _userLoginTokenRepository, _studentService, _coinRepository, _userCoinRefRepository, _roleRepository, _userInRoleRepository);
        }

        public List<UserModel> FindUserHasBirthday(DateTime birthday)
        {
            //SP_Get_BirthdayUserbyDate
         return this._db.Database.SqlQuery<Qtht_AspNetUsers>(
                   "[dbo].[SP_Get_BirthdayUserbyDate] @Birthday",
                   new SqlParameter("@Birthday", birthday.GetParameterNull())
                   ).ToList().Select(x=> new UserModel(x)).ToList();
        }

        public List<UserShortInfoVM> GetAllUserShortInfo(int page = 0, int pageItems = int.MaxValue)
        {
            var result = new List<UserShortInfoVM>();
            var q = _userRepository.GetAll().OrderByDescending(x => x.Id);
            var list = q.Skip(page * pageItems).Take(pageItems).ToList();
            foreach (var item in list)
            {
                result.Add(ServiceAutoMapModelMapper.CreateUserShortInfo(item));
                //result.Add(new UserShortInfoVM(item));
            }
            return result;
        }

        public List<UserModel> GetUserModels(int page = 0, int pageItems = Int32.MaxValue)
        {
            var result = new List<UserModel>();
            var q = _userRepository.GetAll().OrderByDescending(x => x.Id);
            var list = q.Skip(page * pageItems).Take(pageItems).ToList();
            foreach (var item in list)
            {
                result.Add(new UserModel(item));
            }
            return result;
        }



        public List<UserShortInfoVM> GetAllUserShortInfo(out int totalItems, int page = 0, int pageItems = int.MaxValue)
        {
            var result = new List<UserShortInfoVM>();
            var q = _userRepository.GetAll().OrderByDescending(x => x.Id);
            totalItems = q.Count();
            var list = q.Skip(page * pageItems).Take(pageItems).ToList();
            foreach (var item in list)
            {
                result.Add(new UserShortInfoVM(item));
            }
            return result;
        }

        public List<UserShortInfoVM> GetAllUserShortInfo(UserStatusCollection status, out int totalItems, int page = 0, int pageItems = int.MaxValue)
        {
            var statusName = status.ToString();
            var result = new List<UserShortInfoVM>();
            var q = _userRepository.GetAll().Where(x => x.Status == statusName).OrderByDescending(x => x.Id);
            totalItems = q.Count();
            var list = q.Skip(page * pageItems).Take(pageItems).ToList();
            foreach (var item in list)
            {
                result.Add(ServiceAutoMapModelMapper.CreateUserShortInfo(item));
                //result.Add(new UserShortInfoVM(item));
            }
            return result;
        }

        public VtaServiceResult<List<PosTransactionHistoryModel>> GetTransactionHistoriesFromPOS(string userId)
        {
            var result = new VtaServiceResult<List<PosTransactionHistoryModel>>() { Successfully = true, Code = "200" };

            //[dbo].[SP_get_HistoryVmoneybyLoyUserId] @LoyUserId = N'9d43f2a0-1c89-4613-82fa-fa483729091f'
            var dbContext = _userRepository.DbContext();
            result.Data = dbContext.Database.SqlQuery<PosTransactionHistoryModel>(
                    "[vta_pos_sync].[dbo].[SP_get_HistoryVmoneybyLoyUserId] @LoyUserId ",
                    new SqlParameter("@LoyUserId", userId.GetParameterNull())
                    ).ToList();
            return result;
        }

        #endregion
    }
}
