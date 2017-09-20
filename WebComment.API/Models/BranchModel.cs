using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebComment.API.Models
{
    public class BranchModel
    {
    }
    public class CityVM
    {
        public string Cityid { get; set; }
        public string Cityname { get; set; }
        public long? AreaId { get; set; }
        public long? Top { get; set; }

    }


    public class TragopInfoModel
    {
        public List<CityVM> ListCityNoiO { get; set; }
        public List<CityVM> ListCityHoKhau { get; set; }
        public List<NgheNghiepModel> ListNgheNghiep { get; set; }
        public List<StoreVnPostModel> ListStoreVnpostCity { get; set; }
    }
    public class NgheNghiepModel
    {
        public string NgheNghiepValue { get; set; }
        public string Name { get; set; }
    }
    public class BranchCityRivets
    {
        public string Cityid { get; set; }
        public string Cityname { get; set; }
        public long? AreaId { get; set; }
        public long? Top { get; set; }
        public List<BranchDistrictRivets> ListBranchDistrictRivets { get; set; }

    }

    public class BranchDistrictRivets
    {
        public string DistrictId { get; set; }
        public string CityId { get; set; }
        public string Districtname { get; set; }
        public int Top { get; set; }
        public List<BranchsVM> LiBranchRivets { get; set; }
    }

    public class AreaVM
    {
        public string AreaName { get; set; }
        public string Status { get; set; }
        public long? AreaId { get; set; }
        public long? Position { get; set; }
    }

    public class DistrictVM
    {
        public string DistrictId { get; set; }
        public string CityId { get; set; }
        public string Districtname { get; set; }
        public int Top { get; set; }
    }

    public class BranchsVM
    {
        public long BranchsId { get; set; }
        public string BranchsAdd { get; set; }
        public string BranchsCodePos { get; set; }
        public string BranchsShortAdd { get; set; }
        public string Districtid { get; set; }
        public string Cityid { get; set; }
        public string Status { get; set; }
        public sbyte? GroupId { get; set; }
    }

    public class TraGopTypeModel
    {
        public List<TraGopKeyModel> ListTraGopModel { get; set; }
        public string NhaTaiChinh { get; set; }
    }
    public class TraGopKeyModel
    {
        public string Key { get; set; }
        public double Value { get; set; }
    }
}