namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170120_AddColumnChucDanh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ChucDanh", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ChucDanh");
        }
    }
}
