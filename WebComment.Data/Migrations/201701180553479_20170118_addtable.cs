namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170118_addtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CM_LichLamViec",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MaNV = c.Long(nullable: false),
                        Ca = c.Int(nullable: false),
                        Ngay = c.Int(nullable: false),
                        NgayLamViec = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CM_UserLoginHistory",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MaNV = c.Long(nullable: false),
                        PhongBan = c.String(),
                        Date = c.DateTime(nullable: false),
                        LogInTime = c.DateTime(nullable: false),
                        LogOutTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CM_UserLoginHistory");
            DropTable("dbo.CM_LichLamViec");
        }
    }
}
