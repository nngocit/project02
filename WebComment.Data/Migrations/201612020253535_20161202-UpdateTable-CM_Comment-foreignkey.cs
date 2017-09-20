namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161202UpdateTableCM_Commentforeignkey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CM_Comment", "Id", "dbo.CM_ThongTinKhachHang");
            DropIndex("dbo.CM_Comment", new[] { "Id" });
            DropPrimaryKey("dbo.CM_Comment");
            AddColumn("dbo.CM_Comment", "CM_ThongTinKhachHangId", c => c.Long());
            AlterColumn("dbo.CM_Comment", "Id", c => c.Long(nullable: false, identity: true));
            AlterColumn("dbo.CM_Comment", "CM_TrangThaiId", c => c.Int());
            AddPrimaryKey("dbo.CM_Comment", "Id");
            CreateIndex("dbo.CM_Comment", "CM_ThongTinKhachHangId");
            CreateIndex("dbo.CM_Comment", "CM_LoaiCommentId");
            CreateIndex("dbo.CM_Comment", "CM_TrangThaiId");
            AddForeignKey("dbo.CM_Comment", "CM_LoaiCommentId", "dbo.CM_LoaiComment", "Id");
            AddForeignKey("dbo.CM_Comment", "CM_TrangThaiId", "dbo.CM_TrangThai", "Id");
            AddForeignKey("dbo.CM_Comment", "CM_ThongTinKhachHangId", "dbo.CM_ThongTinKhachHang", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CM_Comment", "CM_ThongTinKhachHangId", "dbo.CM_ThongTinKhachHang");
            DropForeignKey("dbo.CM_Comment", "CM_TrangThaiId", "dbo.CM_TrangThai");
            DropForeignKey("dbo.CM_Comment", "CM_LoaiCommentId", "dbo.CM_LoaiComment");
            DropIndex("dbo.CM_Comment", new[] { "CM_TrangThaiId" });
            DropIndex("dbo.CM_Comment", new[] { "CM_LoaiCommentId" });
            DropIndex("dbo.CM_Comment", new[] { "CM_ThongTinKhachHangId" });
            DropPrimaryKey("dbo.CM_Comment");
            AlterColumn("dbo.CM_Comment", "CM_TrangThaiId", c => c.String());
            AlterColumn("dbo.CM_Comment", "Id", c => c.Long(nullable: false));
            DropColumn("dbo.CM_Comment", "CM_ThongTinKhachHangId");
            AddPrimaryKey("dbo.CM_Comment", "Id");
            CreateIndex("dbo.CM_Comment", "Id");
            AddForeignKey("dbo.CM_Comment", "Id", "dbo.CM_ThongTinKhachHang", "Id");
        }
    }
}
