using System;
using System.Collections.Generic;
using System.Linq;
using vta.loyalty.service;
using VNPOST.BackEnd.API.DAL;
using VNPOST.BackEnd.API.Models;
using VNPOST.Data.SqlContext;

namespace VNPOST.BackEnd.API.Services.loy
{
    public class UserRoleManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GenericRepository<Qtht_AspNetRoles> _roleRepository;
        private readonly GenericRepository<Qtht_AspNetUserRoles> _userInRoleRepository;
        private readonly GenericRepository<Qtht_AspNetUsers> _userRepository;


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

        public bool IsExitsUser
        {
            get
            {
                return User != null;
            }
        }

        #endregion

        public UserRoleManager(string userId, IUnitOfWork unitOfWork)
        {
            _userId = userId;
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.Repository<Qtht_AspNetUsers>();
            _roleRepository = _unitOfWork.Repository<Qtht_AspNetRoles>();
            _userInRoleRepository = _unitOfWork.Repository<Qtht_AspNetUserRoles>();
        }

        #region public methods
        public List<RoleInfoVM> GetAllUserRoleInfo()
        {
            var result = new List<RoleInfoVM>();
            var userRoles = _userInRoleRepository.GetByUser(User);
            if (userRoles != null)
            {
                foreach (var item in userRoles)
                {
                    result.Add(new RoleInfoVM(item.Role as Role));
                }
            }
            return result;
        }

        public bool IsInRoleNames(string roleNames)
        {
            return false;
        }

        public bool IsInRole(string roleId)
        {
            return false;
        }

        public bool AddRole(string roleName)
        {
            var role = _roleRepository.GetByName(roleName);
            if (role != null)
            {
                return AddRole(role);
            }
            return false;
        }

        public bool AddRole(Role role)
        {
            if (role != null)
            {
                var userInRole = _userInRoleRepository.Get(User, role);
                if (userInRole == null)
                {
                    userInRole = new UserInRole() { RoleId = role.Id, UserId = _userId };
                    _userInRoleRepository.Add(userInRole);
                    try
                    {
                        _userInRoleRepository.Save();
                        return true;
                    }
                    catch (Exception ex) { }
                }
            }
            return false;
        }

        public bool RemoveRole(string roleName)
        {
            var role = _roleRepository.GetByName(roleName);
            if (role != null)
            {
                return RemoveRole(role);
            }
            return false;
        }

        public bool RemoveRole(Role role)
        {
            if (role != null)
            {
                var userInRole = _userInRoleRepository.Get(User, role);
                if (userInRole != null)
                {
                    _userInRoleRepository.Delete(userInRole);
                    try
                    {
                        _userInRoleRepository.Save();
                        return true;
                    }
                    catch (Exception ex) { }
                }
            }
            return false;
        }
        #endregion
    }

    public class RoleManager : IRoleManager
    {
        private readonly IRoleRepository _roleRepository;

        #region properties
        private string _roleId;
        private core.Account.Role _role;
        public core.Account.Role Role
        {
            get
            {
                if (_role == null)
                {
                    _role = _roleRepository.Get(_roleId);
                }
                return _role;
            }
            private set
            {
                _role = value;
            }
        }

        private RoleModel _roleModel { get; set; }
        public RoleModel RoleModel
        {
            get
            {
                if (_roleModel == null) _roleModel = Role != null ? new RoleModel(Role) : new RoleModel();
                return _roleModel;
            }
            private set { _roleModel = value; }
        }

        public bool IsExitsRole
        {
            get
            {
                return Role != null;
            }
        }
        #endregion

        public RoleManager(string roleId, IRoleRepository roleRepository, bool roleIdAsRoleName = false)
        {
            _roleRepository = roleRepository;
            if (roleIdAsRoleName)
            {
                var role = _roleRepository.GetByName(roleId);
                if (role != null)
                {
                    _roleId = role.Id;
                    Role = role;
                }
            }
            _roleId = roleId;
        }

        #region private methods
        private void ResetDataCache()
        {
            Role = null;
            RoleModel = null;
        }
        #endregion

        #region public methods
        public RoleInfoVM GetOrInitRoleInfo()
        {
            if (Role != null) return new RoleInfoVM(Role);
            return new RoleInfoVM();
        }

        public bool CreateOrUpdate(RoleModel roleModel)
        {
            if (!IsExitsRole)
            {
                //create new
                Role = new core.Account.Role();
                _roleRepository.Add(Role);
            }
            Role = roleModel.ToEntity(Role);
            try
            {
                _roleRepository.Save();
                _roleId = Role.Id;
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public bool Delete()
        {
            if (IsExitsRole)
            {
                _roleRepository.Delete(Role);
                try
                {
                    _roleRepository.Save();
                    return true;
                }
                catch (Exception ex)
                {
                }
            }
            return false;
        }

        #endregion
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserInRoleRepository _userInRoleRepository;
        private readonly IUserRepository _userRepository;

        public RoleService(IRoleRepository roleRepository, IUserInRoleRepository userInRoleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userInRoleRepository = userInRoleRepository;
            _userRepository = userRepository;
        }

        public RoleService(MainDbContext db) : this(new RoleRepository(db), new UserInRoleRepository(db), new UserRepository(db)) { }

        #region public methods
        public UserRoleManager GetUserRoleManager(string userId)
        {
            return new UserRoleManager(userId, _roleRepository, _userInRoleRepository, _userRepository);
        }

        public RoleManager GetRoleManager(string roleId, bool roleIdAsRoleName = false)
        {
            return new RoleManager(roleId, _roleRepository, roleIdAsRoleName);
        }

        public List<RoleModel> GetRoles(int page = 0, int pageItems = int.MaxValue)
        {
            var q = _roleRepository.GetAll().OrderByDescending(x => x.Name);
            var list = q.Skip(page * pageItems).Take(pageItems).ToList();
            var result = new List<RoleModel>();
            foreach (var item in list)
            {
                result.Add(new RoleModel(item));
            }
            return result;
        }

        #endregion
    }
}
