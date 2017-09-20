namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161206UpdateTableCM_ReplyCommentAddForeignKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId", c => c.Long());
            CreateIndex("dbo.CM_ReplyComment", "CM_CommentId");
            CreateIndex("dbo.CM_ReplyComment", "CM_ThongTinKhachHangId");
            CreateIndex("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId");
            AddForeignKey("dbo.CM_ReplyComment", "CM_CommentId", "dbo.CM_Comment", "Id");
            AddForeignKey("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId", "dbo.CM_NhanVienTiepNhanComment", "Id");
            AddForeignKey("dbo.CM_ReplyComment", "CM_ThongTinKhachHangId", "dbo.CM_ThongTinKhachHang", "Id");
            DropColumn("dbo.CM_ReplyComment", "CM_NhanVienId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CM_ReplyComment", "CM_NhanVienId", c => c.Long());
            DropForeignKey("dbo.CM_ReplyComment", "CM_ThongTinKhachHangId", "dbo.CM_ThongTinKhachHang");
            DropForeignKey("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId", "dbo.CM_NhanVienTiepNhanComment");
            DropForeignKey("dbo.CM_ReplyComment", "CM_CommentId", "dbo.CM_Comment");
            DropIndex("dbo.CM_ReplyComment", new[] { "CM_NhanVienTiepNhanCommentId" });
            DropIndex("dbo.CM_ReplyComment", new[] { "CM_ThongTinKhachHangId" });
            DropIndex("dbo.CM_ReplyComment", new[] { "CM_CommentId" });
            DropColumn("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId");
        }
    }
}
