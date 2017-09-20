namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161208UpdateTableCM_NhanVienTiepNhan : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId", "dbo.CM_NhanVienTiepNhanComment");
            DropIndex("dbo.CM_ReplyComment", new[] { "CM_NhanVienTiepNhanCommentId" });
            RenameColumn(table: "dbo.CM_NhanVienTiepNhanComment", name: "Comment_Id", newName: "CM_CommentId");
            RenameIndex(table: "dbo.CM_NhanVienTiepNhanComment", name: "IX_Comment_Id", newName: "IX_CM_CommentId");
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "MaNVTiepNhan_VTA", c => c.String());
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "PhongBan", c => c.String());
            CreateIndex("dbo.CM_NhanVienTiepNhanComment", "CM_ReplyCommentId");
            AddForeignKey("dbo.CM_NhanVienTiepNhanComment", "CM_ReplyCommentId", "dbo.CM_ReplyComment", "Id");
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "DiemDanhGia");
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "CM_NhanVienDanhGiaId");
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "CM_NhanVienTiepNhanId");
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "Group");
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "Status");
            DropColumn("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId", c => c.Long());
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "Status", c => c.String());
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "Group", c => c.String());
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "CM_NhanVienTiepNhanId", c => c.Long());
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "CM_NhanVienDanhGiaId", c => c.Long());
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "DiemDanhGia", c => c.Int());
            DropForeignKey("dbo.CM_NhanVienTiepNhanComment", "CM_ReplyCommentId", "dbo.CM_ReplyComment");
            DropIndex("dbo.CM_NhanVienTiepNhanComment", new[] { "CM_ReplyCommentId" });
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "PhongBan");
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "MaNVTiepNhan_VTA");
            RenameIndex(table: "dbo.CM_NhanVienTiepNhanComment", name: "IX_CM_CommentId", newName: "IX_Comment_Id");
            RenameColumn(table: "dbo.CM_NhanVienTiepNhanComment", name: "CM_CommentId", newName: "Comment_Id");
            CreateIndex("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId");
            AddForeignKey("dbo.CM_ReplyComment", "CM_NhanVienTiepNhanCommentId", "dbo.CM_NhanVienTiepNhanComment", "Id");
        }
    }
}
