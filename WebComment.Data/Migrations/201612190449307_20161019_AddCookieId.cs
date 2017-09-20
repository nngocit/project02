namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161019_AddCookieId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CM_ThongTinKhachHang", "CookieId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CM_ThongTinKhachHang", "CookieId");
        }
    }
}
