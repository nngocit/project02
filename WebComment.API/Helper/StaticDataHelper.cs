using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebComment.API.Models;
using VTA.Service.CoreCache;
using WebComment.Commons;
using WebComment.Data;

namespace WebComment.API.Helper
{
    public class StaticDataHelper
    {
        public static List<DM_Menu> GetCacheDataMenuRole()
        {
            
            var keyCacheProductList = "MenuRole";
            var cacheValues = DataCache.GetCache<List<DM_Menu>>(keyCacheProductList);
            if (cacheValues == null)
            {
                var db = new SqlDbContext();
                 cacheValues = db.DM_Menu.Where(x=>x.Status.Equals("A")).ToList();
                 DataCache.SetCache(keyCacheProductList, cacheValues,
                    DateTime.Now.Add(TimeSpan.FromMinutes(Globals.TimeCache)));
                
                
            }
           // NLog.LogManager.GetCurrentClassLogger().Debug("GetCacheDataMenuRole-" + JsonHelper.SerializeObject(cacheValues));
            return cacheValues;


        }

        public static void SetCacheDataUser(UserInfo user)
        {

            var keyCacheProductList = "User";
            var cacheValues = DataCache.GetCache<List<UserInfo>>(keyCacheProductList);
            if (cacheValues == null)
            {
                cacheValues = new List<UserInfo>();
            }
            cacheValues.Add(user);
            DataCache.SetCache(keyCacheProductList, cacheValues,
                     DateTime.Now.Add(TimeSpan.FromMinutes(Globals.TimeCacheUser)));
        //    NLog.LogManager.GetCurrentClassLogger().Debug("SetCacheDataUser-" + JsonHelper.SerializeObject(cacheValues));

        }
        public static List<UserInfo> GetCacheDataUser()
        {

            var keyCacheProductList = "User";
            var cacheValues = DataCache.GetCache<List<UserInfo>>(keyCacheProductList);
            
            return cacheValues;


        }
        public static List<MenuModel> GetListMenubyToken(UserInfo user)
        {

           
            var data = GetCacheDataMenuRole().Where(x=>x.Loai.Equals("MENU")).ToList();
            var list = new List<MenuModel>();

       //     NLog.LogManager.GetCurrentClassLogger().Debug("UserCanDoByToken-allowRoles" + JsonHelper.SerializeObject(allowRoles));
           // var rs = userCheck.Roles.Any(x => allowRoles.Contains("," + x.Code + ","));
            foreach (var menucap1 in data.Where(x=>x.ParentId == null).ToList())
            {
                var allowRoles = "," + menucap1.ListRole + ",";

                if (user.Roles != null)
                {
                    if (user.Roles.Any(x => allowRoles.Contains("," + x.Code + ",")))
                    {
                        var menu = new MenuModel
                        {
                            MenuCap1 = menucap1,
                            ListMenuCon =
                                data.Where(
                                    x =>
                                        x.ParentId == menucap1.Id &&
                                        user.Roles.Any(b => ("," + x.ListRole + ",").Contains("," + b.Code + ","))).Select(x=> new DmMenuModel(x,user)).ToList()
                        };
                        list.Add(menu);
                    }
                }
              

               
            }

            return list;


        }

    }
}