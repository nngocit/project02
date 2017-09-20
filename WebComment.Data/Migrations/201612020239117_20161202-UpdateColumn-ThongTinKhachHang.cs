namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161202UpdateColumnThongTinKhachHang : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CM_Comment");
            AlterColumn("dbo.CM_Comment", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.CM_Comment", "Id");
            CreateIndex("dbo.CM_Comment", "Id");
            AddForeignKey("dbo.CM_Comment", "Id", "dbo.CM_ThongTinKhachHang", "Id");
            DropColumn("dbo.CM_Comment", "KH_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CM_Comment", "KH_Id", c => c.Long());
            DropForeignKey("dbo.CM_Comment", "Id", "dbo.CM_ThongTinKhachHang");
            DropIndex("dbo.CM_Comment", new[] { "Id" });
            DropPrimaryKey("dbo.CM_Comment");
            AlterColumn("dbo.CM_Comment", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.CM_Comment", "Id");
        }
    }
}
