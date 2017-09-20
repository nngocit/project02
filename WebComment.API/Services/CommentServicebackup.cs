using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using Microsoft.Owin.Infrastructure;
using Ninject.Infrastructure.Disposal;
using Ninject.Selection;
using NLog.LayoutRenderers;
using PhoneNumbers;
using WebComment.API.DAL;
using WebComment.API.IServices;
using WebComment.API.Models;
using WebComment.Commons;
using WebComment.Data;
using RestSharp;

namespace WebComment.API.Services
{
    public class CommentService : ICommentService
    {
        private UnitOfWork _unitOfWork;
        private IUserService _userService = new UserService(new UnitOfWork(new SqlDbContext()));

        public CommentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AllConfigData GetAllConfigData()
        {
            var allConfigData = new AllConfigData();

            try
            {
                var cm_loaicomment = _unitOfWork.Repository<CM_LoaiComment>().Queryable();
                var cm_trangthai = _unitOfWork.Repository<CM_TrangThai>().Queryable();
                var cm_user = _unitOfWork.Repository<User>().Queryable();
                var cm_role = _unitOfWork.Repository<Role>().Queryable();

                allConfigData.ListLoaiComent = cm_loaicomment.Where(x => x.Status == "A").ToList();
                allConfigData.ListTrangThai = cm_trangthai.Where(x => x.Status == "A").ToList();

                var lstNhanVienTiepNhan = (from user in cm_user
                                           where (user.PhongBan == "CS" || user.PhongBan == "SO") && user.Status == "Active"
                                           select new NhanVienTiepNhan()
                                           {
                                               MaNV = !String.IsNullOrEmpty(user.EmployeeId) ? user.EmployeeId.Trim() : user.PersonalId,
                                               TenNV = user.FullName,
                                               Phongban = user.PhongBan
                                           }).ToList();

                allConfigData.ListNhanVienTiepNhan = lstNhanVienTiepNhan;

                var lstRoles = (from role in cm_role
                                where
                                    role.Code == "CM_AdminCS" || role.Code == "CM_NhanVienCS" || role.Code == "CM_AdminSO" ||
                                    role.Code == "CM_NhanVienSO"
                                select new CMRole()
                                {
                                    Id = role.Id,
                                    Name = role.Name,
                                    Code = role.Code
                                }).ToList();

                allConfigData.ListCMRoles = lstRoles;

                return allConfigData;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetAllConfigData() - Mes: ({0}) ", ex.ToString());
                return null;
            }
        }

        public bool UpdateComment(long commentId, UpdateCommentDataPost comment)
        {
            try
            {
                var cm_comment = _unitOfWork.Repository<CM_Comment>().Queryable();
                var cm_nvqldanhgia = _unitOfWork.Repository<CM_NVQuanLyDanhGia>().Queryable();
                var cm_nhanvientiepnhan = _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Queryable();

                var commentToUpdate = cm_comment.FirstOrDefault(x => x.Id == commentId);

                if (commentToUpdate != null)
                {
                    //Neu phan cong comment cho nhan vien tiep nhan comment
                    if (!String.IsNullOrEmpty(comment.MaNVTiepNhan) && !String.IsNullOrEmpty(comment.PhongBanNV))
                    {
                        var currentNVTN =
                            cm_nhanvientiepnhan.Where(
                                x => x.CM_CommentId == commentId && x.PhongBan == comment.PhongBanNV)
                                .OrderByDescending(x => x.Id)
                                .FirstOrDefault();
                        if (currentNVTN != null)
                        {
                            //Asssign cho 1 em khac
                            if (currentNVTN.MaNVTiepNhan_VTA.Trim() != comment.MaNVTiepNhan)
                            {
                                currentNVTN.Status = "D";

                                CM_NhanVienTiepNhanComment nhanvientiepnhan = new CM_NhanVienTiepNhanComment();
                                nhanvientiepnhan.PhongBan = comment.PhongBanNV;
                                nhanvientiepnhan.MaNVTiepNhan_VTA = comment.MaNVTiepNhan;
                                nhanvientiepnhan.CM_CommentId = commentId;
                                nhanvientiepnhan.Comment = commentToUpdate;
                                nhanvientiepnhan.NgayGioTiepNhan = DateTime.Now;
                                nhanvientiepnhan.Status = "A";

                                _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Insert(nhanvientiepnhan);


                                //Cap nhat LastAssign cho User
                                var cm_user = _unitOfWork.Repository<User>().Queryable();
                                var user =
                                    cm_user.FirstOrDefault(
                                        x =>
                                            x.EmployeeId == comment.MaNVTiepNhan || x.PersonalId == comment.MaNVTiepNhan);
                                if (user != null)
                                {
                                    user.LastAssign = DateTime.Now;
                                }
                            }

                        }
                        else //Truong hop comment do chua chia cho ai
                        {
                            CM_NhanVienTiepNhanComment nhanvientiepnhan = new CM_NhanVienTiepNhanComment();
                            nhanvientiepnhan.PhongBan = comment.PhongBanNV;
                            nhanvientiepnhan.MaNVTiepNhan_VTA = comment.MaNVTiepNhan;
                            nhanvientiepnhan.CM_CommentId = commentId;
                            nhanvientiepnhan.Comment = commentToUpdate;
                            nhanvientiepnhan.NgayGioTiepNhan = DateTime.Now;
                            nhanvientiepnhan.Status = "A";

                            _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Insert(nhanvientiepnhan);

                            //Cap nhat LastAssign cho User
                            var cm_user = _unitOfWork.Repository<User>().Queryable();
                            var user =
                                cm_user.FirstOrDefault(
                                    x => x.EmployeeId == comment.MaNVTiepNhan || x.PersonalId == comment.MaNVTiepNhan);
                            if (user != null)
                            {
                                user.LastAssign = DateTime.Now;
                            }
                        }

                    }

                    //cap nhat level cho comment
                    if (comment.LoaiCommentLevel1 != null)
                        commentToUpdate.CM_LoaiCommentLevel1Id = comment.LoaiCommentLevel1;
                    if (comment.LoaiCommentLevel2 != null)
                        commentToUpdate.CM_LoaiCommentLevel2Id = comment.LoaiCommentLevel2;
                    if (comment.LoaiCommentLevel3 != null)
                        commentToUpdate.CM_LoaiCommentLevel3Id = comment.LoaiCommentLevel3;
                    if (comment.LoaiCommentLevel4 != null)
                        commentToUpdate.CM_LoaiCommentLevel4Id = comment.LoaiCommentLevel4;

                    //Cap nhat trang thai ch0 Comment
                    if (comment.TrangThaiId != null)
                    {
                        commentToUpdate.CM_TrangThaiId = comment.TrangThaiId;

                        //Chia lai comment cho nhan vien SO
                        if (commentToUpdate.CM_TrangThaiId == 3)
                        {
                            //var autoToolAssignedStatus = _unitOfWork.GetDbContext().Database.SqlQuery<string>
                            //        ("[AdmincpDemo].[dbo].[SP_CM_AssignCurrentCommentsForAvaiableUsers] @PhongBan, @RoleCode, @CommentId",
                            //        new SqlParameter("@PhongBan", "SO"),
                            //        new SqlParameter("@RoleCode", "CM_NhanvienSO"),
                            //        new SqlParameter("@CommentId", commentId)).FirstOrDefault();
                            //NLog.LogManager.GetCurrentClassLogger().Debug("PostComment2() - Autotool, assigned comment {0} for SO: {1}", commentId, autoToolAssignedStatus);

                            string autoToolAssignedStatus = AssignCurrentCommentForAvalableUser("SO", commentId);
                            NLog.LogManager.GetCurrentClassLogger()
                                .Debug("UpdateComment() - Autotool, assigned comment {0} for SO: {1}", commentId,
                                    autoToolAssignedStatus);
                        }

                    }


                    //Check is DG
                    if (comment.IsDG != null)
                    {
                        commentToUpdate.LoaiHienThi = (comment.IsDG.ToLower() == "t") ? "DG" : "CM";
                    }

                    //Nhan vien quan ly danh gia
                    if (!String.IsNullOrEmpty(comment.MaNVQL_VTA) && !String.IsNullOrEmpty(comment.DiemDanhGia))
                    {
                        var nvqlDanhgiaExisted = cm_nvqldanhgia.FirstOrDefault(x => x.CM_CommentId == commentId);
                        if (nvqlDanhgiaExisted != null)
                        {
                            nvqlDanhgiaExisted.MaNVQuanLy_VTA = comment.MaNVQL_VTA;
                            nvqlDanhgiaExisted.PhongBan = comment.PhongBanNVQL;
                            nvqlDanhgiaExisted.NgayGioDanhGia = DateTime.Now;
                            nvqlDanhgiaExisted.DiemDanhGia = Convert.ToInt32(comment.DiemDanhGia);
                        }
                        else
                        {
                            CM_NVQuanLyDanhGia nvqldanhgia = new CM_NVQuanLyDanhGia();
                            nvqldanhgia.CM_CommentId = commentId;
                            nvqldanhgia.MaNVQuanLy_VTA = comment.MaNVQL_VTA;
                            nvqldanhgia.PhongBan = comment.PhongBanNVQL;
                            nvqldanhgia.NgayGioDanhGia = DateTime.Now;
                            nvqldanhgia.DiemDanhGia = Convert.ToInt32(comment.DiemDanhGia);
                            nvqldanhgia.Status = "A";

                            _unitOfWork.Repository<CM_NVQuanLyDanhGia>().Insert(nvqldanhgia);
                        }
                    }

                    _unitOfWork.Save();
                    return true;
                }
                else
                {
                    NLog.LogManager.GetCurrentClassLogger()
                        .Debug("UpdateComment() - Reply existed comment:: Not found CommentToUpdate");
                    return false;
                }
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("UpdateComment() - Mes: ({0}) ", ex.ToString());
                return false;
            }
        }

        public bool DeleteComment(long commentId)
        {
            try
            {
                var cm_comment = _unitOfWork.Repository<CM_Comment>().Queryable();
                var cm_replycomment = _unitOfWork.Repository<CM_ReplyComment>().Queryable();
                var cm_nhanvientiepnhan = _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Queryable();
                var cm_nvqldanhgia = _unitOfWork.Repository<CM_NVQuanLyDanhGia>().Queryable();

                var commentToDelete = cm_comment.FirstOrDefault(x => x.Id == commentId);

                if (commentToDelete != null)
                {
                    var replyComments = cm_replycomment.Where(x => x.CM_CommentId == commentId).ToList();
                    if (replyComments.Any())
                    {
                        foreach (var reply in replyComments)
                        {
                            reply.Status = "D";
                        }
                    }

                    var nhanVienTiepNhans = cm_nhanvientiepnhan.Where(x => x.CM_CommentId == commentId).ToList();
                    if (nhanVienTiepNhans.Any())
                    {
                        foreach (var nhanvien in nhanVienTiepNhans)
                        {
                            nhanvien.Status = "D";
                        }
                    }

                    var nvQuanLys = cm_nvqldanhgia.Where(x => x.CM_CommentId == commentId).ToList();
                    if (nvQuanLys.Any())
                    {
                        foreach (var ql in nvQuanLys)
                        {
                            ql.Status = "D";
                        }
                    }

                    commentToDelete.Status = "D";

                    _unitOfWork.Save();
                    return true;
                }
                else
                {
                    NLog.LogManager.GetCurrentClassLogger().Debug("DeleteComment() - Not found commentToDelete");
                    return false;
                }
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("DeleteComment() - Mes: ({0}) ", ex.ToString());
                return false;
            }
        }

        public bool PostComment(CommentDataPost comment)
        {
            try
            {
                //Thong tin khach hang
                CM_ThongTinKhachHang khachHang = new CM_ThongTinKhachHang();
                khachHang.ChucDanh = comment.ChucDanh;
                khachHang.HoTen = comment.HoTen;
                khachHang.Avatar = comment.Avatar;
                khachHang.GioiTinh = comment.GioiTinh;
                khachHang.SoDienThoai = comment.SoDienThoai;
                khachHang.Email = comment.Email;
                khachHang.NgayGioTao = DateTime.Now;
                khachHang.CookieId = comment.CookieId;

                if (comment.CommentId != null) //Reply existed comment: Insert data to ReplyComment table
                {
                    var cm_comment = _unitOfWork.Repository<CM_Comment>().Queryable();
                    var commentToReply = cm_comment.FirstOrDefault(x => x.Id == comment.CommentId);

                    if (commentToReply != null)
                    {
                        CM_ReplyComment replyCommentToInsert = new CM_ReplyComment();
                        replyCommentToInsert.NgayGioTao = DateTime.Now;
                        replyCommentToInsert.NoiDung = comment.NoiDung;
                        replyCommentToInsert.Status = "A";
                        //Comment
                        replyCommentToInsert.Comment = commentToReply;
                        //KH
                        replyCommentToInsert.ThongTinKhachHang = khachHang;
                        replyCommentToInsert.MaNV_VTAReply = null;

                        _unitOfWork.Repository<CM_ReplyComment>().Insert(replyCommentToInsert);
                    }
                    else
                    {
                        NLog.LogManager.GetCurrentClassLogger()
                            .Debug("PostComment() - Reply existed comment:: Not found CommentToReply");
                        return false;
                    }

                }
                else //Insert new comment to Comment table
                {
                    CM_Comment commentToInsert = new CM_Comment();
                    commentToInsert.NgayGioTao = DateTime.Now;
                    commentToInsert.NoiDung = String.IsNullOrEmpty(comment.NoiDung) ? "" : comment.NoiDung;
                    if (khachHang != null)
                    {
                        commentToInsert.ThongTinKhachHang = new CM_ThongTinKhachHang();
                        commentToInsert.ThongTinKhachHang = khachHang;
                    }
                    commentToInsert.SP_MaSP = String.IsNullOrEmpty(comment.SP_MaSP) ? "" : comment.SP_MaSP;
                    commentToInsert.SP_TenSP = String.IsNullOrEmpty(comment.SP_TenSP) ? "" : comment.SP_TenSP;
                    commentToInsert.SP_URL = String.IsNullOrEmpty(comment.SP_UrlSP)
                        ? ""
                        : HttpUtility.UrlDecode(comment.SP_UrlSP);
                    //LoaiComment is null: Chua phan loai
                    commentToInsert.CM_TrangThaiId = 1; //Mới - CM
                    commentToInsert.LoaiHienThi = String.IsNullOrEmpty(comment.LoaiHienThi) ? "CM" : comment.LoaiHienThi;
                    commentToInsert.Status = "A";
                    commentToInsert.Rating = comment.Rating;

                    _unitOfWork.Repository<CM_Comment>().Insert(commentToInsert);
                }

                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("PostComment() - Mes: ({0}) ", ex.ToString());
                return false;
            }
        }

        public long PostComment2(CommentDataPost comment)
        {
            long returnId = 0;
            try
            {
                //Thong tin khach hang
                CM_ThongTinKhachHang khachHang = new CM_ThongTinKhachHang();
                khachHang.ChucDanh = comment.ChucDanh;
                khachHang.HoTen = comment.HoTen;
                khachHang.Avatar = comment.Avatar;
                khachHang.GioiTinh = comment.GioiTinh;
                khachHang.SoDienThoai = comment.SoDienThoai;
                khachHang.Email = comment.Email;
                khachHang.NgayGioTao = DateTime.Now;
                khachHang.CookieId = comment.CookieId;

                if (comment.CommentId != null && comment.CommentId != 0)
                //Reply existed comment: Insert data to ReplyComment table
                {
                    var cm_comment = _unitOfWork.Repository<CM_Comment>().Queryable();
                    var commentToReply = cm_comment.FirstOrDefault(x => x.Id == comment.CommentId);

                    if (commentToReply != null)
                    {
                        CM_ReplyComment replyCommentToInsert = new CM_ReplyComment();
                        replyCommentToInsert.NgayGioTao = DateTime.Now;
                        replyCommentToInsert.NoiDung = comment.NoiDung;
                        replyCommentToInsert.Status = "A";
                        //Comment
                        replyCommentToInsert.Comment = commentToReply;
                        //KH
                        replyCommentToInsert.ThongTinKhachHang = khachHang;
                        replyCommentToInsert.MaNV_VTAReply = null;

                        _unitOfWork.Repository<CM_ReplyComment>().Insert(replyCommentToInsert);
                        _unitOfWork.Save();

                        returnId = commentToReply.Id;
                    }
                    else
                    {
                        NLog.LogManager.GetCurrentClassLogger()
                            .Debug("PostComment2() - Reply existed comment:: Not found CommentToReply");
                        return returnId;
                    }

                }
                else //Insert new comment to Comment table
                {
                    CM_Comment commentToInsert = new CM_Comment();
                    commentToInsert.NgayGioTao = DateTime.Now;
                    commentToInsert.NoiDung = String.IsNullOrEmpty(comment.NoiDung) ? "" : comment.NoiDung;
                    if (khachHang != null)
                    {
                        commentToInsert.ThongTinKhachHang = new CM_ThongTinKhachHang();
                        commentToInsert.ThongTinKhachHang = khachHang;
                    }
                    commentToInsert.SP_MaSP = String.IsNullOrEmpty(comment.SP_MaSP) ? "" : comment.SP_MaSP;
                    commentToInsert.SP_TenSP = String.IsNullOrEmpty(comment.SP_TenSP) ? "" : comment.SP_TenSP;
                    commentToInsert.SP_URL = String.IsNullOrEmpty(comment.SP_UrlSP)
                        ? ""
                        : HttpUtility.UrlDecode(comment.SP_UrlSP);
                    //LoaiComment is null: Chua phan loai
                    commentToInsert.CM_TrangThaiId = 1; //Mới - CM
                    commentToInsert.LoaiHienThi = String.IsNullOrEmpty(comment.LoaiHienThi) ? "CM" : comment.LoaiHienThi;
                    commentToInsert.Status = "A";
                    commentToInsert.Rating = comment.Rating;

                    _unitOfWork.Repository<CM_Comment>().Insert(commentToInsert);
                    _unitOfWork.Save();
                    returnId = commentToInsert.Id;

                    //Chia lai comment cho nhan vien CS
                    //EXEC [SP_CM_AssignCurrentCommentsForAvaiableUsers] 'CS', 'CM_NhanvienCS', '10165'
                    //var autoToolAssignedStatus = _unitOfWork.GetDbContext().Database.SqlQuery<string>
                    //            ("[AdmincpDemo].[dbo].[SP_CM_AssignCurrentCommentsForAvaiableUsers] @PhongBan, @RoleCode, @CommentId",
                    //            new SqlParameter("@PhongBan", "CS"),
                    //            new SqlParameter("@RoleCode", "CM_NhanvienCS"),
                    //            new SqlParameter("@CommentId", returnId)).FirstOrDefault();

                    var autoToolAssignedStatus = AssignCurrentCommentForAvalableUser("CS", returnId);
                    NLog.LogManager.GetCurrentClassLogger()
                        .Debug("PostComment2() - Autotool, assigned comment {0} for CS: {1}", returnId,
                            autoToolAssignedStatus);
                }


                return returnId;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("PostComment() - Mes: ({0}) ", ex.ToString());
                return returnId;
            }
        }

        public bool VTAReplyComment(NVReplyCommentDataPost comment)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var cm_comment = _unitOfWork.Repository<CM_Comment>().Queryable().ToList();
                var cm_trangthai = _unitOfWork.Repository<CM_TrangThai>().Queryable().ToList();
                var cm_nhanvientiepnhancomment = _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Queryable().ToList();
                var tblUser = new User();

                var commentToReply = cm_comment.FirstOrDefault(x => x.Id == comment.CommentId);

                var nhanvientiepnhan = cm_nhanvientiepnhancomment
                    .Where(
                        x =>
                            x.CM_CommentId == comment.CommentId && x.MaNVTiepNhan_VTA == comment.MaNVReply &&
                            x.NgayGioTraLoi == null)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (commentToReply != null)
                {
                    CM_ReplyComment replyCommentToInsert = new CM_ReplyComment();
                    replyCommentToInsert.NgayGioTao = DateTime.Now;
                    replyCommentToInsert.NoiDung = comment.NoiDung;
                    replyCommentToInsert.Status = "A";
                    replyCommentToInsert.TypeName = commentToReply.LoaiHienThi == "DG" ? "DG" : "CM";

                    //Comment
                    replyCommentToInsert.Comment = commentToReply;

                    //KH
                    replyCommentToInsert.ThongTinKhachHang = null;

                    //NhanVienTiepNhan
                    if (nhanvientiepnhan != null)
                    {
                        //replyCommentToInsert.NhanVienTiepNhanComment = new CM_NhanVienTiepNhanComment();
                        //replyCommentToInsert.NhanVienTiepNhanComment = nhanvientiepnhan;
                        replyCommentToInsert.CM_NhanVienTiepNhanCommentId = nhanvientiepnhan.Id;

                        //Cap nhat ngay gio reply
                        nhanvientiepnhan.NgayGioTraLoi = replyCommentToInsert.NgayGioTao;
                        nhanvientiepnhan.Status = "D";
                    }
                    //Cap nhat lai table Comment: Da Reply
                    commentToReply.CM_TrangThaiId = 2;
                    commentToReply.TrangThai = cm_trangthai.FirstOrDefault(x => x.Id == 2);

                    //Ma Nhan Vien reply: Co the quan Ly hoac nhan vien duoc chia comment
                    replyCommentToInsert.MaNV_VTAReply = comment.MaNVReply;
                    tblUser =
                     _unitOfWork.Repository<User>()
                         .Queryable()
                         .FirstOrDefault(
                             x =>
                                 x.EmployeeId.Trim() == comment.MaNVReply && (x.PhongBan == "CS" || x.PhongBan == "SO"));

                    _unitOfWork.Repository<CM_ReplyComment>().Insert(replyCommentToInsert);
                    //NhanVien is null
                }
                else
                {
                    return false;
                }
                NLog.LogManager.GetCurrentClassLogger()
                 .Debug(" Sau  Khi reply  la ({0}) ", (object)stopwatch.Elapsed.TotalSeconds);
                _unitOfWork.Save();

                var mail = new MailHelper();
                string subject = "Bạn đã nhận được phản hồi bình luận từ Viễn Thông A!";
                string body = "";
                string mailTemp = "/Templete/Mailcomment.htm";
                if (mail.ReadEmailTemplate(mailTemp, ref subject, ref body))
                {

                    body = body.Replace("[QuanTriVien]", tblUser.FullName);
                    body = body.Replace("[HoTenKhachHang]", string.IsNullOrEmpty(commentToReply.ThongTinKhachHang.GioiTinh) ? "anh/chị " + commentToReply.ThongTinKhachHang.HoTen : (commentToReply.ThongTinKhachHang.GioiTinh == "M" ? "anh " + commentToReply.ThongTinKhachHang.HoTen : "chị " + commentToReply.ThongTinKhachHang.HoTen));
                    body = body.Replace("[Link]", "https://vienthonga.com/" + commentToReply.SP_URL);
                    body = body.Replace("[NoiDungCauHoi]", commentToReply.NoiDung);
                    body = body.Replace("[GioiTinh]", string.IsNullOrEmpty(commentToReply.ThongTinKhachHang.GioiTinh) ? "anh/chị " : (commentToReply.ThongTinKhachHang.GioiTinh == "M" ? "anh " : "chị "));
                    body = body.Replace("[NoiDungTraLoi]", comment.NoiDung);
                    if (!string.IsNullOrEmpty(commentToReply.ThongTinKhachHang.Email) && commentToReply.ThongTinKhachHang.Email.Contains("@"))
                    {
                        if (mail.SendEmail(commentToReply.ThongTinKhachHang.Email,
                            "",
                            "",
                            "Bạn đã nhận được phản hồi bình luận từ Viễn Thông A!", body))
                        {
                            NLog.LogManager.GetCurrentClassLogger().Debug("VTAReplyComment() - Gui email thanh cong cho khach hang: {0}", commentToReply.ThongTinKhachHang.Email);
                        }
                        else
                        {
                            NLog.LogManager.GetCurrentClassLogger().Debug("VTAReplyComment() - Khong gui duoc email cho khach hang: {0}", commentToReply.ThongTinKhachHang.Email);
                        }
                    }
                    else
                    {
                        NLog.LogManager.GetCurrentClassLogger().Debug("VTAReplyComment() - Khong ton tai email khach hang de gui mail");
                        //return false;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("VTAReplyComment() - Mes: ({0}) ", ex.ToString());
                return false;
            }
        }

        public bool VTAReplyCommentV2(NVReplyCommentDataPost comment)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var commentToReply = _unitOfWork.Repository<CM_Comment>().Queryable().FirstOrDefault(x => x.Id == comment.CommentId);

                if (commentToReply != null)
                {
                    var cm_trangthai = _unitOfWork.Repository<CM_TrangThai>().Queryable().FirstOrDefault(x => x.Id == 2);


                    var cm_nhanvientiepnhancomment = _unitOfWork.Repository<CM_NhanVienTiepNhanComment>()
                        .Queryable()
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault(
                            x =>
                                x.CM_CommentId == comment.CommentId && x.MaNVTiepNhan_VTA == comment.MaNVReply &&
                                x.NgayGioTraLoi == null);
                    var nhanvientiepnhan = cm_nhanvientiepnhancomment;


                    CM_ReplyComment replyCommentToInsert = new CM_ReplyComment();
                    replyCommentToInsert.NgayGioTao = DateTime.Now;
                    replyCommentToInsert.NoiDung = comment.NoiDung;
                    replyCommentToInsert.Status = "A";
                    replyCommentToInsert.TypeName = commentToReply.LoaiHienThi == "DG" ? "DG" : "CM";

                    //Comment
                    replyCommentToInsert.Comment = commentToReply;

                    //KH
                    replyCommentToInsert.ThongTinKhachHang = null;

                    //NhanVienTiepNhan
                    if (nhanvientiepnhan != null)
                    {
                        //replyCommentToInsert.NhanVienTiepNhanComment = new CM_NhanVienTiepNhanComment();
                        //replyCommentToInsert.NhanVienTiepNhanComment = nhanvientiepnhan;
                        replyCommentToInsert.CM_NhanVienTiepNhanCommentId = nhanvientiepnhan.Id;

                        //Cap nhat ngay gio reply
                        nhanvientiepnhan.NgayGioTraLoi = replyCommentToInsert.NgayGioTao;
                        nhanvientiepnhan.Status = "D";
                    }
                    //Cap nhat lai table Comment: Da Reply
                    commentToReply.CM_TrangThaiId = 2;
                    commentToReply.TrangThai = cm_trangthai;

                    //Ma Nhan Vien reply: Co the quan Ly hoac nhan vien duoc chia comment
                    replyCommentToInsert.MaNV_VTAReply = comment.MaNVReply;
                    _unitOfWork.Repository<CM_ReplyComment>().Insert(replyCommentToInsert);
                    //NhanVien is null
                    NLog.LogManager.GetCurrentClassLogger().Debug(" Sau  Khi reply  la ({0}) ", (object)stopwatch.Elapsed.TotalSeconds);
                    _unitOfWork.Save();
                }
                else
                {
                    return false;
                }


            
                //var tblUser = new User();
                //tblUser =
                //    _unitOfWork.Repository<User>()
                //        .Queryable()
                //        .FirstOrDefault(
                //            x =>
                //                x.EmployeeId.Trim() == comment.MaNVReply && (x.PhongBan == "CS" || x.PhongBan == "SO"));
                //var mail = new MailHelper();
                //string subject = "Bạn đã nhận được phản hồi bình luận từ Viễn Thông A!";
                //string body = "";
                //string mailTemp = "/Templete/Mailcomment.htm";
                //if (mail.ReadEmailTemplate(mailTemp, ref subject, ref body))
                //{

                //    body = body.Replace("[QuanTriVien]", tblUser.FullName);
                //    body = body.Replace("[HoTenKhachHang]", string.IsNullOrEmpty(commentToReply.ThongTinKhachHang.GioiTinh) ? "anh/chị " + commentToReply.ThongTinKhachHang.HoTen : (commentToReply.ThongTinKhachHang.GioiTinh == "M" ? "anh " + commentToReply.ThongTinKhachHang.HoTen : "chị " + commentToReply.ThongTinKhachHang.HoTen));
                //    body = body.Replace("[Link]", "https://vienthonga.com/" + commentToReply.SP_URL);
                //    body = body.Replace("[NoiDungCauHoi]", commentToReply.NoiDung);
                //    body = body.Replace("[GioiTinh]", string.IsNullOrEmpty(commentToReply.ThongTinKhachHang.GioiTinh) ? "anh/chị " : (commentToReply.ThongTinKhachHang.GioiTinh == "M" ? "anh " : "chị "));
                //    body = body.Replace("[NoiDungTraLoi]", comment.NoiDung);
                //    if (!string.IsNullOrEmpty(commentToReply.ThongTinKhachHang.Email) && commentToReply.ThongTinKhachHang.Email.Contains("@"))
                //    {
                //        if (mail.SendEmail(commentToReply.ThongTinKhachHang.Email,
                //            "",
                //            "",
                //            "Bạn đã nhận được phản hồi bình luận từ Viễn Thông A!", body))
                //        {
                //            NLog.LogManager.GetCurrentClassLogger().Debug("VTAReplyComment() - Gui email thanh cong cho khach hang: {0}", commentToReply.ThongTinKhachHang.Email);
                //        }
                //        else
                //        {
                //            NLog.LogManager.GetCurrentClassLogger().Debug("VTAReplyComment() - Khong gui duoc email cho khach hang: {0}", commentToReply.ThongTinKhachHang.Email);
                //        }
                //    }
                //    else
                //    {
                //        NLog.LogManager.GetCurrentClassLogger().Debug("VTAReplyComment() - Khong ton tai email khach hang de gui mail");
                //        return false;
                //    }

                //}

                return true;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("VTAReplyComment() - Mes: ({0}) ", ex.ToString());
                return false;
            }
        }

        public bool UpdateReplyComment(long replyId, UpdateReplyCommentDataPost repcomment)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var cm_replycomment = _unitOfWork.Repository<CM_ReplyComment>().Queryable();

                var replyComment = cm_replycomment.FirstOrDefault(x => x.Id == replyId);

                if (replyComment != null && !string.IsNullOrEmpty(repcomment.NoiDung))
                {
                    replyComment.NoiDung = repcomment.NoiDung;
                    _unitOfWork.Save();
                    NLog.LogManager.GetCurrentClassLogger()
                   .Debug(" Sau  Khi reply  la ({0}) ", (object)stopwatch.Elapsed.TotalSeconds);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("UpdateReplyComment() - Mes: ({0}) ", ex.ToString());
                return false;
            }

        }

        public List<SearchUserResult> SearchUser(SearchUserParams param)
        {
            List<SearchUserResult> lstFinalResult = new List<SearchUserResult>();
            try
            {
                //Phongban
                var lstResult =
                    _unitOfWork.Repository<User>()
                        .Queryable()
                        .Where(
                            x =>
                                (!string.IsNullOrEmpty(param.PhongBan))
                                    ? (x.PhongBan.Trim() == param.PhongBan.Trim())
                                    : (x.PhongBan.Trim() == "CS" || x.PhongBan.Trim() == "SO") && x.Status == "Active")
                        .ToList();

                //Status
                if (!string.IsNullOrEmpty(param.Status))
                {
                    lstResult = lstResult.Where(x => x.Status.ToLower() == param.Status.ToLower()).ToList();
                }

                //MaNV
                if (!string.IsNullOrEmpty(param.MaNV))
                {
                    lstResult = lstResult.Where(x => x.EmployeeId == param.MaNV || x.PersonalId == param.MaNV).ToList();
                }

                //TenDangNhap
                if (!string.IsNullOrEmpty(param.UsernameCitrix))
                {
                    lstResult =
                        lstResult.Where(x => (!string.IsNullOrEmpty(x.Email) && x.Email.Contains(param.UsernameCitrix)))
                            .ToList();
                }

                //Ten Nhan vien
                if (!string.IsNullOrEmpty(param.TenNV))
                {
                    lstResult = lstResult.Where(x => x.FullName.ToLower().Contains(param.TenNV.ToLower())).ToList();
                }

                //So dien thoai
                if (!string.IsNullOrEmpty(param.SoDienThoai))
                {
                    lstResult = lstResult.Where(x => x.PhoneNumber == param.SoDienThoai).ToList();
                }

                //NgayDangKy
                DateTime? ngayBatDau = null;
                DateTime? ngayKetThuc = null;
                if (!string.IsNullOrEmpty(param.NgayBatDau))
                {
                    var tempStart = Convert.ToDateTime(param.NgayBatDau);
                    ngayBatDau = new DateTime(tempStart.Year, tempStart.Month, tempStart.Day, 0, 0, 0);
                }
                if (!string.IsNullOrEmpty(param.NgayKetThuc))
                {
                    var tempEnd = Convert.ToDateTime(param.NgayKetThuc);
                    ngayKetThuc =
                        new DateTime(tempEnd.Year, tempEnd.Month, tempEnd.Day, 0, 0, 0).AddDays(1).AddSeconds(-1);
                }
                if (ngayBatDau != null && ngayKetThuc != null)
                    lstResult = lstResult.Where(x => x.CreateDate >= ngayBatDau && x.CreateDate <= ngayKetThuc).ToList();

                if (lstResult.Any())
                {
                    foreach (var user in lstResult)
                    {
                        var currentRole =
                            _unitOfWork.Repository<UserInRole>()
                                .Queryable()
                                .FirstOrDefault(
                                    x => x.UserId.Equals(user.Id.Trim()) && x.Role.Code.Contains(user.PhongBan));

                        lstFinalResult.Add(new SearchUserResult()
                        {
                            NgayDangKy = user.CreateDate,
                            UsernameCitrix = user.UserName,
                            MaNV = user.EmployeeId,
                            TenNV = user.FullName,
                            SoDienThoai = user.PhoneNumber,
                            Email = user.Email,
                            ChucDanh = user.ChucDanh,
                            ChucVu =
                                (currentRole != null)
                                    ? (currentRole.Role.Code.ToLower().Contains("admin") ? "Quản lý" : "Nhân viên")
                                    : String.Empty,
                            Nhom = user.PhongBan,
                            TrangThai = user.Status,
                            Avatar = user.Avatar
                        });
                    }

                    //Chuc vu
                    if (!string.IsNullOrEmpty(param.ChucVu))
                    {
                        lstFinalResult = param.ChucVu.ToLower().Contains("admin")
                            ? lstFinalResult.Where(x => x.ChucVu == "Quản lý").ToList()
                            : lstFinalResult.Where(x => x.ChucVu == "Nhân viên").ToList();
                    }

                }
                return lstFinalResult;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("SearchUser() - Mes: ({0}) ", ex.ToString());
                return lstFinalResult;
            }
        }

        public bool InsertLoginHistory(UserLoginHistoryDataPost historyPost)
        {
            try
            {
                //NLog.LogManager.GetCurrentClassLogger().Debug("InsertLoginHistory(): {0} manv: {1}, phongban: {2}", historyPost.Type, historyPost.MaNV, latestLogin.PhongBan.ToString());

                var cm_User = _unitOfWork.Repository<User>().Queryable();
                var cm_UserLogInHistory = _unitOfWork.Repository<CM_UserLoginHistory>().Queryable();

                var user =
                    cm_User.FirstOrDefault(x => x.EmployeeId == historyPost.MaNV || x.PersonalId == historyPost.MaNV);

                if (user != null)
                {
                    if (historyPost.Type == "LogOut")
                    {
                        //find last login
                        var latestLogin =
                            cm_UserLogInHistory.Where(x => x.MaNV.Trim() == historyPost.MaNV.Trim())
                                .OrderByDescending(x => x.Id);
                        if (latestLogin != null)
                        {
                            var rowToUpdate = latestLogin.FirstOrDefault();

                            //update logout
                            if (rowToUpdate.LogInTime != null && rowToUpdate.LogOutTime == null)
                                rowToUpdate.LogOutTime = DateTime.Now;
                        }
                        NLog.LogManager.GetCurrentClassLogger()
                            .Debug("InsertLoginHistory(): LogOut sucessfully - {0} manv: {1}", historyPost.Type,
                                historyPost.MaNV);
                    }
                    else
                    {
                        CM_UserLoginHistory history = new CM_UserLoginHistory();
                        history.MaNV = historyPost.MaNV.Trim();
                        history.PhongBan = user.PhongBan;
                        history.Date = DateTime.Now.Date;
                        history.LogInTime = DateTime.Now;
                        _unitOfWork.Repository<CM_UserLoginHistory>().Insert(history);

                        NLog.LogManager.GetCurrentClassLogger()
                            .Debug("InsertLoginHistory(): LogIn sucessfully - {0} manv: {1}", historyPost.Type,
                                historyPost.MaNV);
                    }

                    _unitOfWork.Save();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("InsertLoginHistory() - Mes: ({0}) ", ex.ToString());
                return false;
            }
        }

        public List<SearchCommentResult> SearchComment(SearchCommentParams param)
        {

            List<SearchCommentResult> lstFinalResult = new List<SearchCommentResult>();


            //var lstCommentIdInNhanVienTiepNhan = _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Queryable().Select(x=>x.CM_CommentId).Distinct().ToList();
            //var cm_replycomment = _unitOfWork.Repository<CM_ReplyComment>().Queryable().Where(x => x.MaNV_VTAReply != null 
            //                        && x.NgayGioTao < DateTime.Today && x.Id > 124).ToList();

            //var tblUser = _unitOfWork.Repository<User>().Queryable();

            //foreach (var reply in cm_replycomment)
            //{
            //    CM_NhanVienTiepNhanComment nhanvientiepnhan = new CM_NhanVienTiepNhanComment();

            //    //,[CM_CommentId]
            //    nhanvientiepnhan.CM_CommentId = reply.CM_CommentId;
            //    //,[MaNVTiepNhan_VTA]
            //    nhanvientiepnhan.MaNVTiepNhan_VTA = reply.MaNV_VTAReply;


            //    var userInfo = tblUser.FirstOrDefault(x => x.EmployeeId.Trim() == reply.MaNV_VTAReply || x.PersonalId.Trim() == reply.MaNV_VTAReply);
            //    if (userInfo != null)
            //    {
            //        //,[PhongBan]
            //        nhanvientiepnhan.PhongBan = userInfo.PhongBan;
            //    }

            //    //,[NgayGioTiepNhan]
            //    nhanvientiepnhan.NgayGioTiepNhan = reply.Comment.NgayGioTao;

            //    //,[NgayGioTraLoi]
            //    nhanvientiepnhan.NgayGioTraLoi = reply.NgayGioTao;
            //    //,[Status]
            //    nhanvientiepnhan.Status = "D";

            //    _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Insert(nhanvientiepnhan);
            //    _unitOfWork.Save();

            //    reply.CM_NhanVienTiepNhanCommentId = nhanvientiepnhan.Id;
            //    _unitOfWork.Save();
            //}


            /*
            try
            {
                var lstResult = _unitOfWork.Repository<CM_Comment>().Queryable().Where(x => x.Status == "A").ToList();

                //Comment Id
                if (!string.IsNullOrEmpty(param.CommentId))
                {
                    long commentId;
                    Int64.TryParse(param.CommentId, out commentId);
                    if (commentId != 0)
                        lstResult = lstResult.Where(x => x.Id == commentId).ToList();
                    else
                        return lstFinalResult;
                }

                //NgayBatDau & NgayKetThuc
                DateTime? ngayBatDau = null;
                DateTime? ngayKetThuc = null;

                if (!string.IsNullOrEmpty(param.NgayBatDau))
                {
                    var tempStart = Convert.ToDateTime(param.NgayBatDau);
                    ngayBatDau = new DateTime(tempStart.Year, tempStart.Month, tempStart.Day, 0, 0, 0);
                }
                if (!string.IsNullOrEmpty(param.NgayKetThuc))
                {
                    var tempEnd = Convert.ToDateTime(param.NgayKetThuc);
                    ngayKetThuc = new DateTime(tempEnd.Year, tempEnd.Month, tempEnd.Day, 0, 0, 0).AddDays(1).AddSeconds(-1);
                }
                if (ngayBatDau != null && ngayKetThuc != null)
                    lstResult = lstResult.Where(x => x.NgayGioTao >= ngayBatDau && x.NgayGioTao <= ngayKetThuc).ToList();

                //Ten san pham
                if (!string.IsNullOrEmpty(param.TenSanPham))
                {
                    lstResult = lstResult.Where(x => x.SP_TenSP.ToLower().Contains(param.TenSanPham.ToLower())).ToList();
                }

                //Ten NguoiCommentLoaiComment
                if (!string.IsNullOrEmpty(param.NguoiComment))
                {
                    string decode = HttpUtility.UrlDecode(param.NguoiComment);
                    lstResult = lstResult.Where(x => !string.IsNullOrEmpty(x.ThongTinKhachHang.HoTen) && x.ThongTinKhachHang.HoTen.ToLower().Contains(decode.ToLower())).ToList();
                }

                //LoaiHienThi
                if (!String.IsNullOrEmpty(param.LoaiHienThi))
                {
                    lstResult = lstResult.Where(x => x.LoaiHienThi == param.LoaiHienThi).ToList();
                }

                //TrangThaiComment
                if (param.TrangThaiCM != null)
                {
                    if (param.TrangThaiCM == -1)
                        lstResult = lstResult.Where(x => x.CM_TrangThaiId != 5 && x.LoaiHienThi == "CM").ToList();
                    else
                        lstResult = lstResult.Where(x => x.CM_TrangThaiId == param.TrangThaiCM && x.LoaiHienThi == "CM").ToList();
                }

                //Loai Comment
                if (param.LoaiCommentLevel1 != null)
                {
                    if (param.LoaiCommentLevel1 == -1) //Chua phan loai
                        lstResult = lstResult.Where(x => x.CM_LoaiCommentLevel1Id == null).ToList();
                    else
                        lstResult = lstResult.Where(x => x.CM_LoaiCommentLevel1Id == param.LoaiCommentLevel1).ToList();
                }
                if (param.LoaiCommentLevel2 != null)
                {
                    lstResult = lstResult.Where(x => x.CM_LoaiCommentLevel2Id == param.LoaiCommentLevel2).ToList();
                }
                if (param.LoaiCommentLevel3 != null)
                {
                    lstResult = lstResult.Where(x => x.CM_LoaiCommentLevel3Id == param.LoaiCommentLevel3).ToList();
                }
                if (param.LoaiCommentLevel4 != null)
                {
                    lstResult = lstResult.Where(x => x.CM_LoaiCommentLevel1Id == param.LoaiCommentLevel4).ToList();
                }

                //Filter again with MaNVCSTiepNhan, 
                if (param.MaNVCSTiepNhan != null)
                {
                    var listCommentIds = _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Queryable().Where(x => x.MaNVTiepNhan_VTA == param.MaNVCSTiepNhan && x.Status == "A").Select(x => x.CM_CommentId).ToList();
                    if (listCommentIds.Any())
                    {
                        lstResult = lstResult.Where(x => listCommentIds.Contains(x.Id)).ToList();
                    }
                    else
                    {
                        lstResult.Clear();
                    }
                }

                //MaNVSOTiepNhan
                if (param.MaNVSOTiepNhan != null)
                {
                    var listCommentIds = _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Queryable().Where(x => x.MaNVTiepNhan_VTA == param.MaNVSOTiepNhan && x.Status == "A").Select(x => x.CM_CommentId).ToList();
                    if (listCommentIds.Any())
                    {
                        lstResult = lstResult.Where(x => listCommentIds.Contains(x.Id)).ToList();
                    }
                    else
                    {
                        lstResult.Clear();
                    }
                }

                //PhongBan
                if (!string.IsNullOrEmpty(param.PhongBan))
                {
                    if (param.PhongBan.Trim() == "SO")
                    {
                        lstResult = lstResult.Where(x => x.CM_TrangThaiId == 3 || x.CM_TrangThaiId == 4).ToList();
                    }
                }

                //Finally search result
                if (lstResult.Any())
                {
                    var cm_nhanvientiepnhan = _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Queryable();
                    var cm_nvqldanhgia = _unitOfWork.Repository<CM_NVQuanLyDanhGia>().Queryable();
                    var tblUser = _unitOfWork.Repository<User>().Queryable();

                    foreach (var comment in lstResult)
                    {
                        SearchCommentResult commentItem = new ();
                        commentItem.Comment = comment;

                        var nvqlDanhGia = cm_nvqldanhgia.Where(x => x.CM_CommentId == comment.Id).OrderByDescending(x => x.NgayGioDanhGia).FirstOrDefault();
                        if (nvqlDanhGia != null)
                        {
                            commentItem.DiemDanhGia = (decimal)nvqlDanhGia.DiemDanhGia;
                            commentItem.MaNVQuanLy_VTA = nvqlDanhGia.MaNVQuanLy_VTA;
                            commentItem.NgayGioDanhGia = nvqlDanhGia.NgayGioDanhGia;
                            commentItem.CM_NVQuanLyDanhGiaId = nvqlDanhGia.Id;
                        }

                        var nvCSTiepNhan = cm_nhanvientiepnhan.Where(x => x.CM_CommentId == comment.Id && x.PhongBan == "CS" && x.Status == "A").OrderByDescending(x => x.NgayGioTiepNhan).FirstOrDefault();
                        if (nvCSTiepNhan != null)
                        {
                            commentItem.MaNVCSTiepNhan = nvCSTiepNhan.MaNVTiepNhan_VTA;
                            var userInfo = tblUser.FirstOrDefault(x => x.EmployeeId.Trim() == commentItem.MaNVCSTiepNhan || x.PersonalId.Trim() == commentItem.MaNVCSTiepNhan);
                            if (userInfo != null)
                                commentItem.TenNVCSTiepNhan = userInfo.FullName;
                            commentItem.CM_NVCSTiepNhanId = nvCSTiepNhan.Id;

                            if (nvCSTiepNhan.NgayGioTraLoi != null)
                                commentItem.NgayGioTraLoi = nvCSTiepNhan.NgayGioTraLoi;
                        }

                        var nvSOTiepNhan = cm_nhanvientiepnhan.Where(x => x.CM_CommentId == comment.Id && x.PhongBan == "SO" && x.Status == "A").OrderByDescending(x => x.NgayGioTiepNhan).FirstOrDefault();
                        if (nvSOTiepNhan != null)
                        {
                            commentItem.MaNVSOTiepNhan = nvSOTiepNhan.MaNVTiepNhan_VTA;
                            var userInfo = tblUser.FirstOrDefault(x => x.EmployeeId.Trim() == commentItem.MaNVSOTiepNhan || x.PersonalId.Trim() == commentItem.MaNVSOTiepNhan);
                            if (userInfo != null)
                                commentItem.TenNVSOTiepNhan = userInfo.FullName;
                            commentItem.CM_NVSOTiepNhanId = nvSOTiepNhan.Id;

                            if (nvSOTiepNhan.NgayGioTraLoi != null && nvCSTiepNhan.NgayGioTraLoi != null && nvSOTiepNhan.NgayGioTraLoi > nvCSTiepNhan.NgayGioTraLoi)
                                commentItem.NgayGioTraLoi = nvSOTiepNhan.NgayGioTraLoi;
                        }

                        if (commentItem.NgayGioTraLoi == null)
                        {
                            var reply = _unitOfWork.Repository<CM_ReplyComment>().Queryable().Where(x => x.CM_CommentId == comment.Id && x.Status == "A" && x.CM_ThongTinKhachHangId == null).OrderByDescending(x => x.NgayGioTao).FirstOrDefault();
                            if (reply != null)
                                commentItem.NgayGioTraLoi = reply.NgayGioTao;
                        }

                        lstFinalResult.Add(commentItem);
                    }

                    //TrangThaiDG
                    if (param.TrangThaiDG != null)
                    {
                        if (param.TrangThaiDG == 6) //Da danh gia
                        {
                            lstFinalResult = lstFinalResult.Where(x => x.NgayGioDanhGia != null).ToList();
                        }

                        if (param.TrangThaiDG == 7) //Chua danh gia
                        {
                            lstFinalResult = lstFinalResult.Where(x => x.NgayGioDanhGia == null).ToList();
                        }

                    }

                    //Sort comment theo ID desc
                    if (lstFinalResult.Any())
                        lstFinalResult = lstFinalResult.OrderByDescending(x => x.Comment.Id).ToList();
                }
                
            

        }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("SearchComment() - Mes: ({0}) ", ex.ToString());
                return lstFinalResult;
            }
            */
            return lstFinalResult;
        }

        public SearchCommentResultV2 SearchCommentV2(SearchCommentParamsV2 param, bool isReport = false)
        {
            SearchCommentResultV2 lstFinalResult = new SearchCommentResultV2();
            lstFinalResult.ListComments = new List<SearchCommentResult>();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                List<CM_Comment> lstResult = new List<CM_Comment>();

                lstFinalResult.CurrentPage = 1;
                lstFinalResult.TotalPage = 0;
                if (param.PageNo != null)
                {
                    lstFinalResult.CurrentPage = Convert.ToInt32(param.PageNo);
                }

                //NgayBatDau & NgayKetThuc
                DateTime? ngayBatDau = null;
                DateTime? ngayKetThuc = null;

                if (!string.IsNullOrEmpty(param.NgayBatDau))
                {
                    var tempStart = Convert.ToDateTime(param.NgayBatDau);
                    ngayBatDau = new DateTime(tempStart.Year, tempStart.Month, tempStart.Day, 0, 0, 0);
                }
                if (!string.IsNullOrEmpty(param.NgayKetThuc))
                {
                    var tempEnd = Convert.ToDateTime(param.NgayKetThuc);
                    ngayKetThuc =
                        new DateTime(tempEnd.Year, tempEnd.Month, tempEnd.Day, 0, 0, 0).AddDays(1).AddSeconds(-1);
                }
                if (ngayBatDau != null && ngayKetThuc != null)
                {
                    lstResult =
                        _unitOfWork.Repository<CM_Comment>()
                            .Queryable()
                            .Where(x => x.Status == "A" && x.NgayGioTao >= ngayBatDau && x.NgayGioTao <= ngayKetThuc)
                            .ToList();
                    NLog.LogManager.GetCurrentClassLogger()
                        .Debug(" SearchCommentV2() - total comment ({0}) la {1} giay ", lstResult.Count,
                            (object)stopwatch.Elapsed.TotalSeconds);
                }


                //Comment Id
                if (!string.IsNullOrEmpty(param.CommentId))
                {
                    long commentId;
                    Int64.TryParse(param.CommentId, out commentId);
                    if (commentId != 0)
                        lstResult = lstResult.Where(x => x.Id == commentId).ToList();
                    else
                        return lstFinalResult;
                }


                //Ten san pham
                if (!string.IsNullOrEmpty(param.TenSanPham))
                {
                    lstResult = lstResult.Where(x => x.SP_TenSP.ToLower().Contains(param.TenSanPham.ToLower())).ToList();
                }

                //Ten NguoiCommentLoaiComment
                if (!string.IsNullOrEmpty(param.NguoiComment))
                {
                    string decode = HttpUtility.UrlDecode(param.NguoiComment);
                    lstResult =
                        lstResult.Where(
                            x =>
                                !string.IsNullOrEmpty(x.ThongTinKhachHang.HoTen) &&
                                x.ThongTinKhachHang.HoTen.ToLower().Contains(decode.ToLower())).ToList();
                }

                //LoaiHienThi
                if (!String.IsNullOrEmpty(param.LoaiHienThi))
                {
                    lstResult = lstResult.Where(x => x.LoaiHienThi == param.LoaiHienThi).ToList();
                }

                //TrangThaiComment
                if (param.TrangThaiCM != null)
                {
                    if (param.TrangThaiCM == -1)
                        lstResult = lstResult.Where(x => x.CM_TrangThaiId != 5 && x.LoaiHienThi == "CM").ToList();
                    else
                        lstResult =
                            lstResult.Where(x => x.CM_TrangThaiId == param.TrangThaiCM && x.LoaiHienThi == "CM")
                                .ToList();
                }

                //Loai Comment
                if (param.LoaiCommentLevel1 != null)
                {
                    if (param.LoaiCommentLevel1 == -1) //Chua phan loai
                        lstResult = lstResult.Where(x => x.CM_LoaiCommentLevel1Id == null).ToList();
                    else
                        lstResult = lstResult.Where(x => x.CM_LoaiCommentLevel1Id == param.LoaiCommentLevel1).ToList();
                }
                if (param.LoaiCommentLevel2 != null)
                {
                    lstResult = lstResult.Where(x => x.CM_LoaiCommentLevel2Id == param.LoaiCommentLevel2).ToList();
                }
                if (param.LoaiCommentLevel3 != null)
                {
                    lstResult = lstResult.Where(x => x.CM_LoaiCommentLevel3Id == param.LoaiCommentLevel3).ToList();
                }
                if (param.LoaiCommentLevel4 != null)
                {
                    lstResult = lstResult.Where(x => x.CM_LoaiCommentLevel1Id == param.LoaiCommentLevel4).ToList();
                }

                //Filter again with MaNVCSTiepNhan, 
                if (param.MaNVCSTiepNhan != null)
                {
                    var listCommentIds =
                        _unitOfWork.Repository<CM_NhanVienTiepNhanComment>()
                            .Queryable()
                            .Where(
                                x =>
                                    x.MaNVTiepNhan_VTA == param.MaNVCSTiepNhan &&
                                    ((x.NgayGioTraLoi != null && x.Status == "D") ||
                                     (x.NgayGioTraLoi == null && x.Status == "A")))
                            .Distinct()
                            .Select(x => x.CM_CommentId)
                            .ToList();
                    if (listCommentIds.Any())
                    {
                        lstResult = lstResult.Where(x => listCommentIds.Contains(x.Id)).ToList();
                    }
                    else
                    {
                        lstResult.Clear();
                    }
                }

                //MaNVSOTiepNhan
                if (param.MaNVSOTiepNhan != null)
                {
                    var listCommentIds =
                        _unitOfWork.Repository<CM_NhanVienTiepNhanComment>()
                            .Queryable()
                            .Where(
                                x =>
                                    x.MaNVTiepNhan_VTA == param.MaNVSOTiepNhan &&
                                    ((x.NgayGioTraLoi != null && x.Status == "D") ||
                                     (x.NgayGioTraLoi == null && x.Status == "A")))
                            .Select(x => x.CM_CommentId)
                            .ToList();
                    if (listCommentIds.Any())
                    {
                        lstResult = lstResult.Where(x => listCommentIds.Contains(x.Id)).ToList();
                    }
                    else
                    {
                        lstResult.Clear();
                    }
                }

                //PhongBan
                if (!string.IsNullOrEmpty(param.PhongBan))
                {
                    if (param.PhongBan.Trim() == "SO")
                    {
                        lstResult = lstResult.Where(x => x.CM_TrangThaiId == 3 || x.CM_TrangThaiId == 4).ToList();
                    }
                }

                NLog.LogManager.GetCurrentClassLogger()
                    .Debug(" SearchCommentV2() Truoc khi ket table {0} giay ", (object)stopwatch.Elapsed.TotalSeconds);

                //Finally search result
                if (lstResult.Any())
                {
                    //Sort comment theo ID desc
                    lstResult = lstResult.OrderByDescending(x => x.Id).ToList();
                    var listResultIds =
                        lstResult.Select(result => Convert.ToInt64(result.Id)).Select(x => (long?)x).ToList();
                    var cm_nvqlDanhGia =
                        _unitOfWork.Repository<CM_NVQuanLyDanhGia>()
                            .Queryable()
                            .Where(x => listResultIds.Contains(x.CM_CommentId))
                            .ToList();
                    var cm_nhanvientiepnhan =
                        _unitOfWork.Repository<CM_NhanVienTiepNhanComment>()
                            .Queryable()
                            .Where(x => listResultIds.Contains(x.CM_CommentId))
                            .ToList();
                    var cm_reply =
                        _unitOfWork.Repository<CM_ReplyComment>()
                            .Queryable()
                            .Where(x => listResultIds.Contains(x.CM_CommentId) && x.Status == "A")
                            .ToList();
                    var tblUser =
                        _unitOfWork.Repository<User>()
                            .Queryable()
                            .Where(x => x.PhongBan == "SO" || x.PhongBan == "CS")
                            .ToList();

                    lstFinalResult.TotalItem = lstResult.Count;

                    if (param.TrangThaiDG == null && !isReport)
                    {
                        lstFinalResult.TotalPage = (Convert.ToInt32(lstResult.Count % 20) != 0)
                            ? (lstResult.Count / 20) + 1
                            : (lstResult.Count / 20);
                        lstResult = lstResult.Skip((lstFinalResult.CurrentPage - 1) * 20).Take(20).ToList();
                    }

                    foreach (var comment in lstResult)
                    {
                        SearchCommentResult commentItem = new SearchCommentResult { Comment = comment };

                        var nvqlDanhGia =
                            cm_nvqlDanhGia.Where(x => x.CM_CommentId == comment.Id)
                                .OrderByDescending(x => x.NgayGioDanhGia)
                                .FirstOrDefault();

                        if (nvqlDanhGia != null)
                        {
                            commentItem.DiemDanhGia = (decimal)nvqlDanhGia.DiemDanhGia;
                            commentItem.MaNVQuanLy_VTA = nvqlDanhGia.MaNVQuanLy_VTA;
                            commentItem.NgayGioDanhGia = nvqlDanhGia.NgayGioDanhGia;
                            commentItem.CM_NVQuanLyDanhGiaId = nvqlDanhGia.Id;
                        }

                        var nvCSTiepNhan =
                            cm_nhanvientiepnhan.Where(x => x.CM_CommentId == comment.Id && x.PhongBan == "CS")
                                .OrderByDescending(x => x.NgayGioTiepNhan)
                                .FirstOrDefault();
                        if (nvCSTiepNhan != null)
                        {
                            commentItem.MaNVCSTiepNhan = nvCSTiepNhan.MaNVTiepNhan_VTA;
                            var userInfo =
                                tblUser.FirstOrDefault(
                                    x =>
                                        x.EmployeeId.Trim() == commentItem.MaNVCSTiepNhan ||
                                        x.PersonalId.Trim() == commentItem.MaNVCSTiepNhan);
                            if (userInfo != null)
                                commentItem.TenNVCSTiepNhan = userInfo.FullName;
                            commentItem.CM_NVCSTiepNhanId = nvCSTiepNhan.Id;

                            if (nvCSTiepNhan.NgayGioTraLoi != null)
                                commentItem.NgayGioTraLoi = nvCSTiepNhan.NgayGioTraLoi;
                        }

                        var nvSOTiepNhan =
                            cm_nhanvientiepnhan.Where(x => x.CM_CommentId == comment.Id && x.PhongBan == "SO")
                                .OrderByDescending(x => x.NgayGioTiepNhan)
                                .FirstOrDefault();
                        if (nvSOTiepNhan != null)
                        {
                            commentItem.MaNVSOTiepNhan = nvSOTiepNhan.MaNVTiepNhan_VTA;
                            var userInfo =
                                tblUser.FirstOrDefault(
                                    x =>
                                        x.EmployeeId.Trim() == commentItem.MaNVSOTiepNhan ||
                                        x.PersonalId.Trim() == commentItem.MaNVSOTiepNhan);
                            if (userInfo != null)
                                commentItem.TenNVSOTiepNhan = userInfo.FullName;
                            commentItem.CM_NVSOTiepNhanId = nvSOTiepNhan.Id;

                            if (nvSOTiepNhan.NgayGioTraLoi != null && nvCSTiepNhan.NgayGioTraLoi != null &&
                                nvSOTiepNhan.NgayGioTraLoi > nvCSTiepNhan.NgayGioTraLoi)
                                commentItem.NgayGioTraLoi = nvSOTiepNhan.NgayGioTraLoi;
                        }

                        if (commentItem.NgayGioTraLoi == null)
                        {
                            var reply =
                                cm_reply.Where(x => x.CM_CommentId == comment.Id && x.CM_ThongTinKhachHangId == null)
                                    .OrderByDescending(x => x.NgayGioTao)
                                    .FirstOrDefault();
                            if (reply != null)
                                commentItem.NgayGioTraLoi = reply.NgayGioTao;
                        }

                        //Report need Replies
                        if (isReport)
                        {
                            List<string> lstMaNVCS =
                                tblUser.Where(x => x.PhongBan == "CS" && x.Status == "Active")
                                    .Select(x => x.EmployeeId)
                                    .ToList();
                            var lstReplies =
                                cm_reply.Where(
                                    x =>
                                        !string.IsNullOrEmpty(x.MaNV_VTAReply) && lstMaNVCS.Contains(x.MaNV_VTAReply) &&
                                        x.CM_CommentId == comment.Id).OrderBy(x => x.NgayGioTao).Take(3).ToList();
                            if (lstReplies != null)
                            {
                                var lstRepliesList = lstReplies;

                                foreach (var rep in lstRepliesList)
                                {
                                    ReplyComment reply = new ReplyComment();
                                    reply.Id = rep.Id;
                                    //reply.CreateDateTime = GetElapsedTime(rep.NgayGioTao);
                                    //reply.DTCreateDateTime = rep.NgayGioTao;
                                    //reply.Likes = rep.Likes;
                                    reply.NoiDung = StripHTML(MinifyHTML(WebUtility.HtmlDecode(rep.NoiDung)));
                                    reply.ParentId = comment.Id;
                                    //reply.Status = rep.Status;
                                    //reply.TypeName = rep.TypeName;
                                    //reply.TypeQuanTri = rep.ThongTinKhachHang != null ? "F" : "T";

                                    commentItem.Replies.Add(reply);
                                }
                            }
                        }

                        //TrangThaiDG
                        if (param.TrangThaiDG != null)
                        {
                            if (param.TrangThaiDG == 6) //Da danh gia
                            {
                                if (commentItem.NgayGioDanhGia != null)
                                    lstFinalResult.ListComments.Add(commentItem);
                            }

                            if (param.TrangThaiDG == 7) //Chua danh gia
                            {
                                if (commentItem.NgayGioDanhGia == null)
                                    lstFinalResult.ListComments.Add(commentItem);
                            }
                        }
                        else
                        {
                            lstFinalResult.ListComments.Add(commentItem);
                        }
                    }

                    //Order by 
                    lstFinalResult.ListComments =
                        lstFinalResult.ListComments.OrderByDescending(x => x.Comment.Id).ToList();
                }

                //Pagination
                if (param.TrangThaiDG != null && !isReport)
                {
                    lstFinalResult.ListComments =
                        lstFinalResult.ListComments.Skip((lstFinalResult.CurrentPage - 1) * 20).Take(20).ToList();
                    //Count total page
                    lstFinalResult.TotalPage = (Convert.ToInt32(lstFinalResult.ListComments.Count % 20) != 0)
                        ? (lstFinalResult.ListComments.Count / 20) + 1
                        : (lstResult.Count / 20);
                }

                NLog.LogManager.GetCurrentClassLogger()
                    .Debug("SearchCommentV2() Sau khi ket table {0} giay ", (object)stopwatch.Elapsed.TotalSeconds);

                return lstFinalResult;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("SearchComment() - Mes: ({0}) ", ex.ToString());
                return lstFinalResult;
            }
        }

        public List<ReportCommentResult> XuatReportAdmin(SearchCommentParamsV2 param)
        {
            List<ReportCommentResult> finalResult = new List<ReportCommentResult>();

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var lstForXuatReport = SearchCommentV2(param, true);
                if (lstForXuatReport.ListComments.Any())
                {
                    int index = 1;
                    foreach (var comment in lstForXuatReport.ListComments)
                    {
                        ReportCommentResult reportComment = new ReportCommentResult();

                        //public int Stt { get; set; }
                        reportComment.Stt = index;
                        //public long CommentId { get; set; }
                        reportComment.CommentId = comment.Comment.Id;
                        //public DateTime NgayGioTao { get; set; }
                        reportComment.NgayGioTao = comment.Comment.NgayGioTao;
                        //public string LoaiComment { get; set; }
                        //public CM_LoaiComment LoaiCommentLevel1Id { get; set; }
                        reportComment.LoaiCommentLevel1Id = comment.Comment.LoaiCommentLevel1Id;
                        //public CM_LoaiComment LoaiCommentLevel2Id { get; set; }
                        reportComment.LoaiCommentLevel2Id = comment.Comment.LoaiCommentLevel2Id;
                        //public CM_LoaiComment LoaiCommentLevel3Id { get; set; }
                        reportComment.LoaiCommentLevel3Id = comment.Comment.LoaiCommentLevel3Id;
                        //public CM_LoaiComment LoaiCommentLevel4Id { get; set; }
                        reportComment.LoaiCommentLevel4Id = comment.Comment.LoaiCommentLevel4Id;
                        //public CM_ThongTinKhachHang ThongTinKhachHang { get; set; }
                        reportComment.ThongTinKhachHang = comment.Comment.ThongTinKhachHang;
                        //public string NoiDung { get; set; }
                        reportComment.NoiDung = StripHTML(MinifyHTML(WebUtility.HtmlDecode(comment.Comment.NoiDung)));

                        //public string SP_MaSP { get; set; }
                        reportComment.SP_MaSP = comment.Comment.SP_MaSP;
                        //public string SP_TenSP { get; set; }
                        reportComment.SP_TenSP = comment.Comment.SP_TenSP;
                        //public string SP_URL { get; set; }
                        reportComment.SP_URL = comment.Comment.SP_URL;

                        //public List<ReplyComment> Replies { get; set; }
                        reportComment.Replies = comment.Replies;

                        //public string MaNVCSTiepNhan { get; set; }
                        reportComment.MaNVCSTiepNhan = comment.MaNVCSTiepNhan;
                        //public string TenNVCSTiepNhan { get; set; }
                        reportComment.TenNVCSTiepNhan = comment.TenNVCSTiepNhan;
                        //public long? CM_NVCSTiepNhanId { get; set; }
                        reportComment.CM_NVCSTiepNhanId = comment.CM_NVCSTiepNhanId;

                        //public string MaNVSOTiepNhan { get; set; }
                        reportComment.MaNVSOTiepNhan = comment.MaNVSOTiepNhan;
                        //public string TenNVSOTiepNhan { get; set; }
                        reportComment.TenNVSOTiepNhan = comment.TenNVSOTiepNhan;
                        //public long? CM_NVSOTiepNhanId { get; set; }
                        reportComment.CM_NVSOTiepNhanId = comment.CM_NVSOTiepNhanId;

                        //public decimal DiemDanhGia { get; set; }
                        reportComment.DiemDanhGia = comment.DiemDanhGia;
                        //public string MaNVQuanLy_VTA { get; set; }
                        reportComment.MaNVQuanLy_VTA = comment.MaNVQuanLy_VTA;
                        //public long? CM_NVQuanLyDanhGiaId { get; set; }
                        reportComment.CM_NVQuanLyDanhGiaId = comment.CM_NVQuanLyDanhGiaId;
                        //public DateTime? NgayGioDanhGia { get; set; }
                        reportComment.NgayGioDanhGia = comment.NgayGioDanhGia;
                        //public DateTime? NgayGioTraLoi { get; set; }
                        reportComment.NgayGioTraLoi = comment.NgayGioTraLoi;
                        //public int? TrangThaiCM { get; set; }
                        reportComment.TrangThaiCM = comment.Comment.CM_TrangThaiId;
                        //public string TrangThaiCM_Text { get; set; }
                        reportComment.TrangThaiCM_Text = comment.Comment.TrangThai.TenTrangThai;

                        //public string ThoiGianCho { get; set; }
                        reportComment.ThoiGianCho = "";

                        finalResult.Add(reportComment);
                        index++;
                    }
                }

                NLog.LogManager.GetCurrentClassLogger()
                    .Debug("XuatReportAdmin() Thuc thi trong {0} giay ", (object)stopwatch.Elapsed.TotalSeconds);

                return finalResult;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("XuatReportAdmin() - Mes: ({0}) ", ex.ToString());
                return finalResult;
            }
        }

        public static string StripHTML(string input)
        {
            return WebUtility.HtmlDecode(Regex.Replace(input, "<.*?>", String.Empty));
        }

        public static string MinifyHTML(string html)
        {
            try
            {
                /// Solution A
                html = Regex.Replace(html, @"\n|\t", " ");
                html = Regex.Replace(html, @">\s+<", "><").Trim();
                html = Regex.Replace(html, @"\s{2,}", " ");

                /// Solution B
                //html = Regex.Replace(html, @"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}", "");
                //html = Regex.Replace(html, @"[ \f\r\t\v]?([\n\xFE\xFF/{}[\];,<>*%&|^!~?:=])[\f\r\t\v]?", "$1");
                //html = html.Replace(";\n", ";");

                /// Solution C
                html = Regex.Replace(html, @"[a-zA-Z]+#", "#");
                html = Regex.Replace(html, @"[\n\r]+\s*", string.Empty);
                html = Regex.Replace(html, @"\s+", " ");
                html = Regex.Replace(html, @"\s?([:,;{}])\s?", "$1");
                html = html.Replace(";}", "}");
                html = Regex.Replace(html, @"([\s:]0)(px|pt|%|em)", "$1");

                /// Remove comments
                html = Regex.Replace(html, @"/\*[\d\D]*?\*/", string.Empty);

                //Remove <script>
                html = Regex.Replace(html, @"<script[^>]*>[\s\S]*?</script>", string.Empty);

                return html;
            }
            catch (Exception)
            {
                return html;
            }
        }

        public CommentDetailAdmin GetCommentDetail(long Id)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            CommentDetailAdmin returnCommentDetail = new CommentDetailAdmin();
            try
            {
                var cm_comment = _unitOfWork.Repository<CM_Comment>().Queryable();
                var cm_replycomment = _unitOfWork.Repository<CM_ReplyComment>().Queryable();

                returnCommentDetail.Comment = new CM_Comment();
                var comment = cm_comment.FirstOrDefault(x => x.Id == Id);
                if (comment != null)
                    returnCommentDetail.Comment = comment;
                else
                    return null;

                returnCommentDetail.Replies = new List<ReplyComment>();
                var lstReplies = cm_replycomment.Where(x => x.Comment.Id == Id)
                    .OrderBy(x => x.NgayGioTao)
                    .ToList();
                if (lstReplies.Any())
                {
                    foreach (var rep in lstReplies)
                    {
                        ReplyComment reply = new ReplyComment();
                        reply.Id = rep.Id;
                        reply.CreateDateTime = GetElapsedTime(rep.NgayGioTao);
                        reply.DTCreateDateTime = rep.NgayGioTao;
                        reply.Likes = rep.Likes;
                        reply.NoiDung = rep.NoiDung;
                        reply.ParentId = Id;
                        reply.Status = rep.Status;
                        reply.TypeName = rep.TypeName;
                        reply.TypeQuanTri = rep.ThongTinKhachHang != null ? "F" : "T";

                        reply.KhachHang = new KhachHang();
                        if (rep.ThongTinKhachHang != null)
                        {
                            reply.KhachHang.Avatar = rep.ThongTinKhachHang.Avatar;
                            reply.KhachHang.HoTen = rep.ThongTinKhachHang.HoTen;
                            reply.KhachHang.Email = rep.ThongTinKhachHang.Email;
                            reply.KhachHang.GioiTinh = rep.ThongTinKhachHang.GioiTinh;
                            reply.KhachHang.Id = rep.ThongTinKhachHang.Id;
                        }

                        returnCommentDetail.Replies.Add(reply);
                    }
                }

                var latestReply =
                    _unitOfWork.Repository<CM_ReplyComment>()
                        .Queryable()
                        .Where(x => x.CM_CommentId == comment.Id && x.Status == "A");
                if (latestReply.Any())
                    returnCommentDetail.NgayGioTraLoi =
                        latestReply.OrderByDescending(x => x.NgayGioTao).First().NgayGioTao;

                //Bo sung xem toan bo comment, reply comment
                returnCommentDetail.ListAllComments = new List<BinhLuan>();
                var lstBLByUrl = cm_comment.Where(x => x.SP_URL.Contains(comment.SP_URL) &&
                                                       x.Status == "A")
                    .OrderByDescending(x => x.NgayGioTao).ToList();
                if (lstBLByUrl.Any())
                {
                    foreach (var bl in lstBLByUrl)
                    {
                        BinhLuan binhLuan = new BinhLuan();
                        binhLuan.Id = bl.Id;
                        binhLuan.CreateDateTime = GetElapsedTime(bl.NgayGioTao);
                        binhLuan.DTCreateDateTime = bl.NgayGioTao;
                        binhLuan.NoiDung = bl.NoiDung;
                        binhLuan.TrangThai = bl.Status;
                        binhLuan.TrangThaiHeThong = bl.CM_TrangThaiId;
                        binhLuan.TypeName = bl.LoaiHienThi;

                        binhLuan.KhachHang = new KhachHang();
                        if (bl.ThongTinKhachHang != null)
                        {
                            binhLuan.KhachHang.Avatar = bl.ThongTinKhachHang.Avatar;
                            binhLuan.KhachHang.HoTen = bl.ThongTinKhachHang.HoTen;
                            binhLuan.KhachHang.Email = bl.ThongTinKhachHang.Email;
                            binhLuan.KhachHang.GioiTinh = bl.ThongTinKhachHang.GioiTinh;
                            binhLuan.KhachHang.Id = bl.ThongTinKhachHang.Id;
                        }

                        binhLuan.Replies = new List<ReplyComment>();
                        //Lay danh sach nhung replies
                        var lstRepliesAll = cm_replycomment.Where(x => x.Comment.Id == bl.Id)
                            .OrderBy(x => x.NgayGioTao)
                            .ToList();
                        if (lstRepliesAll.Any())
                        {
                            foreach (var rep in lstRepliesAll)
                            {
                                ReplyComment reply = new ReplyComment();
                                reply.Id = rep.Id;
                                reply.CreateDateTime = GetElapsedTime(rep.NgayGioTao);
                                reply.DTCreateDateTime = rep.NgayGioTao;
                                reply.Likes = rep.Likes;
                                reply.NoiDung = rep.NoiDung;
                                reply.ParentId = binhLuan.Id;
                                reply.Status = rep.Status;
                                reply.TypeName = rep.TypeName;
                                reply.TypeQuanTri = rep.ThongTinKhachHang != null ? "F" : "T";

                                if (reply.TypeQuanTri != "T")
                                {
                                    reply.KhachHang = new KhachHang();
                                    if (rep.ThongTinKhachHang != null)
                                    {
                                        reply.KhachHang.Avatar = rep.ThongTinKhachHang.Avatar;
                                        reply.KhachHang.HoTen = rep.ThongTinKhachHang.HoTen;
                                        reply.KhachHang.Email = rep.ThongTinKhachHang.Email;
                                        reply.KhachHang.GioiTinh = rep.ThongTinKhachHang.GioiTinh;
                                        reply.KhachHang.Id = rep.ThongTinKhachHang.Id;
                                    }
                                }
                                else
                                {
                                    var tblUser = _unitOfWork.Repository<User>().Queryable();
                                    if (!string.IsNullOrEmpty(rep.MaNV_VTAReply))
                                    {
                                        var userInfo =
                                            tblUser.FirstOrDefault(
                                                x =>
                                                    x.EmployeeId.Trim() == rep.MaNV_VTAReply ||
                                                    x.PersonalId.Trim() == rep.MaNV_VTAReply);
                                        if (userInfo != null)
                                        {
                                            if (!string.IsNullOrEmpty(userInfo.FullName))
                                                reply.TenQTV = GetShortName(userInfo.FullName);
                                        }

                                    }
                                }

                                binhLuan.Replies.Add(reply);
                            }
                        }
                        returnCommentDetail.ListAllComments.Add(binhLuan);
                    }
                }
                NLog.LogManager.GetCurrentClassLogger()
                    .Debug(" Get Detail Comment ver1 - total detail ({0}) la {1} giay ",
                        returnCommentDetail.Replies.Count, (object)stopwatch.Elapsed.TotalSeconds);
                return returnCommentDetail;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetCommentDetail({0}) - Mes: {1} ", Id, ex.ToString());
                return returnCommentDetail;
            }
        }

        public CommentDetailAdmin GetCommentDetailVer2(long Id)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            CommentDetailAdmin returnCommentDetail = new CommentDetailAdmin();
            ReplyComment reply = null;
            try
            {
                var cm_comment = _unitOfWork.Repository<CM_Comment>().Queryable().FirstOrDefault(x => x.Id == Id);
                var cm_replycomment =
                    _unitOfWork.Repository<CM_ReplyComment>()
                        .Queryable()
                        .Where(x => x.CM_CommentId == Id && x.Status == "A")
                        .OrderBy(x => x.NgayGioTao)
                        .ToList();
                if (cm_comment != null)
                    returnCommentDetail.Comment = cm_comment;
                else
                    return null;


                returnCommentDetail.Replies = new List<ReplyComment>();


                if (cm_replycomment.Any())
                {
                    foreach (var rep in cm_replycomment)
                    {
                        reply = new ReplyComment();
                        reply.Id = rep.Id;
                        reply.CreateDateTime = GetElapsedTime(rep.NgayGioTao);
                        reply.DTCreateDateTime = rep.NgayGioTao;
                        reply.Likes = rep.Likes;
                        reply.NoiDung = rep.NoiDung;
                        reply.ParentId = Id;
                        reply.Status = rep.Status;
                        reply.TypeName = rep.TypeName;
                        reply.TypeQuanTri = rep.ThongTinKhachHang != null ? "F" : "T";

                        reply.KhachHang = new KhachHang();
                        if (rep.ThongTinKhachHang != null)
                        {
                            reply.KhachHang.Avatar = rep.ThongTinKhachHang.Avatar;
                            reply.KhachHang.HoTen = rep.ThongTinKhachHang.HoTen;
                            reply.KhachHang.Email = rep.ThongTinKhachHang.Email;
                            reply.KhachHang.GioiTinh = rep.ThongTinKhachHang.GioiTinh;
                            reply.KhachHang.Id = rep.ThongTinKhachHang.Id;
                        }


                        returnCommentDetail.Replies.Add(reply);
                        reply = null;
                    }
                }


                if (cm_replycomment.Any())
                    returnCommentDetail.NgayGioTraLoi =
                        cm_replycomment.OrderByDescending(x => x.NgayGioTao).First().NgayGioTao;


                NLog.LogManager.GetCurrentClassLogger()
                    .Debug(" Get Detail Comment ver2 - total detail ({0}) la {1} giay ",
                        returnCommentDetail.Replies.Count, (object)stopwatch.Elapsed.TotalSeconds);
                return returnCommentDetail;

            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger()
                    .Debug("Get Detail Comment ver2 ({0}) - Mes: {1} ", Id, ex.ToString());
                return returnCommentDetail;
            }
        }

        public CommentDetailAdmin GetListAllCommentDetail(long Id)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            CommentDetailAdmin returnCommentDetail = new CommentDetailAdmin();
            returnCommentDetail.ListAllComments = new List<BinhLuan>();
            returnCommentDetail.Comment = new CM_Comment();
            try
            {

                var cm_comment = _unitOfWork.Repository<CM_Comment>().Queryable().Select(

                   x => new { Id = x.Id, NgayGioTao = x.NgayGioTao, NoiDung = x.NoiDung, SP_TenSP = x.SP_TenSP, SP_URL = x.SP_URL, Status = x.Status }

            ).OrderByDescending(x => x.NgayGioTao).ToList();
                var comment = cm_comment.FirstOrDefault(x => x.Id == Id);
                returnCommentDetail.Comment.SP_TenSP = comment.SP_URL;
                var lstBLByUrl = cm_comment.Where(x => x.SP_URL.Contains(comment.SP_URL) &&
                                                     x.Status == "A")
                                         .OrderByDescending(x => x.NgayGioTao).ToList();

                if (lstBLByUrl.Any())
                {
                    foreach (var bl in lstBLByUrl)
                    {
                        BinhLuan binhLuan = new BinhLuan();

                        binhLuan.NoiDung = bl.NoiDung;
                        returnCommentDetail.ListAllComments.Add(binhLuan);

                    }

                }

                NLog.LogManager.GetCurrentClassLogger().Debug(" GetAllCommentVer2 - total detail ({0}) la {1} giay ", "TEST", (object)stopwatch.Elapsed.TotalSeconds);
                return returnCommentDetail;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetAllCommentVer2({0}) - Mes: {1} ", Id, ex.ToString());
                return returnCommentDetail;
            }

        }


        public bool ResetData(ResetDataParam param)
        {
            //return true;
            try
            {
                //Lay danh sach comment can reset
                var cm_nhanvientiepnhan = _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Queryable().Where(x => x.Status == "A" && x.NgayGioTraLoi == null && (!string.IsNullOrEmpty(param.PhongBan) ? x.PhongBan.Trim() == param.PhongBan.Trim() : 1 == 1)).ToList();
                if (cm_nhanvientiepnhan != null && cm_nhanvientiepnhan.Any())
                {
                    _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().DeleteRange(cm_nhanvientiepnhan);
                    _unitOfWork.Save();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("ResetData() - Mes: ({0}) ", ex.ToString());
                return false;
            }
        }

        public CommentVtaSite GetListCommentVtaSite(string lastSlut)
        {
            CommentVtaSite returnCommentVtaSite = new CommentVtaSite();
            try
            {
                lastSlut = HttpUtility.UrlDecode(lastSlut);

                returnCommentVtaSite.SP_UrlSP = lastSlut;
                returnCommentVtaSite.SP_MaSP = String.Empty;
                returnCommentVtaSite.SP_TenSP = String.Empty;

                var cm_comment = _unitOfWork.Repository<CM_Comment>().Queryable();
                var cm_replycomment = _unitOfWork.Repository<CM_ReplyComment>().Queryable();

                //Lay loai DG
                returnCommentVtaSite.DanhGia = new List<DanhGia>();
                var lstDGByUrl = cm_comment.Where(x => x.SP_URL.Contains(lastSlut) &&
                                                        x.LoaiHienThi == "DG" &&
                                                        x.Status == "A" &&
                                                        x.TrangThai.Id != 8) //An
                                            .OrderByDescending(x => x.NgayGioTao).ToList();
                if (lstDGByUrl.Any())
                {
                    foreach (var dg in lstDGByUrl)
                    {
                        DanhGia danhGia = new DanhGia();
                        danhGia.Status = dg.Status;
                        danhGia.CreateDateTime = GetElapsedTime(dg.NgayGioTao);
                        danhGia.DTCreateDateTime = dg.NgayGioTao;
                        danhGia.Id = dg.Id;
                        danhGia.Likes = dg.Likes;
                        danhGia.NoiDung = dg.NoiDung;
                        danhGia.Rating = dg.Rating;
                        danhGia.TypeName = "DG";

                        danhGia.KhachHang = new KhachHang();
                        if (dg.ThongTinKhachHang != null)
                        {
                            danhGia.KhachHang.Avatar = dg.ThongTinKhachHang.Avatar;
                            danhGia.KhachHang.HoTen = dg.ThongTinKhachHang.HoTen;
                            danhGia.KhachHang.Email = dg.ThongTinKhachHang.Email;
                            danhGia.KhachHang.GioiTinh = dg.ThongTinKhachHang.GioiTinh;
                            danhGia.KhachHang.Id = dg.ThongTinKhachHang.Id;
                        }

                        danhGia.Replies = new List<ReplyComment>();
                        //Lay danh sach nhung replies
                        var lstReplies = cm_replycomment.Where(x => x.Comment.Id == dg.Id)
                                .OrderBy(x => x.NgayGioTao)
                                .ToList();
                        if (lstReplies.Any())
                        {
                            foreach (var rep in lstReplies)
                            {
                                ReplyComment reply = new ReplyComment();
                                reply.Id = rep.Id;
                                reply.CreateDateTime = GetElapsedTime(rep.NgayGioTao);
                                reply.DTCreateDateTime = rep.NgayGioTao;
                                reply.Likes = rep.Likes;
                                reply.NoiDung = rep.NoiDung;
                                reply.ParentId = danhGia.Id;
                                reply.Status = rep.Status;
                                reply.TypeName = rep.TypeName;
                                reply.TypeQuanTri = rep.ThongTinKhachHang != null ? "F" : "T";

                                reply.KhachHang = new KhachHang();
                                if (rep.ThongTinKhachHang != null)
                                {
                                    reply.KhachHang.Avatar = rep.ThongTinKhachHang.Avatar;
                                    reply.KhachHang.HoTen = rep.ThongTinKhachHang.HoTen;
                                    reply.KhachHang.Email = rep.ThongTinKhachHang.Email;
                                    reply.KhachHang.GioiTinh = rep.ThongTinKhachHang.GioiTinh;
                                    reply.KhachHang.Id = rep.ThongTinKhachHang.Id;
                                }

                                danhGia.Replies.Add(reply);
                            }
                        }
                        returnCommentVtaSite.DanhGia.Add(danhGia);
                    }
                }

                //Lay loai Comment
                returnCommentVtaSite.BinhLuan = new List<BinhLuan>();
                var lstBLByUrl = cm_comment.Where(x => x.SP_URL.Contains(lastSlut) &&
                                                        x.LoaiHienThi == "CM" &&
                                                        x.Status == "A" &&
                                                        x.TrangThai.Id != 8) //An
                                            .OrderByDescending(x => x.NgayGioTao).ToList();
                if (lstBLByUrl.Any())
                {
                    foreach (var bl in lstBLByUrl)
                    {
                        BinhLuan binhLuan = new BinhLuan();
                        binhLuan.Id = bl.Id;
                        binhLuan.CreateDateTime = GetElapsedTime(bl.NgayGioTao);
                        binhLuan.DTCreateDateTime = bl.NgayGioTao;
                        binhLuan.NoiDung = bl.NoiDung;
                        binhLuan.TrangThai = bl.Status;
                        binhLuan.TypeName = "CM";

                        binhLuan.KhachHang = new KhachHang();
                        if (bl.ThongTinKhachHang != null)
                        {
                            binhLuan.KhachHang.Avatar = bl.ThongTinKhachHang.Avatar;
                            binhLuan.KhachHang.HoTen = bl.ThongTinKhachHang.HoTen;
                            binhLuan.KhachHang.Email = bl.ThongTinKhachHang.Email;
                            binhLuan.KhachHang.GioiTinh = bl.ThongTinKhachHang.GioiTinh;
                            binhLuan.KhachHang.Id = bl.ThongTinKhachHang.Id;
                        }

                        binhLuan.Replies = new List<ReplyComment>();
                        //Lay danh sach nhung replies
                        var lstReplies = cm_replycomment.Where(x => x.Comment.Id == bl.Id)
                                .OrderByDescending(x => x.NgayGioTao)
                                .ToList();
                        if (lstReplies.Any())
                        {
                            foreach (var rep in lstReplies)
                            {
                                ReplyComment reply = new ReplyComment();
                                reply.Id = rep.Id;
                                reply.CreateDateTime = GetElapsedTime(rep.NgayGioTao);
                                reply.DTCreateDateTime = rep.NgayGioTao;
                                reply.Likes = rep.Likes;
                                reply.NoiDung = rep.NoiDung;
                                reply.ParentId = binhLuan.Id;
                                reply.Status = rep.Status;
                                reply.TypeName = rep.TypeName;
                                reply.TypeQuanTri = rep.ThongTinKhachHang != null ? "F" : "T";

                                reply.KhachHang = new KhachHang();
                                if (rep.ThongTinKhachHang != null)
                                {
                                    reply.KhachHang.Avatar = rep.ThongTinKhachHang.Avatar;
                                    reply.KhachHang.HoTen = rep.ThongTinKhachHang.HoTen;
                                    reply.KhachHang.Email = rep.ThongTinKhachHang.Email;
                                    reply.KhachHang.GioiTinh = rep.ThongTinKhachHang.GioiTinh;
                                    reply.KhachHang.Id = rep.ThongTinKhachHang.Id;
                                }

                                binhLuan.Replies.Add(reply);
                            }
                        }
                        returnCommentVtaSite.BinhLuan.Add(binhLuan);
                    }
                }

                return returnCommentVtaSite;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetListCommentVtaSite({0}) - Mes: {1} ", lastSlut, ex.ToString());
                return returnCommentVtaSite;
            }
        }

        public CommentVtaSite GetListCommentVtaSiteV2(string pageDG, string pageBL, string lastSlut)
        {
            CommentVtaSite returnCommentVtaSite = new CommentVtaSite();
            try
            {
                lastSlut = HttpUtility.UrlDecode(lastSlut.Trim());
                returnCommentVtaSite.SP_UrlSP = lastSlut;

                var cm_comment = _unitOfWork.Repository<CM_Comment>().Queryable();
                var cm_replycomment = _unitOfWork.Repository<CM_ReplyComment>().Queryable();
                var tblUser = _unitOfWork.Repository<User>().Queryable().ToList();

                var lstDgAndBl = cm_comment.Where(x => x.SP_URL.Contains(lastSlut) &&
                                                        x.TrangThai.Id != 8 &&
                                                        x.Status == "A")
                                            .OrderByDescending(x => x.NgayGioTao).ToList();
                var lstDgAndBlIds = lstDgAndBl != null ? lstDgAndBl.Select(x => x.Id).ToList() : null;

                var lstReplieDGAndBLs = lstDgAndBlIds != null ? cm_replycomment.Where(x => lstDgAndBlIds.Contains(x.Comment.Id)).OrderBy(x => x.NgayGioTao).ToList() : null;


                var lstDGByUrl = lstDgAndBl.Where(x => x.LoaiHienThi == "DG").ToList();
                var lstBLByUrl = lstDgAndBl.Where(x => x.LoaiHienThi == "CM").ToList();

                #region Lay loai DG
                returnCommentVtaSite.DanhGia = new List<DanhGia>();

                returnCommentVtaSite.TotalDG = lstDGByUrl.Count;
                returnCommentVtaSite.TotalPageDG = (Convert.ToInt32(lstDGByUrl.Count % 10) != 0) ? (lstDGByUrl.Count / 10) + 1 : (lstDGByUrl.Count / 10);
                if (!string.IsNullOrEmpty(pageDG))
                {
                    int pageDg = Convert.ToInt32(pageDG);
                    if (pageDg == 0)
                        pageDg = 1;
                    lstDGByUrl = lstDGByUrl.Skip((pageDg - 1) * 10).Take(10).ToList();

                    returnCommentVtaSite.CurrentPageDG = pageDg;
                }

                if (lstDGByUrl.Any())
                {
                    foreach (var dg in lstDGByUrl)
                    {
                        DanhGia danhGia = new DanhGia();
                        danhGia.Status = dg.Status;
                        danhGia.TrangThaiHeThong = dg.CM_TrangThaiId;
                        danhGia.CreateDateTime = GetElapsedTime(dg.NgayGioTao);
                        danhGia.DTCreateDateTime = dg.NgayGioTao;
                        danhGia.Id = dg.Id;
                        danhGia.Likes = dg.Likes;
                        danhGia.NoiDung = DetectAndHidePhoneNumber(dg.NoiDung);
                        danhGia.Rating = dg.Rating;
                        danhGia.TypeName = "DG";

                        danhGia.KhachHang = new KhachHang();
                        if (dg.ThongTinKhachHang != null)
                        {
                            danhGia.KhachHang.Avatar = dg.ThongTinKhachHang.Avatar;
                            danhGia.KhachHang.HoTen = dg.ThongTinKhachHang.HoTen;
                            danhGia.KhachHang.Email = dg.ThongTinKhachHang.Email;
                            danhGia.KhachHang.GioiTinh = dg.ThongTinKhachHang.GioiTinh;
                            danhGia.KhachHang.Id = dg.ThongTinKhachHang.Id;
                        }

                        danhGia.Replies = new List<ReplyComment>();

                        var lstReplieDGsForCurrentCM = !lstReplieDGAndBLs.Any() ? null : lstReplieDGAndBLs.Where(x => x.Comment.Id == dg.Id).OrderBy(x => x.NgayGioTao).ToList();
                        if (lstReplieDGsForCurrentCM != null)
                        {
                            foreach (var rep in lstReplieDGsForCurrentCM)
                            {
                                ReplyComment reply = new ReplyComment();
                                reply.Id = rep.Id;
                                reply.CreateDateTime = GetElapsedTime(rep.NgayGioTao);
                                reply.DTCreateDateTime = rep.NgayGioTao;
                                reply.Likes = rep.Likes;
                                reply.NoiDung = rep.NoiDung;
                                reply.ParentId = danhGia.Id;
                                reply.Status = rep.Status;
                                reply.TypeName = rep.TypeName;
                                reply.TypeQuanTri = rep.ThongTinKhachHang != null ? "F" : "T";

                                if (reply.TypeQuanTri != "T")
                                {
                                    reply.KhachHang = new KhachHang();
                                    if (rep.ThongTinKhachHang != null)
                                    {
                                        reply.KhachHang.Avatar = rep.ThongTinKhachHang.Avatar;
                                        reply.KhachHang.HoTen = rep.ThongTinKhachHang.HoTen;
                                        reply.KhachHang.Email = rep.ThongTinKhachHang.Email;
                                        reply.KhachHang.GioiTinh = rep.ThongTinKhachHang.GioiTinh;
                                        reply.KhachHang.Id = rep.ThongTinKhachHang.Id;
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(rep.MaNV_VTAReply))
                                    {
                                        var userInfo = tblUser.FirstOrDefault(x => (!string.IsNullOrEmpty(x.EmployeeId) && x.EmployeeId.ToString().Trim() == rep.MaNV_VTAReply.ToString().Trim()) || (!string.IsNullOrEmpty(x.PersonalId) && x.PersonalId.ToString().Trim() == rep.MaNV_VTAReply.ToString().Trim()));
                                        if (!string.IsNullOrEmpty(userInfo?.FullName))
                                        {
                                            reply.TenQTV = GetShortName(userInfo.FullName);
                                            reply.AvatarQTV = userInfo.Avatar;
                                        }
                                    }
                                }

                                danhGia.Replies.Add(reply);
                            }
                        }
                        returnCommentVtaSite.DanhGia.Add(danhGia);
                    }
                }
                #endregion

                //Lay loai Comment
                returnCommentVtaSite.BinhLuan = new List<BinhLuan>();


                returnCommentVtaSite.TotalBL = lstBLByUrl.Count;
                returnCommentVtaSite.TotalPageBL = (Convert.ToInt32(lstBLByUrl.Count % 10) != 0) ? (lstBLByUrl.Count / 10) + 1 : (lstBLByUrl.Count / 10);
                if (!string.IsNullOrEmpty(pageBL))
                {
                    int pageBl = Convert.ToInt32(pageBL);
                    if (pageBl == 0)
                        pageBl = 1;
                    lstBLByUrl = lstBLByUrl.Skip((pageBl - 1) * 10).Take(10).ToList();

                    returnCommentVtaSite.CurrentPageBL = pageBl;
                }

                if (lstBLByUrl.Any())
                {
                    foreach (var bl in lstBLByUrl)
                    {
                        BinhLuan binhLuan = new BinhLuan();
                        binhLuan.Id = bl.Id;
                        binhLuan.CreateDateTime = GetElapsedTime(bl.NgayGioTao);
                        binhLuan.DTCreateDateTime = bl.NgayGioTao;
                        binhLuan.NoiDung = DetectAndHidePhoneNumber(bl.NoiDung);
                        binhLuan.TrangThai = bl.Status;
                        binhLuan.TrangThaiHeThong = bl.CM_TrangThaiId;
                        binhLuan.TypeName = "CM";

                        binhLuan.KhachHang = new KhachHang();
                        if (bl.ThongTinKhachHang != null)
                        {
                            binhLuan.KhachHang.Avatar = bl.ThongTinKhachHang.Avatar;
                            binhLuan.KhachHang.HoTen = bl.ThongTinKhachHang.HoTen;
                            binhLuan.KhachHang.Email = bl.ThongTinKhachHang.Email;
                            binhLuan.KhachHang.GioiTinh = bl.ThongTinKhachHang.GioiTinh;
                            binhLuan.KhachHang.Id = bl.ThongTinKhachHang.Id;
                        }

                        binhLuan.Replies = new List<ReplyComment>();

                        //Lay danh sach nhung replies
                        var lstReplieBLsForCurrentCM = !lstReplieDGAndBLs.Any() ? null : lstReplieDGAndBLs.Where(x => x.Comment.Id == bl.Id).OrderBy(x => x.NgayGioTao).ToList();
                        if (lstReplieBLsForCurrentCM != null)
                        {
                            foreach (var rep in lstReplieBLsForCurrentCM)
                            {
                                ReplyComment reply = new ReplyComment();
                                reply.Id = rep.Id;
                                reply.CreateDateTime = GetElapsedTime(rep.NgayGioTao);
                                reply.DTCreateDateTime = rep.NgayGioTao;
                                reply.Likes = rep.Likes;
                                reply.NoiDung = rep.NoiDung;
                                reply.ParentId = binhLuan.Id;
                                reply.Status = rep.Status;
                                reply.TypeName = rep.TypeName;
                                reply.TypeQuanTri = rep.ThongTinKhachHang != null ? "F" : "T";

                                if (reply.TypeQuanTri != "T")
                                {
                                    reply.KhachHang = new KhachHang();
                                    if (rep.ThongTinKhachHang != null)
                                    {
                                        reply.KhachHang.Avatar = rep.ThongTinKhachHang.Avatar;
                                        reply.KhachHang.HoTen = rep.ThongTinKhachHang.HoTen;
                                        reply.KhachHang.Email = rep.ThongTinKhachHang.Email;
                                        reply.KhachHang.GioiTinh = rep.ThongTinKhachHang.GioiTinh;
                                        reply.KhachHang.Id = rep.ThongTinKhachHang.Id;
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(rep.MaNV_VTAReply))
                                    {
                                        var userInfo = tblUser.FirstOrDefault(x => (!string.IsNullOrEmpty(x.EmployeeId) && x.EmployeeId.ToString().Trim() == rep.MaNV_VTAReply.ToString().Trim()) || (!string.IsNullOrEmpty(x.PersonalId) && x.PersonalId.ToString().Trim() == rep.MaNV_VTAReply.ToString().Trim()));
                                        if (!string.IsNullOrEmpty(userInfo?.FullName))
                                        {
                                            reply.TenQTV = GetShortName(userInfo.FullName);
                                            reply.AvatarQTV = userInfo.Avatar;
                                        }
                                    }
                                }

                                binhLuan.Replies.Add(reply);
                            }
                        }
                        returnCommentVtaSite.BinhLuan.Add(binhLuan);
                    }
                }

                return returnCommentVtaSite;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetListCommentVtaSiteV2({0}) - Mes: {1} ", lastSlut, ex.ToString());
                return returnCommentVtaSite;
            }
        }

        public static string GetShortName(string fullName)
        {
            string result = string.Empty;
            try
            {
                var nameArr = fullName.Split(' ');
                if (nameArr.Length <= 2)
                {
                    result = fullName;
                }
                else
                {
                    result = nameArr.ElementAtOrDefault(nameArr.Length - 2).Trim() + " " + nameArr.Last().Trim();
                }

                return result;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetShortName({0}) - Mes: {1}", ex.Message, ex.StackTrace);
                return fullName;
            }
        }

        public static string DetectAndHidePhoneNumber(string phoneNumberString)
        {
            string result = phoneNumberString;
            try
            {
                PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
                var phoneNums = phoneUtil.FindNumbers(phoneNumberString, "VN");

                if (phoneNums != null)
                {
                    foreach (var number in phoneNums)
                    {
                        result = result.Replace(number.RawString, number.RawString.Remove(number.RawString.Length - 3, 3) + "xxx");
                    }
                }

                //var matches = Regex.Matches(phoneNumberString, "[0-9]+");

                //foreach (var match in matches)
                //{
                //    var matchTrimed = match.ToString().Trim();
                //    if (matchTrimed.Length == 10 || matchTrimed.Length == 11)
                //    {
                //        if(PhoneNumbers.)
                //    }

                //}

                // var lstNumber = PhoneNumbers.PhoneNumber.

                return result;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("DetectAndHidePhoneNumber({0}) - Mes: {1}", ex.Message, ex.StackTrace);
                return phoneNumberString;
            }
        }

        public CheckRepliedReponse CheckRepliedCommentVtaSite(string lstCommentId)
        {
            CheckRepliedReponse replyResponse = new CheckRepliedReponse();
            try
            {
                if (!string.IsNullOrEmpty(lstCommentId))
                {
                    var cm_replycomment = _unitOfWork.Repository<CM_ReplyComment>().Queryable();

                    var lstCommentIds = lstCommentId.Split(',');
                    if (lstCommentIds.Any())
                    {
                        replyResponse.CheckReplied = new List<ShortReply>();
                        for (int i = 0; i < lstCommentIds.Count(); i++)
                        {
                            long? commentId = Convert.ToUInt32(lstCommentIds.ElementAtOrDefault(i));
                            var reply = cm_replycomment.Where(x => x.CM_CommentId == commentId && x.Status == "A")
                                            .OrderByDescending(x => x.NgayGioTao)
                                            .FirstOrDefault();

                            if (reply != null && reply.MaNV_VTAReply != null)
                            {
                                ShortReply shotReply = new ShortReply();
                                shotReply.NgayGioReply = reply.NgayGioTao;
                                shotReply.NoiDung = reply.NoiDung;
                                shotReply.CommentId = reply.Comment.Id;
                                var tblUser = _unitOfWork.Repository<User>().Queryable();
                                var userInfo = tblUser.FirstOrDefault(x => x.EmployeeId.Trim() == reply.MaNV_VTAReply || x.PersonalId == reply.MaNV_VTAReply);
                                if (userInfo != null)
                                {
                                    shotReply.TenNVReply = GetShortName(userInfo.FullName);
                                }

                                shotReply.LastSlug = reply.Comment.SP_URL;

                                replyResponse.CheckReplied.Add(shotReply);
                            }
                        }
                    }
                }
                return replyResponse;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("CheckRepliedCommentVtaSite - Mes: {0} ", ex.ToString());
                return replyResponse;
            }
        }

        public List<EmployeeInfoModel> SearchEmployee(string maNhanVien)
        {
            List<EmployeeInfoModel> employeeInfo = new List<EmployeeInfoModel>();

            try
            {
                var client = new RestClient(Globals.VTAWebAPILocal);
                var req = new RestRequest("mobileapp-api/GetEmployeeInfoFromCitrix?term=" + maNhanVien, Method.GET) { RequestFormat = DataFormat.Json };
                var response = client.Execute<HttpContentResult<EmployeeInfoModel>>(req).Data;
                if (response.Result && response.Data != null)
                {
                    employeeInfo.Add(response.Data);
                }
                return employeeInfo;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("SearchEmployee - Mes: {0} ", ex.ToString());
                return employeeInfo;
            }
        }

        public string RegisterUser(RegisterEmployeeInfo empInfo)
        {
            string result = string.Empty;
            try
            {
                var client = new RestClient(Globals.VTAWebAPILocal);
                var req = new RestRequest("mobileapp-api/GetEmployeeInfoFromCitrix?term=" + empInfo.UsernameCitrix, Method.GET) { RequestFormat = DataFormat.Json };
                var response = client.Execute<HttpContentResult<EmployeeInfoModel>>(req).Data;
                if (response.Result)
                {
                    var userInfo = new UserModel();
                    userInfo.UserName = string.IsNullOrEmpty(response.Data.EMAIL)
                        ? string.Empty
                        : response.Data.EMAIL.Replace("@vienthonga.com", String.Empty).Trim();

                    userInfo.Password = "Vt@2016";

                    userInfo.FullName = string.IsNullOrEmpty(response.Data.FullName)
                        ? string.Empty
                        : response.Data.FullName.Trim();

                    userInfo.Email = string.IsNullOrEmpty(response.Data.EMAIL)
                        ? string.Empty
                        : response.Data.EMAIL.Trim();

                    userInfo.PersonalId = string.IsNullOrEmpty(response.Data.EmpID)
                        ? string.Empty
                        : response.Data.EmpID.Trim();

                    userInfo.EmployeeId = string.IsNullOrEmpty(response.Data.EmpID)
                        ? string.Empty
                        : response.Data.EmpID.Trim();

                    userInfo.CreateDate = DateTime.Now;
                    //userInfo.ListRole = String.Empty;
                    userInfo.Roles = new List<UserInRole>();
                    userInfo.Status = "Active";
                    userInfo.PhongBan = empInfo.PhongBan;
                    userInfo.Avatar = empInfo.Avatar;
                    userInfo.ChucDanh = string.IsNullOrEmpty(response.Data.TitleName)
                        ? string.Empty
                        : response.Data.TitleName.Trim();

                    var resp = _userService.CreateUser(userInfo);
                    if (resp.Successfully)
                    {
                        //////Add role
                        var user = _unitOfWork.Repository<User>().Queryable().FirstOrDefault(x => x.EmployeeId == empInfo.EmpID || x.PersonalId == empInfo.EmpID);
                        if (user != null)
                        {
                            _unitOfWork.Repository<UserInRole>().Insert(new UserInRole() { UserId = user.Id, RoleId = empInfo.RoleId });
                            _unitOfWork.Save();
                        }

                        result = String.Format("SUCCESS: {0}", resp.Message);
                    }
                    else
                    {
                        result = String.Format("ERROR: {0}", resp.Message);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //result = $"ERROR: {ex.Message.ToString()}";
                result = String.Format("ERROR: {0}", ex.Message);
                NLog.LogManager.GetCurrentClassLogger().Debug("RegisterUser - Mes: {0} ", ex.Message.ToString());
                return result;
            }
        }

        public bool UpdateUser(string maNV, UpdateUserDataPost userInfo)
        {
            try
            {
                var cm_user = _unitOfWork.Repository<User>().Queryable();
                var user = cm_user.FirstOrDefault(x => x.EmployeeId == maNV || x.PersonalId == maNV);
                if (user != null)
                {
                    user.Status = userInfo.Status.Trim();
                    user.Avatar = userInfo.Avatar;
                    if (!string.IsNullOrEmpty(userInfo.RoleId))
                    {
                        var currentRole =
                           _unitOfWork.Repository<UserInRole>()
                               .Queryable()
                               .FirstOrDefault(x => x.UserId.Equals(user.Id.Trim()) && x.Role.Code.Contains(user.PhongBan));
                        if (currentRole != null)
                        {
                            //Delete current role
                            if (currentRole.RoleId != userInfo.RoleId)
                            {
                                _unitOfWork.Repository<UserInRole>().Delete(currentRole);
                                _unitOfWork.Repository<UserInRole>().Insert(new UserInRole() { UserId = user.Id, RoleId = userInfo.RoleId });
                            }
                        }
                        else
                        {
                            _unitOfWork.Repository<UserInRole>().Insert(new UserInRole() { UserId = user.Id, RoleId = userInfo.RoleId });
                        }
                    }

                    _unitOfWork.Save();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("UpdateUser() - Mes: ({0}) ", ex.ToString());
                return false;
            }
        }

        public List<CM_UserLoginHistory> GetListLogInHistory(string MaNV)
        {
            List<CM_UserLoginHistory> returnData = new List<CM_UserLoginHistory>();
            try
            {
                var cm_UserLogInHistory = _unitOfWork.Repository<CM_UserLoginHistory>().Queryable();
                returnData = cm_UserLogInHistory.Where(x => x.MaNV == MaNV).OrderByDescending(x => x.Id).ToList();
                return returnData;
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("GetListLogInHistory() - Mes: {0} ", ex.ToString());
                return null;
            }
        }

        private static string GetElapsedTime(DateTime datetime)
        {
            TimeSpan ts = DateTime.Now.Subtract(datetime);

            // The trick: make variable contain date and time representing the desired timespan,
            // having +1 in each date component.
            DateTime date = DateTime.MinValue + ts;

            return ProcessPeriod(date.Year - 1, date.Month - 1, "năm")
                   ?? ProcessPeriod(date.Month - 1, date.Day - 1, "tháng")
                   ?? ProcessPeriod(date.Day - 1, date.Hour, "ngày", "Hôm qua")
                   ?? ProcessPeriod(date.Hour, date.Minute, "giờ")
                   ?? ProcessPeriod(date.Minute, date.Second, "phút")
                   ?? ProcessPeriod(date.Second, 0, "giây")
                   ?? "Bây giờ";
        }

        private static string ProcessPeriod(int value, int subValue, string name, string singularName = null)
        {
            if (value == 0)
            {
                return null;
            }
            if (value == 1)
            {
                if (!String.IsNullOrEmpty(singularName))
                {
                    return singularName;
                }
                string articleSuffix = name[0] == 'h' ? "n" : String.Empty;

                string result = String.Format("Cách đây {0} {1}", articleSuffix, name);
                if (result.Trim() == "Cách đây  giờ")
                    result = "Cách đây 1 giờ";
                if (result.Trim() == "Cách đây  phút")
                    result = "Cách đây 1 phút";
                if (result.Trim() == "Cách đây  ngày")
                    result = "Cách đây 1 ngày";
                if (result.Trim() == "Cách đây  giây")
                    result = "Cách đây 1 giây";
                if (result.Trim() == "Cách đây  tháng")
                    result = "Cách đây 1 tháng";
                return result;
            }
            return String.Format("Cách đây {0} {1}", value, name);
        }

        public int CheckUserInRoleRole(string userId, string rolecode)
        {

            var currentRole = _unitOfWork.Repository<UserInRole>()
                                .Queryable()
                                .FirstOrDefault(x => x.UserId.Equals(userId) && x.Role.Code.Contains(rolecode));
            NLog.LogManager.GetCurrentClassLogger()
                .Debug("CheckUserInRoleRole():  currentRole - {0} ", currentRole);
            if (currentRole != null)
            {
                return 1;
            }
            return 0;
        }
        public int CMCheckUserLogedInAndInCa(string EmployeeId, int currentCa)
        {
            var currentLoggedIn = _unitOfWork.Repository<CM_UserLoginHistory>()
                              .Queryable()
                              .Where(x => x.MaNV == EmployeeId.Trim()).OrderByDescending(x => x.LogInTime).FirstOrDefault();
            if (currentLoggedIn != null)
            {
                if (currentLoggedIn.LogOutTime == null)  //Van con dang trong phien lam viec
                {
                    if (currentCa == 3) //ov
                    {
                        var curtCa = _unitOfWork.Repository<CM_LichLamViec>()
                            .Queryable()
                            .Where(x => x.Ca == 1 || x.Ca == 2
                            && x.MaNV == EmployeeId
                            && x.NgayLamViec == DateTime.Today
                            ).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (curtCa != null)
                            return 1;
                    }
                    else
                    {
                        //Van con trong ca cua ngay hom nay
                        var curtCa = _unitOfWork.Repository<CM_LichLamViec>()
                                  .Queryable()
                                  .Where(x => x.Ca == currentCa
                                  && x.MaNV == EmployeeId
                                  && x.NgayLamViec == DateTime.Today
                                  ).OrderByDescending(x => x.Id).FirstOrDefault();
                        if (curtCa != null)
                            return 1;
                    }

                }

            }
            return 0;
        }

        public bool GetCurrentPendingComment(string EmployeeId)
        {
            var lstCurrentPending = _unitOfWork.Repository<CM_NhanVienTiepNhanComment>()
                              .Queryable()
                              .Where(x => x.MaNVTiepNhan_VTA == EmployeeId.Trim()
                              && x.Status == "A"
                              && x.Comment.TrangThai.Id == 1
                              && x.NgayGioTraLoi == null).ToList();
            NLog.LogManager.GetCurrentClassLogger()
                 .Debug("GetCurrentPendingComment():  lstCurrentPending - Count - {0} ", lstCurrentPending.Count());
            return lstCurrentPending.Count() < 75;

        }

        private string AssignCurrentCommentForAvalableUser(string _PhongBan, long CommentId)
        {
            try
            {
                var currentCa = 1;

                TimeSpan now = DateTime.Now.TimeOfDay;
                TimeSpan start1 = new TimeSpan(8, 0, 0); //10 o'clock
                TimeSpan end1 = new TimeSpan(15, 0, 0); //12 o'clock

                TimeSpan start2 = new TimeSpan(16, 00, 0); //10 o'clock
                TimeSpan end2 = new TimeSpan(22, 00, 0); //12 o'clock

                TimeSpan start3 = new TimeSpan(15, 0, 0); //10 o'clock
                TimeSpan end3 = new TimeSpan(16, 0, 0); //12 o'clock

                if ((now >= start1) && (now <= end1))
                {
                    currentCa = 1;
                }
                else if ((now > start2) && (now <= end2))
                {
                    currentCa = 2;
                }
                else if ((now > start3) && (now <= end3))
                {
                    currentCa = 3;
                }

                var today = DateTime.Today;

                var cmLichLamViec = _unitOfWork.Repository<CM_LichLamViec>().Queryable().Where(x => x.NgayLamViec.Value == today).Select(x => x.MaNV).ToList();
                var aspNetUsers = _unitOfWork.Repository<User>().Queryable().Where(x => x.PhongBan.Trim() == _PhongBan.Trim() && cmLichLamViec.Contains(x.EmployeeId) && x.Status == "Active").ToList();

                List<User> selectUserIds = new List<User>();
                foreach (var user in aspNetUsers)
                {
                    if (CheckUserInRoleRole(user.Id, _PhongBan == "CS" ? "CM_NhanvienCS" : "CM_NhanvienSO") == 1 &&
                        (string.IsNullOrEmpty(user.EmployeeId)
                            ? CMCheckUserLogedInAndInCa(user.PersonalId, currentCa)
                            : CMCheckUserLogedInAndInCa(user.EmployeeId, currentCa)) == 1 &&
                        GetCurrentPendingComment(user.EmployeeId))
                    {
                        selectUserIds.Add(user);
                    }
                }

                var selectUserId = selectUserIds.OrderBy(x => x.LastAssign).FirstOrDefault();
                if (selectUserId != null)
                {
                    if (AssignComment(_PhongBan, CommentId, selectUserId.EmployeeId))
                        return "SUCCESS-Assigned sucessfully!";
                    return "FAILED-Not found user in table User for assign!";
                }
                return "SUCCESS-Not found user!";

            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("AssignCurrentCommentForAvalableUser():  Message - {0} StackTrace: {1}", ex.Message, ex.StackTrace);
                return $"ERROR-{ex.Message}-{ex.StackTrace}";
            }
        }

        public string AutotoolProcess()
        {
            try
            {

                //Testing
                //AssignCurrentCommentForAvalableUser("CS", 193);

                TimeSpan now = DateTime.Now.TimeOfDay;
                TimeSpan start = new TimeSpan(8, 0, 0); //10 o'clock
                TimeSpan end = new TimeSpan(22, 0, 0); //12 o'clock

                if (!((now >= start) && (now <= end)))
                {
                    NLog.LogManager.GetCurrentClassLogger().Debug("AutotoolProcess():  NOT IN WORKING HOUR!");
                    return $"SUCCESS-NOT IN WORKING HOUR";
                }



                var tblNhanVienTiepNhan = _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Queryable();
                var tblComment = _unitOfWork.Repository<CM_Comment>().Queryable();


                var last2Days = DateTime.Today.AddDays(-2);

                var lstCommentCanChia = tblComment.Where(x => x.CM_TrangThaiId == 1 && x.Status == "A" && x.NgayGioTao >= last2Days).ToList();
                var commentIdInNhanVienTiepNhanCS = tblNhanVienTiepNhan.Where(x => x.PhongBan == "CS" && x.NgayGioTiepNhan >= last2Days).Select(x => x.CM_CommentId).ToList();
                var listCS = lstCommentCanChia.Where(x => !commentIdInNhanVienTiepNhanCS.Contains(x.Id)).ToList();

                NLog.LogManager.GetCurrentClassLogger().Debug("AutotoolProcess():  {0} comment cần chia cho CS", listCS.Count);

                if (listCS.Any())
                {
                    foreach (var row in listCS)
                    {
                        AssignCurrentCommentForAvalableUser("CS", row.Id);
                    }
                }

                var commentIdInNhanVienTiepNhanSO = tblNhanVienTiepNhan.Where(x => x.PhongBan == "SO").Select(x => x.CM_CommentId).ToList();
                var ListSO = tblComment.Where(x => x.CM_TrangThaiId == 3 && x.Status == "A" && !commentIdInNhanVienTiepNhanSO.Contains(x.Id)).ToList();
                NLog.LogManager.GetCurrentClassLogger().Debug("AutotoolProcess():  {0} comment cần chia cho SO", ListSO.Count);

                if (ListSO.Any())
                {
                    foreach (var row in ListSO)
                    {
                        AssignCurrentCommentForAvalableUser("SO", row.Id);
                    }
                }

                return "SUCCESS";
            }
            catch (Exception ex)
            {
                NLog.LogManager.GetCurrentClassLogger().Debug("AutotoolProcess():  Message - {0} StackTrace: {1}", ex.Message, ex.StackTrace);
                return $"ERROR-{ex.Message}-{ex.StackTrace}";
            }

        }

        private bool AssignComment(string PhongBan, long CommentId, string MaNV)
        {
            try
            {
                var _cm_user = _unitOfWork.Repository<User>().Queryable();
                var _user = _cm_user.FirstOrDefault(x => x.EmployeeId == MaNV);
                if (_user == null)
                    return false;

                _user.LastAssign = DateTime.Now;
                NLog.LogManager.GetCurrentClassLogger()
                    .Debug("UpdateAssignUser():  GetDate - {0} EmployeeId: {1}", _user.LastAssign, _user.EmployeeId);

                CM_NhanVienTiepNhanComment nhanVienTiepNhanComment = new CM_NhanVienTiepNhanComment
                {
                    NgayGioTiepNhan = DateTime.Now,
                    CM_CommentId = CommentId,
                    MaNVTiepNhan_VTA = MaNV,
                    PhongBan = PhongBan,
                    Status = "A"
                };
                _unitOfWork.Repository<CM_NhanVienTiepNhanComment>().Insert(nhanVienTiepNhanComment);
                _unitOfWork.Save();

                NLog.LogManager.GetCurrentClassLogger()
                    .Debug("AssignComment():  PhongBan - {0} CommentId: {1}, MaNV: {2}",
                        nhanVienTiepNhanComment.PhongBan, nhanVienTiepNhanComment.CM_CommentId,
                        nhanVienTiepNhanComment.MaNVTiepNhan_VTA);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool SendMailCommentReply()
        {

            var mail = new MailHelper();
            string subject = "Chăm sóc khách hàng";
            string body = "";
            string mailTemp = "/Templete/Mailcomment.htm";
            if (mail.ReadEmailTemplate(mailTemp, ref subject, ref body))
            {

                body = body.Replace("[QuanTriVien]", "IT TEST");


                mail.SendEmail("xuanngocit@gmail.com",
                    "",
                    "",
                    subject, body);
            }
            NLog.LogManager.GetCurrentClassLogger()
                    .Debug("SendEmail():  TEST - {0}", "");

            return true;
        }
    }
}