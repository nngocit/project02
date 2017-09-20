namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161201AddTableCM_TrangThai_changLoaiTrangThaiType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CM_TrangThai", "LoaiTrangThai", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CM_TrangThai", "LoaiTrangThai", c => c.Int(nullable: false));
        }
    }
}
