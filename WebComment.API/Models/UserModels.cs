using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using WebComment.Commons;
using WebComment.Data;

namespace WebComment.API.Models
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime CreateDate { get; set; }
        public string Sex { get; set; }
        public string PersonalId { get; set; }
        public string PhongBan { get; set; }
        public string ChucDanh { get; set; }
        public string Avatar { get; set; }
        public long? CompanyId { get; set; }

        // public List<string> RoleIds { get; set; }
        private List<RoleModel> _roleIdsList;


        public List<RoleModel> RoleIds
        {
            get
            {

                var lstRole = new List<RoleModel>();
                if (!string.IsNullOrEmpty(ListRole))
                {
                    if (ListRole.Split(',').Any())
                    {
                        foreach (string role in ListRole.Split(','))
                        {
                            var roleMD = new RoleModel() { RoleId = role.Split(':')[0], Name = role.Split(':')[2], Code = role.Split(':')[1] };
                            lstRole.Add(roleMD);
                        }
                    }
                    return lstRole;
                }
                else return _roleIdsList;

            }
            set { _roleIdsList = value; }
        }

        public int Age
        {
            get { return DateTime.Now.Year - (Birthday != null ? Birthday.Value.Year : DateTime.Now.Year); }
        }

        public string Status { get; set; }
        public string EmployeeId { get; set; }

        public List<UserInRole> Roles { get; set; }


        public string ListRole { get; set; }
        public string DisplayName
        {
            get
            {
                return !string.IsNullOrEmpty(FullName) ? FullName : (!string.IsNullOrEmpty(UserName) ? UserName : Email);
            }
        }

        public UserModel()
        {
            Status = GlobalsEnum.UserStatus.NotActiveYet.ToString();
            Roles = new List<UserInRole>();
            CreateDate = DateTime.Now;
        }

        public UserModel(User user)
        {
            UserId = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Phone = user.PhoneNumber;
            FullName = user.FullName;
            Birthday = user.Birthday;
            //Sex = user.Sex;
            Status = user.Status;
            EmployeeId = user.EmployeeId;
            //CreateDate = user.CreateDate;
            PersonalId = user.PersonalId;
            CompanyId = user.CompanyId;
            Roles = user.UserInRoles.ToList();
            PhongBan = user.PhongBan;
            Avatar = user.Avatar;

        }

        public User ToEntity(User user)
        {
            //user.CreateDate = CreateDate;
            // user.UserName = UserName;
            if (string.IsNullOrEmpty(UserId))
            {
                user.UserName = UserName;
            }
            user.Email = Email;
            user.FullName = FullName;
            user.Birthday = Birthday;
            //user.Sex = !string.IsNullOrEmpty(Sex) ? Sex.ToLower() : string.Empty;
            user.PhoneNumber = Phone;
            user.Status = Status;
            user.EmployeeId = EmployeeId;
            user.PersonalId = PersonalId;
            user.CompanyId = CompanyId;
            user.PhongBan = PhongBan;
            user.Avatar = Avatar;

            return user;
        }
    }

    public class UserLoginTokenModel
    {
        public string UserId { get; set; }
        public string Domain { get; set; }
        public string Token { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool ValidToken
        {
            get { return ExpireDate == null || (ExpireDate > DateTime.Now); }
        }

        public UserLoginTokenModel() { }

        public UserLoginTokenModel(UserLoginToken loginToken)
        {
            this.Domain = loginToken.Domain;
            this.Token = loginToken.Token;
            this.ExpireDate = loginToken.ExpireDate;
            this.UserId = loginToken.UserId;
        }

        public UserLoginToken ToEntity(UserLoginToken entity)
        {
            entity.UserId = UserId;
            entity.Domain = Domain;
            //entity.Token = Token;
            entity.ExpireDate = ExpireDate;

            return entity;
        }
    }

    public class UserInfo
    {
        public string SessionToken { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Avartar { get; set; }
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public string Phone { get; set; }
        public string PersonalId { get; set; }
        public long? CompanyId { get; set; }
        public List<RoleModel> Roles { get; set; }
        public List<MenuModel> Menu { get; set; }
        public DateTime? Birthday { get; set; }
        public string Sex { get; set; }
        public UserInfo() { }

        public UserInfo(UserModel u)
        {
            Username = u.UserName;
            Email = u.Email;
            Phone = u.Phone;
            Fullname = u.FullName;
            UserId = u.UserId;
            Username = u.UserName;
            EmployeeId = u.EmployeeId;
            PersonalId = u.PersonalId;
            CompanyId = u.CompanyId;
            Birthday = u.Birthday;
            Sex = u.Sex;
            if (u.Roles.Any())
            {
                if (Roles == null)
                {
                    Roles =new List<RoleModel>();
                }
                foreach (var role in u.Roles)
                {
                    Roles.Add(new RoleModel() { RoleId = role.Role.Id, Name = role.Role.Name, Description = role.Role.Description,Code = role.Role.Code});
                }
            }

        }
    }

    #region query models

    public class UserQueryParams
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string EmployeeId { get; set; }
    }
    #endregion
}