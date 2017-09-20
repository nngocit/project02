using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComment.Data;

namespace WebComment.API.Models
{
    public class RoleModel
    {
        public string RoleId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdateBy { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int Level { get; set; }

        public RoleModel Parent { get; set; }

        public RoleModel()
        {
            CreateDate = DateTime.Now;
        }

        public RoleModel(Role role)
        {
            Name = role.Name;
            RoleId = role.Id;
            Code = role.Code;
            Description = role.Description;
            Type = role.Type;
            CreateDate = role.CreateDate;
            //CreateBy = role.CreatedBy;
            //LastUpdateBy = role.LastUpdateBy;
            LastUpdateDate = role.LastUpdateDate;
            Priority = role.Priority;
            //Level = role.Level;

            Parent = role.Parent != null ? new RoleModel(role.Parent) : null;
        }

        public Role ToEntity(Role role)
        {
            role.Name = Name;
            role.Code = Code;
            role.Description = Description;
            role.Type = Type;
            role.CreateDate = CreateDate;
            role.LastUpdateDate = LastUpdateDate;
            role.Priority = Priority;

            return role;
        }
    }
}