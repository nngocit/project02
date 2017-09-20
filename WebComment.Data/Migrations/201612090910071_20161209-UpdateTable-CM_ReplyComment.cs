namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161209UpdateTableCM_ReplyComment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CM_NhanVienTiepNhanComment", "CM_ReplyCommentId", "dbo.CM_ReplyComment");
            DropIndex("dbo.CM_NhanVienTiepNhanComment", new[] { "CM_ReplyCommentId" });
            AddColumn("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId", c => c.Long());
            CreateIndex("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId");
            AddForeignKey("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId", "dbo.CM_NhanVienTiepNhanComment", "Id");
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "CM_ReplyCommentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "CM_ReplyCommentId", c => c.Long());
            DropForeignKey("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId", "dbo.CM_NhanVienTiepNhanComment");
            DropIndex("dbo.CM_ReplyComment", new[] { "CM_NhanVienTiepNhanCommentId" });
            DropColumn("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId");
            CreateIndex("dbo.CM_NhanVienTiepNhanComment", "CM_ReplyCommentId");
            AddForeignKey("dbo.CM_NhanVienTiepNhanComment", "CM_ReplyCommentId", "dbo.CM_ReplyComment", "Id");
        }
    }
}
