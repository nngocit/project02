namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161201AddTableCM_TrangThai : DbMigration
    {
        public override void Up()
        {
            
            CreateTable(
                "dbo.CM_TrangThai",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenTrangThai = c.String(),
                        LoaiTrangThai = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.CM_TrangThai");
        }
    }
}
