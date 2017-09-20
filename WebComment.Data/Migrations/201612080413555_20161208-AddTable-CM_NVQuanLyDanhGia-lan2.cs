namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161208AddTableCM_NVQuanLyDanhGialan2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CM_NVQuanLyDanhGia",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CM_CommentId = c.Long(),
                        MaNVQuanLy_VTA = c.String(),
                        PhongBan = c.String(),
                        NgayGioDanhGia = c.DateTime(),
                        DiemDanhGia = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CM_Comment", t => t.CM_CommentId)
                .Index(t => t.CM_CommentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CM_NVQuanLyDanhGia", "CM_CommentId", "dbo.CM_Comment");
            DropIndex("dbo.CM_NVQuanLyDanhGia", new[] { "CM_CommentId" });
            DropTable("dbo.CM_NVQuanLyDanhGia");
        }
    }
}
