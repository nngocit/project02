namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161215_UpdateThongTinKhachHangTableNgayGioTao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CM_ThongTinKhachHang", "NgayGioTao", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CM_ThongTinKhachHang", "NgayGioTao");
        }
    }
}
