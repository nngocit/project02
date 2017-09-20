namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170804AddGhiChuField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CM_Comment", "GhiChu", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CM_Comment", "GhiChu");
        }
    }
}
