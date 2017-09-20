using System.Collections.Generic;
using vta.loyalty.core.Account;
using vta.loyalty.service.ViewModels;

namespace vta.loyalty.service
{
    public interface IUserRoleManager
    {
        core.Account.User User { get; }
        bool IsExitsUser { get; }
        List<RoleInfoVM> GetAllUserRoleInfo();
        bool IsInRoleNames(string roleNames);
        bool IsInRole(string roleId);
        bool AddRole(string roleName);
        bool AddRole(Role role);
        bool RemoveRole(string roleName);
        bool RemoveRole(Role role);
    }
}