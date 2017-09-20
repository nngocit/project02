namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161202AddColumnChucDanh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CM_ThongTinKhachHang", "ChucDanh", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CM_ThongTinKhachHang", "ChucDanh");
        }
    }
}
