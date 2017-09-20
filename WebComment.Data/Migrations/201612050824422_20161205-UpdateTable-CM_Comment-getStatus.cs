namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161205UpdateTableCM_CommentgetStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "CM_NhanVienTiepNhanId", c => c.Long());
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "Group", c => c.String());
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "Comment_Id", c => c.Long());
            CreateIndex("dbo.CM_NhanVienTiepNhanComment", "Comment_Id");
            AddForeignKey("dbo.CM_NhanVienTiepNhanComment", "Comment_Id", "dbo.CM_Comment", "Id");
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "CM_NhanVienId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "CM_NhanVienId", c => c.Long());
            DropForeignKey("dbo.CM_NhanVienTiepNhanComment", "Comment_Id", "dbo.CM_Comment");
            DropIndex("dbo.CM_NhanVienTiepNhanComment", new[] { "Comment_Id" });
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "Comment_Id");
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "Group");
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "CM_NhanVienTiepNhanId");
        }
    }
}
