using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using WebComment.Data;

namespace WebComment.API.Models
{
    public class CommentModel
    {

    }

    public class AllConfigData
    {
        public List<CM_LoaiComment> ListLoaiComent { get; set; }
        public List<CM_TrangThai> ListTrangThai { get; set; }
        public List<NhanVienTiepNhan> ListNhanVienTiepNhan { get; set; }
        public List<CMRole> ListCMRoles { get; set; }

        public AllConfigData()
        {
            ListLoaiComent = new List<CM_LoaiComment>();
            ListTrangThai = new List<CM_TrangThai>();
            ListNhanVienTiepNhan = new List<NhanVienTiepNhan>();
            ListCMRoles = new List<CMRole>();
        }
    }

    public class CMRole
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }
    public class NhanVienTiepNhan
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string Phongban { get; set; }
    }

    public class CommentDataPost
    {
        //Thong tin khach hang
        public string ChucDanh { get; set; }
        public string HoTen { get; set; }
        public string Avatar { get; set; }
        public string GioiTinh { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string Rating { get; set; }

        //Thong tin nhan vien Tiep Nhan


        public string NoiDung { get; set; }
        public string SP_MaSP { get; set; }
        public string SP_TenSP { get; set; }
        public string SP_UrlSP { get; set; }
        public string CookieId { get; set; }

        //Comment moi: Type=New
        public string LoaiHienThi { get; set; } //CM or DG

        //Tra loi Comment: Type=Reply
        public long? CommentId { get; set; }
    }

    public class UserLoginHistoryDataPost
    {
        public string MaNV { get; set; }
        public string Type { get; set; } //LogIn or LogOut
    }

    public class UpdateCommentDataPost
    {
        public string MaNVTiepNhan { get; set; }
        public string PhongBanNV { get; set; }

        public int? LoaiCommentLevel1 { get; set; }
        public int? LoaiCommentLevel2 { get; set; }
        public int? LoaiCommentLevel3 { get; set; }
        public int? LoaiCommentLevel4 { get; set; }

        public int? TrangThaiId { get; set; }

        public string IsDG { get; set; } //"T" or "F"

        public string MaNVQL_VTA { get; set; }
        public string PhongBanNVQL { get; set; }
        public string DiemDanhGia { get; set; }
        public string GhiChu { get; set; }
    }

    public class NVReplyCommentDataPost
    {
        //Thong tin nhan vien Tiep Nhan
        public string NoiDung { get; set; }

        //Tra loi Comment: Type=Reply
        public long? CommentId { get; set; }

        //Ma nhan vien reply
        public string MaNVReply { get; set; }
    }

    public class UpdateUserDataPost
    {
        public string MaNV { get; set; }
        public string RoleId { get; set; }
        public string PhongBan { get; set; }
        public string Status { get; set; }
        public string Avatar { get; set; }
    }


    public class UpdateReplyCommentDataPost
    {
        public string NoiDung { get; set; }
        public string Status { get; set; }
    }

    public class CheckRepliedReponse
    {
        public List<ShortReply> CheckReplied { get; set; }
    }

    public class ShortReply
    {
        public long CommentId { get; set; }
        public string LastSlug { get; set; }
        public string TenNVReply { get; set; }
        public DateTime NgayGioReply { get; set; }
        public string NoiDung { get; set; }
    }

    public class SearchCommentParams
    {
        public string CommentId { get; set; }
        public string TenSanPham { get; set; }
        public string NguoiComment { get; set; }
        public string LoaiHienThi { get; set; }
        public int? LoaiCommentLevel1 { get; set; }
        public int? LoaiCommentLevel2 { get; set; }
        public int? LoaiCommentLevel3 { get; set; }
        public int? LoaiCommentLevel4 { get; set; }
        public string MaNVCSTiepNhan { get; set; }
        public string MaNVSOTiepNhan { get; set; }

        public string NgayBatDau { get; set; }
        public string NgayKetThuc { get; set; }
        public int? TrangThaiCM { get; set; }
        public int? TrangThaiDG { get; set; }

        public string PhongBan { get; set; }
    }

    public class SearchCommentParamsV3
    {
        public int skip { get; set; }
        public int limit { get; set; }
        public SearchCommentParamsV2 where { get; set; }
    }
   
    public class SearchCommentParamsV2
    {
        public string CommentId { get; set; }
        public string TenSanPham { get; set; }
        public string NguoiComment { get; set; }
        public string LoaiHienThi { get; set; }
        public int? LoaiCommentLevel1 { get; set; }
        public int? LoaiCommentLevel2 { get; set; }
        public int? LoaiCommentLevel3 { get; set; }
        public int? LoaiCommentLevel4 { get; set; }
        public string MaNVCSTiepNhan { get; set; }
        public string MaNVSOTiepNhan { get; set; }

        public string NgayBatDau { get; set; }
        public string NgayKetThuc { get; set; }
        public int? TrangThaiCM { get; set; }
        public int? TrangThaiDG { get; set; }

        public string PhongBan { get; set; }

        public int? PageNo { get; set; }
    }

    public class ResetDataParam
    {
        public string PhongBan { get; set; }

        public ResetDataParam()
        {
            PhongBan = string.Empty;
        }
    }

    public class SearchUserParams
    {
        public string MaNV { get; set; }
        public string UsernameCitrix { get; set; }
        public string ChucVu { get; set; }
        public string TenNV { get; set; }
        public string SoDienThoai { get; set; }
        public string NgayBatDau { get; set; }
        public string NgayKetThuc { get; set; }
        public string PhongBan { get; set; }
        public string Status { get; set; }
    }

    public class SearchUserResult
    {
        public DateTime? NgayDangKy { get; set; }
        public string UsernameCitrix { get; set; }
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string ChucDanh { get; set; }
        public string ChucVu { get; set; }
        public string Nhom { get; set; }
        public string TrangThai { get; set; }
        public string Avatar { get; set; }
    }

    public class SearchCommentResult
    {
        public CM_Comment Comment { get; set; }
        //public CM_NVQuanLyDanhGia QLDanhGia { get; set; }
        //public CM_NhanVienTiepNhanComment NVTiepNhan { get; set; }
        public decimal DiemDanhGia { get; set; }
        public string MaNVQuanLy_VTA { get; set; }
        public long? CM_NVQuanLyDanhGiaId { get; set; }
        public DateTime? NgayGioDanhGia { get; set; }
        public DateTime? NgayGioTraLoi { get; set; }

        public string MaNVCSTiepNhan { get; set; }
        public string TenNVCSTiepNhan { get; set; }
        public long? CM_NVCSTiepNhanId { get; set; }

        public string MaNVSOTiepNhan { get; set; }
        public string TenNVSOTiepNhan { get; set; }
        public long? CM_NVSOTiepNhanId { get; set; }

        public List<ReplyComment> Replies { get; set; }

        public SearchCommentResult()
        {
            Comment = new CM_Comment();

            Replies = new List<ReplyComment>();

            DiemDanhGia = 0;
            MaNVQuanLy_VTA = String.Empty;
            CM_NVQuanLyDanhGiaId = null;
            NgayGioDanhGia = null;

            MaNVCSTiepNhan = String.Empty;
            TenNVCSTiepNhan = String.Empty;
            CM_NVCSTiepNhanId = null;

            MaNVSOTiepNhan = String.Empty;
            TenNVSOTiepNhan = String.Empty;
            CM_NVSOTiepNhanId = null;

            NgayGioTraLoi = null;
            //QLDanhGia = new CM_NVQuanLyDanhGia();
            //NVTiepNhan = new CM_NhanVienTiepNhanComment();
        }
    }

    public class ReportCommentResult
    {
        public int Stt { get; set; }
        public long CommentId { get; set; }
        public DateTime NgayGioTao { get; set; }
        //public string LoaiComment { get; set; }

        public CM_LoaiComment LoaiCommentLevel1Id { get; set; }
        public CM_LoaiComment LoaiCommentLevel2Id { get; set; }
        public CM_LoaiComment LoaiCommentLevel3Id { get; set; }
        public CM_LoaiComment LoaiCommentLevel4Id { get; set; }
        public CM_ThongTinKhachHang ThongTinKhachHang { get; set; }

        public string NoiDung { get; set; }

        public string SP_MaSP { get; set; }
        public string SP_TenSP { get; set; }
        public string SP_URL { get; set; }

        public List<ReplyComment> Replies { get; set; }

        public string MaNVCSTiepNhan { get; set; }
        public string TenNVCSTiepNhan { get; set; }
        public long? CM_NVCSTiepNhanId { get; set; }

        public string MaNVSOTiepNhan { get; set; }
        public string TenNVSOTiepNhan { get; set; }
        public long? CM_NVSOTiepNhanId { get; set; }

        public decimal DiemDanhGia { get; set; }
        public string MaNVQuanLy_VTA { get; set; }
        public long? CM_NVQuanLyDanhGiaId { get; set; }
        public DateTime? NgayGioDanhGia { get; set; }
        public DateTime? NgayGioTraLoi { get; set; }

        public int? TrangThaiCM { get; set; }
        public string TrangThaiCM_Text { get; set; }

        public string ThoiGianCho { get; set; }

        public ReportCommentResult()
        {
            DiemDanhGia = 0;
            MaNVQuanLy_VTA = String.Empty;
            CM_NVQuanLyDanhGiaId = null;
            NgayGioDanhGia = null;

            MaNVCSTiepNhan = String.Empty;
            TenNVCSTiepNhan = String.Empty;
            CM_NVCSTiepNhanId = null;

            MaNVSOTiepNhan = String.Empty;
            TenNVSOTiepNhan = String.Empty;
            CM_NVSOTiepNhanId = null;

            NgayGioTraLoi = null;
        }
    }

    public class SearchCommentResultV2
    {
        public List<SearchCommentResult> ListComments { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItem { get; set; }

        public SearchCommentResultV2()
        {
            TotalPage = 0;
            CurrentPage = 0;
            TotalItem = 0;
        }
    }

    public class ExcelReportComment
    {
        public List<ReportCommentResult> ListComments { get; set; }
    }

    public class CommentVtaSite
    {
        public string SP_MaSP { get; set; }
        public string SP_TenSP { get; set; }
        public string SP_UrlSP { get; set; }

        public List<DanhGia> DanhGia { get; set; }
        public List<BinhLuan> BinhLuan { get; set; }

        public int TotalPageDG { get; set; }
        public int CurrentPageDG { get; set; }
        public int TotalPageBL { get; set; }
        public int CurrentPageBL { get; set; }
        public int TotalBL { get; set; }
        public int TotalDG { get; set; }

        public CommentVtaSite()
        {
            SP_MaSP = string.Empty;
            SP_TenSP = string.Empty;
        }
    }

    public class DanhGia
    {
        public long Id { get; set; }
        public KhachHang KhachHang { get; set; }
        public string Rating { get; set; }
        public string NoiDung { get; set; }
        public string Status { get; set; }
        public int? TrangThaiHeThong { get; set; }
        public string TypeName { get; set; }
        public string Likes { get; set; }
        public String CreateDateTime { get; set; }
        public DateTime DTCreateDateTime { get; set; }
        public List<ReplyComment> Replies { get; set; }
    }

    public class BinhLuan
    {
        public long Id { get; set; }
        public KhachHang KhachHang { get; set; }
        public string NoiDung { get; set; }
        public string TrangThai { get; set; }
        public int? TrangThaiHeThong { get; set; }
        public string TypeName { get; set; }
        public String CreateDateTime { get; set; }
        public DateTime DTCreateDateTime { get; set; }
        public List<ReplyComment> Replies { get; set; }
    }

    public class CommentDetailAdmin
    {
        public CM_Comment Comment { get; set; }
        public List<ReplyComment> Replies { get; set; }
        public DateTime? NgayGioTraLoi { get; set; }
      
        public List<BinhLuan> ListAllComments { get; set; }
    }

    public class ReplyComment
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public KhachHang KhachHang { get; set; }
        public string NoiDung { get; set; }
        public string TypeName { get; set; } //CMDG or CMCM
        public string TypeQuanTri { get; set; }
        public string TenQTV { get; set; }
        public string AvatarQTV { get; set; }
        public string CreateDateTime { get; set; }
        public DateTime DTCreateDateTime { get; set; }
        public string Likes { get; set; }
        public string Status { get; set; }
    }

    public class KhachHang
    {
        public long Id { get; set; }
        public string HoTen { get; set; }
        public string Avatar { get; set; }
        public string GioiTinh { get; set; }
        public string Email { get; set; }
    }

    public class RegisterEmployeeInfo
    {
        public string EmpID { get; set; }
        public string UsernameCitrix { get; set; }
        public string RoleId { get; set; }
        public string PhongBan { get; set; }
        public string Avatar { get; set; } //
    }

    public class EmployeeInfo
    {
        public String FullName { get; set; }
    }
    public class RegisterEmployee
    {
        public List<String> EmployeeInfo { get; set; }
    }
    public class EmployeeInfoModel
    {
        public string EmpID { get; set; }
        public string FullName { get; set; }
        public string EMAIL { get; set; }
        public string CELLPHONE { get; set; }
        public DateTime? DateApply { get; set; }
        public DateTime? DATEEND { get; set; }
        public string Department_cd { get; set; }
        public string Title_cd { get; set; }
        public string TitleName { get; set; }
        public string Location_cd { get; set; }
        public string LocationName { get; set; }
        public string Area_cd { get; set; }
        public string Store_cd { get; set; }
        public string StoreName { get; set; }
        public string TypeSaller { get; set; }
        public string GroupKPI { get; set; }
        public string LV { get; set; }
    }

    public class LichLamViec
    {
        public long Id { get; set; }
        public string MaNV { get; set; }
        public int Ca { get; set; }
        public int Ngay { get; set; }
        public string NgayBatDau { get; set; }
        public string NgayKetThuc { get; set; }
    }
}