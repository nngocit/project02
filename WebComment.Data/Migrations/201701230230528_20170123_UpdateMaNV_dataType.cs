namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170123_UpdateMaNV_dataType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CM_UserLoginHistory", "MaNV", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CM_UserLoginHistory", "MaNV", c => c.Long());
        }
    }
}
