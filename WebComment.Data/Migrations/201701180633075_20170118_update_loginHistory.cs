namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170118_update_loginHistory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CM_LichLamViec", "MaNV", c => c.Long());
            AlterColumn("dbo.CM_LichLamViec", "Ca", c => c.Int());
            AlterColumn("dbo.CM_LichLamViec", "Ngay", c => c.Int());
            AlterColumn("dbo.CM_LichLamViec", "NgayLamViec", c => c.DateTime());
            AlterColumn("dbo.CM_UserLoginHistory", "MaNV", c => c.Long());
            AlterColumn("dbo.CM_UserLoginHistory", "Date", c => c.DateTime());
            AlterColumn("dbo.CM_UserLoginHistory", "LogInTime", c => c.DateTime());
            AlterColumn("dbo.CM_UserLoginHistory", "LogOutTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CM_UserLoginHistory", "LogOutTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CM_UserLoginHistory", "LogInTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CM_UserLoginHistory", "Date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CM_UserLoginHistory", "MaNV", c => c.Long(nullable: false));
            AlterColumn("dbo.CM_LichLamViec", "NgayLamViec", c => c.DateTime(nullable: false));
            AlterColumn("dbo.CM_LichLamViec", "Ngay", c => c.Int(nullable: false));
            AlterColumn("dbo.CM_LichLamViec", "Ca", c => c.Int(nullable: false));
            AlterColumn("dbo.CM_LichLamViec", "MaNV", c => c.Long(nullable: false));
        }
    }
}
