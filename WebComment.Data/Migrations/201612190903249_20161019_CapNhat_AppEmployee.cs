namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161019_CapNhat_AppEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppEmployees", "DoTuoiKH", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppEmployees", "DoTuoiKH");
        }
    }
}
