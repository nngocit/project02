using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComment.API.Helper;
using WebComment.Data;

namespace WebComment.API.Models
{
    public class MenuModel
    {
        public DM_Menu MenuCap1 { get; set; }
        public List<DmMenuModel> ListMenuCon { get; set; }
    }
    public  class DmMenuModel
    {
     
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Loai { get; set; }
        public string ListRole { get; set; }
        public string Code { get; set; }
        public string ControllerName { get; set; }
        public List<DM_Menu> ListAction { get; set; }

        public DmMenuModel()
        {
            
        }
        public DmMenuModel(DM_Menu entity, UserInfo user)
        {
           // var allowRoles = "," + entity.ListRole + ",";
            Id = entity.Id;
            Name = entity.Name;
            Status = entity.Status;
            ParentId = entity.ParentId;
            Loai = entity.Loai;
            ListRole = entity.ListRole;
            Code = entity.Code;
            ControllerName = entity.ControllerName;
            ListAction =  new List<DM_Menu>();
           var  listActionAll = StaticDataHelper.GetCacheDataMenuRole().Where(x => x.Loai.Equals("ACTION") && x.ParentId == entity.Id).ToList();
            foreach (var item in listActionAll)
            {
                var allowRoles = "," + item.ListRole + ",";
                if (user.Roles != null)
                {
                    if (user.Roles.Any(x => allowRoles.Contains("," + x.Code + ",")))
                    {
                       
                        ListAction.Add(item);
                    }
                }
            }

        }
    }
}