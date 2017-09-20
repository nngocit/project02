namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170123_UpdateLichLamViecMaNVtostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CM_LichLamViec", "MaNV", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CM_LichLamViec", "MaNV", c => c.Long());
        }
    }
}
