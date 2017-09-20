using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using NLog.LayoutRenderers;
using WebComment.Data;

namespace WebComment.API.Models
{
    
    public class StoreModels
    {
        public string CompanyCode { get; set; }
        public List<StoreCodeModels> ListStoreCode { get; set; }

        public StoreModels(M_VnPostVta model)
        {
            CompanyCode = model.CompanyCode;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            ListStoreCode = jss.Deserialize<List<StoreCodeModels>>(model.ListStoreCode);
        }
    }

    public class StoreCodeModels
    {
        public string MACS { get; set; }
        public string TenCS { get; set; }
        public string Diachi { get; set; }

        public StoreCodeModels()
        {
            
        }

       /* public StoreCodeModels(string macs)
        {
            SqlDbContext db = new SqlDbContext();
            var vtaStore = db.DM_StoreVTA.Where(x => x.MaCs.Equals(macs)).FirstOrDefault();
            if (vtaStore != null)
            {
                this.MACS = macs;
                this.Diachi = vtaStore.DiaChi;
                this.TenCS = string.Format("{0} - {1} - {2}", vtaStore.TenCs, vtaStore.Quan, vtaStore);
            }
           

        }*/
        
    }

    public class StoreVnPostModel
    {
        public long  Id { get; set; }
        public string Name { get; set; }
        public int CompanyLevel { get; set; }
        public long? ParentCompanyId { get; set; }

    }

    #region tungs models
    public class DmStoreVtaModel
    {
        public int Id { get; set; }
        public string MaCs { get; set; }
        public string DiaChi { get; set; }
        public string Quan { get; set; }
        public string ThanhPho { get; set; }
        public string KhuVuc { get; set; }
        public string TenCs { get; set; }
        public List<string> MaCsCollection { get; set; }
        public DmStoreVtaModel(DM_StoreVTA entity)
        {
            this.Id = entity.Id;
            this.MaCs = entity.MaCs;
            this.DiaChi = entity.DiaChi;
            this.Quan = entity.Quan;
            this.ThanhPho = entity.ThanhPho;
            this.KhuVuc = entity.KhuVuc;
            this.TenCs = entity.TenCs;
        }

        public DmStoreVtaModel()
        {
            
        }
    }

    public class MVnPostVtaModel
    {
        
        public long Id { get; set; }
        public string CompanyCode { get; set; }
        //private string _companyName;
        public List<string> MaCsCollection { get; set; }


        public string CompanyName { get; set; }

        public string ListStoreCodes { get; set; }

        private List<StoreCodeModels> _listStoreCode;

        public List<StoreCodeModels> ListStoreCode
        {
            get
            {
                return _listStoreCode ?? (_listStoreCode = !string.IsNullOrEmpty(ListStoreCodes)
                    ? new JavaScriptSerializer().Deserialize<List<StoreCodeModels>>(ListStoreCodes)
                    : new List<StoreCodeModels>());
            }
            set { _listStoreCode = value; }
        }


        public MVnPostVtaModel(M_VnPostVta entity ,string companyName)
        {
            Id = entity.Id;
            CompanyCode = entity.CompanyCode;
            ListStoreCodes = entity.ListStoreCode;
            CompanyName = companyName;


        }

        public MVnPostVtaModel()
        {
            Id = 0;
        }
       
    }

    public class CompanyModel
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public Nullable<long> ParentCompanyId { get; set; }
        public virtual  CompanyModel ParentCompany { get; set; }
        public string Note { get; set; }
        public Nullable<int> CompanyLevel { get; set; }

        public CompanyModel()
        {
            
        }
        public CompanyModel(Company entity)
        {
            Id = entity.Id;
            CompanyCode = entity.CompanyCode;
            CompanyName = entity.CompanyName;
            Address = entity.Address;
            Phone = entity.Phone;
            ParentCompanyId = entity.ParentCompanyId;
            Note = entity.Note;
            CompanyLevel = entity.CompanyLevel;

        }

        public Company ToEntity(Company entity)
        {
            entity.Id = Id;
            entity.CompanyCode = CompanyCode;
            entity.CompanyLevel = CompanyLevel;
            entity.CompanyName = CompanyName;
            entity.Note = Note;
            entity.ParentCompanyId = ParentCompanyId;
            entity.Phone = Phone;
            return entity;
        }
        
    }

    #endregion

#region test branch
    public class ConfigurationVM
    {
        public List<AreaVM> ListArea { get; set; }
        public List<CityVM> ListCity { get; set; }
        public List<DistrictVM> ListDistrict { get; set; }
        public List<BranchsVM> ListBranchs { get; set; }

        public List<CityVM> ListCityBranch
        {
            get
            {
                return ListCity.Where(c => ListBranchs.Select(x => x.Cityid).Contains(c.Cityid)).ToList();
            }
        }
        public List<DistrictVM> ListDistrictBranch
        {
            get
            {
                return ListDistrict.Where(c => ListBranchs.Select(x => x.Districtid).Contains(c.DistrictId)).ToList();
            }
        }
    }
#endregion
#region parameter company

    public class CompanyParameter
    {
        public long CompanyId { get; set; }
        public string[] Role { get; set; }
    }

    #endregion
    #region parameter Role

    public class RoleParameter
    {
        public string[] Role { get; set; }
    }

    #endregion
    
}