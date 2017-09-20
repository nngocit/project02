namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161201UpdateAndAddSomeTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CM_Comment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NgayGioTao = c.DateTime(nullable: false),
                        CM_LoaiCommentId = c.Int(),
                        NoiDung = c.String(),
                        KH_Id = c.Long(),
                        SP_MaSP = c.String(),
                        SP_TenSP = c.String(),
                        SP_URL = c.String(),
                        CM_TrangThaiId = c.String(),
                        LoaiHienThi = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CM_NhanVienTiepNhanComment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CM_ReplyCommentId = c.Long(),
                        CM_NhanVienId = c.Long(),
                        NgayGioTiepNhan = c.DateTime(),
                        NgayGioTraLoi = c.DateTime(),
                        DiemDanhGia = c.Int(),
                        CM_NhanVienDanhGiaId = c.Long(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CM_ReplyComment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CM_CommentId = c.Long(),
                        NgayGioTao = c.DateTime(nullable: false),
                        NoiDung = c.String(),
                        CM_ThongTinKhachHangId = c.Long(),
                        CM_NhanVienId = c.Long(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CM_Satisfy",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CM_ReplyCommentId = c.Long(),
                        NgayGioTao = c.DateTime(nullable: false),
                        LoaiSatisfy = c.String(),
                        NoiDung = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CM_ThongTinKhachHang",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HoTen = c.String(),
                        Avatar = c.String(),
                        GioiTinh = c.String(),
                        SoDienThoai = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CM_ThongTinKhachHang");
            DropTable("dbo.CM_Satisfy");
            DropTable("dbo.CM_ReplyComment");
            DropTable("dbo.CM_NhanVienTiepNhanComment");
            DropTable("dbo.CM_Comment");
        }
    }
}
