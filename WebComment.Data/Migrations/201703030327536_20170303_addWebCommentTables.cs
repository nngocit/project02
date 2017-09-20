namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170303_addWebCommentTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CM_Comment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NgayGioTao = c.DateTime(nullable: false),
                        NoiDung = c.String(),
                        SP_MaSP = c.String(),
                        SP_TenSP = c.String(),
                        SP_URL = c.String(),
                        LoaiHienThi = c.String(),
                        Status = c.String(),
                        Likes = c.String(),
                        Rating = c.String(),
                        CM_ThongTinKhachHangId = c.Long(),
                        CM_LoaiCommentLevel1Id = c.Int(),
                        CM_LoaiCommentLevel2Id = c.Int(),
                        CM_LoaiCommentLevel3Id = c.Int(),
                        CM_LoaiCommentLevel4Id = c.Int(),
                        CM_TrangThaiId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CM_LoaiComment", t => t.CM_LoaiCommentLevel1Id)
                .ForeignKey("dbo.CM_LoaiComment", t => t.CM_LoaiCommentLevel2Id)
                .ForeignKey("dbo.CM_LoaiComment", t => t.CM_LoaiCommentLevel3Id)
                .ForeignKey("dbo.CM_LoaiComment", t => t.CM_LoaiCommentLevel4Id)
                .ForeignKey("dbo.CM_ThongTinKhachHang", t => t.CM_ThongTinKhachHangId)
                .ForeignKey("dbo.CM_TrangThai", t => t.CM_TrangThaiId)
                .Index(t => t.CM_ThongTinKhachHangId)
                .Index(t => t.CM_LoaiCommentLevel1Id)
                .Index(t => t.CM_LoaiCommentLevel2Id)
                .Index(t => t.CM_LoaiCommentLevel3Id)
                .Index(t => t.CM_LoaiCommentLevel4Id)
                .Index(t => t.CM_TrangThaiId);
            
            CreateTable(
                "dbo.CM_LoaiComment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenLoaiComment = c.String(),
                        Level = c.Int(),
                        HierarchyPath = c.String(),
                        Status = c.String(),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CM_LoaiComment", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.CM_ThongTinKhachHang",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChucDanh = c.String(),
                        HoTen = c.String(),
                        Avatar = c.String(),
                        GioiTinh = c.String(),
                        SoDienThoai = c.String(),
                        Email = c.String(),
                        IPAddress = c.String(),
                        NgayGioTao = c.DateTime(nullable: false),
                        CookieId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CM_TrangThai",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenTrangThai = c.String(),
                        LoaiTrangThai = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CM_LichLamViec",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MaNV = c.String(),
                        Ca = c.Int(),
                        Ngay = c.Int(),
                        NgayLamViec = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CM_NhanVienTiepNhanComment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CM_CommentId = c.Long(),
                        MaNVTiepNhan_VTA = c.String(),
                        PhongBan = c.String(),
                        NgayGioTiepNhan = c.DateTime(),
                        NgayGioTraLoi = c.DateTime(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CM_Comment", t => t.CM_CommentId)
                .Index(t => t.CM_CommentId);
            
            CreateTable(
                "dbo.CM_NVQuanLyDanhGia",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CM_CommentId = c.Long(),
                        MaNVQuanLy_VTA = c.String(),
                        PhongBan = c.String(),
                        NgayGioDanhGia = c.DateTime(),
                        DiemDanhGia = c.Decimal(precision: 18, scale: 2),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CM_Comment", t => t.CM_CommentId)
                .Index(t => t.CM_CommentId);
            
            CreateTable(
                "dbo.CM_ReplyComment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NgayGioTao = c.DateTime(nullable: false),
                        NoiDung = c.String(),
                        Status = c.String(),
                        Likes = c.String(),
                        TypeName = c.String(),
                        CM_CommentId = c.Long(),
                        CM_ThongTinKhachHangId = c.Long(),
                        CM_NhanVienTiepNhanCommentId = c.Long(),
                        MaNV_VTAReply = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CM_Comment", t => t.CM_CommentId)
                .ForeignKey("dbo.CM_NhanVienTiepNhanComment", t => t.CM_NhanVienTiepNhanCommentId)
                .ForeignKey("dbo.CM_ThongTinKhachHang", t => t.CM_ThongTinKhachHangId)
                .Index(t => t.CM_CommentId)
                .Index(t => t.CM_ThongTinKhachHangId)
                .Index(t => t.CM_NhanVienTiepNhanCommentId);
           
            
            CreateTable(
                "dbo.CM_UserLoginHistory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MaNV = c.String(),
                        PhongBan = c.String(),
                        Date = c.DateTime(),
                        LogInTime = c.DateTime(),
                        LogOutTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CM_ReplyComment", "CM_ThongTinKhachHangId", "dbo.CM_ThongTinKhachHang");
            DropForeignKey("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId", "dbo.CM_NhanVienTiepNhanComment");
            DropForeignKey("dbo.CM_ReplyComment", "CM_CommentId", "dbo.CM_Comment");
            DropForeignKey("dbo.CM_NVQuanLyDanhGia", "CM_CommentId", "dbo.CM_Comment");
            DropForeignKey("dbo.CM_NhanVienTiepNhanComment", "CM_CommentId", "dbo.CM_Comment");
            DropForeignKey("dbo.CM_Comment", "CM_TrangThaiId", "dbo.CM_TrangThai");
            DropForeignKey("dbo.CM_Comment", "CM_ThongTinKhachHangId", "dbo.CM_ThongTinKhachHang");
            DropForeignKey("dbo.CM_Comment", "CM_LoaiCommentLevel4Id", "dbo.CM_LoaiComment");
            DropForeignKey("dbo.CM_Comment", "CM_LoaiCommentLevel3Id", "dbo.CM_LoaiComment");
            DropForeignKey("dbo.CM_Comment", "CM_LoaiCommentLevel2Id", "dbo.CM_LoaiComment");
            DropForeignKey("dbo.CM_Comment", "CM_LoaiCommentLevel1Id", "dbo.CM_LoaiComment");
            DropForeignKey("dbo.CM_LoaiComment", "ParentId", "dbo.CM_LoaiComment");

            DropIndex("dbo.CM_ReplyComment", new[] { "CM_NhanVienTiepNhanCommentId" });
            DropIndex("dbo.CM_ReplyComment", new[] { "CM_ThongTinKhachHangId" });
            DropIndex("dbo.CM_ReplyComment", new[] { "CM_CommentId" });
            DropIndex("dbo.CM_NVQuanLyDanhGia", new[] { "CM_CommentId" });
            DropIndex("dbo.CM_NhanVienTiepNhanComment", new[] { "CM_CommentId" });
            DropIndex("dbo.CM_LoaiComment", new[] { "ParentId" });
            DropIndex("dbo.CM_Comment", new[] { "CM_TrangThaiId" });
            DropIndex("dbo.CM_Comment", new[] { "CM_LoaiCommentLevel4Id" });
            DropIndex("dbo.CM_Comment", new[] { "CM_LoaiCommentLevel3Id" });
            DropIndex("dbo.CM_Comment", new[] { "CM_LoaiCommentLevel2Id" });
            DropIndex("dbo.CM_Comment", new[] { "CM_LoaiCommentLevel1Id" });
            DropIndex("dbo.CM_Comment", new[] { "CM_ThongTinKhachHangId" });

            DropTable("dbo.CM_UserLoginHistory");
            DropTable("dbo.CM_Satisfy");
            DropTable("dbo.CM_ReplyComment");
            DropTable("dbo.CM_NVQuanLyDanhGia");
            DropTable("dbo.CM_NhanVienTiepNhanComment");
            DropTable("dbo.CM_LichLamViec");
            DropTable("dbo.CM_TrangThai");
            DropTable("dbo.CM_ThongTinKhachHang");
            DropTable("dbo.CM_LoaiComment");
            DropTable("dbo.CM_Comment");
        }
    }
}
