namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170103_addUpdateStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CM_NhanVienTiepNhanComment", "Status", c => c.String());
            AddColumn("dbo.CM_NVQuanLyDanhGia", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CM_NVQuanLyDanhGia", "Status");
            DropColumn("dbo.CM_NhanVienTiepNhanComment", "Status");
        }
    }
}
