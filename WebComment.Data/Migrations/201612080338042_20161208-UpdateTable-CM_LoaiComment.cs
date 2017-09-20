namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161208UpdateTableCM_LoaiComment : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.CM_LoaiComment", "ParentId");
            AddForeignKey("dbo.CM_LoaiComment", "ParentId", "dbo.CM_LoaiComment", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CM_LoaiComment", "ParentId", "dbo.CM_LoaiComment");
            DropIndex("dbo.CM_LoaiComment", new[] { "ParentId" });
        }
    }
}
