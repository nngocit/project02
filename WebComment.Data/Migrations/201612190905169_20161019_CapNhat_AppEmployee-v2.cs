namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161019_CapNhat_AppEmployeev2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppEmployees", "GioiTinhKH", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppEmployees", "GioiTinhKH");
        }
    }
}
