namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161215_UpdateCommentTableLoaiComment : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CM_Comment", name: "CM_LoaiCommentId", newName: "CM_LoaiCommentLevel1Id");
            RenameIndex(table: "dbo.CM_Comment", name: "IX_CM_LoaiCommentId", newName: "IX_CM_LoaiCommentLevel1Id");
            AddColumn("dbo.CM_Comment", "CM_LoaiCommentLevel2Id", c => c.Int());
            AddColumn("dbo.CM_Comment", "CM_LoaiCommentLevel3Id", c => c.Int());
            AddColumn("dbo.CM_Comment", "CM_LoaiCommentLevel4Id", c => c.Int());
            AddColumn("dbo.CM_ReplyComment", "TypeName", c => c.String());
            CreateIndex("dbo.CM_Comment", "CM_LoaiCommentLevel2Id");
            CreateIndex("dbo.CM_Comment", "CM_LoaiCommentLevel3Id");
            CreateIndex("dbo.CM_Comment", "CM_LoaiCommentLevel4Id");
            AddForeignKey("dbo.CM_Comment", "CM_LoaiCommentLevel2Id", "dbo.CM_LoaiComment", "Id");
            AddForeignKey("dbo.CM_Comment", "CM_LoaiCommentLevel3Id", "dbo.CM_LoaiComment", "Id");
            AddForeignKey("dbo.CM_Comment", "CM_LoaiCommentLevel4Id", "dbo.CM_LoaiComment", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CM_Comment", "CM_LoaiCommentLevel4Id", "dbo.CM_LoaiComment");
            DropForeignKey("dbo.CM_Comment", "CM_LoaiCommentLevel3Id", "dbo.CM_LoaiComment");
            DropForeignKey("dbo.CM_Comment", "CM_LoaiCommentLevel2Id", "dbo.CM_LoaiComment");
            DropIndex("dbo.CM_Comment", new[] { "CM_LoaiCommentLevel4Id" });
            DropIndex("dbo.CM_Comment", new[] { "CM_LoaiCommentLevel3Id" });
            DropIndex("dbo.CM_Comment", new[] { "CM_LoaiCommentLevel2Id" });
            DropColumn("dbo.CM_ReplyComment", "TypeName");
            DropColumn("dbo.CM_Comment", "CM_LoaiCommentLevel4Id");
            DropColumn("dbo.CM_Comment", "CM_LoaiCommentLevel3Id");
            DropColumn("dbo.CM_Comment", "CM_LoaiCommentLevel2Id");
            RenameIndex(table: "dbo.CM_Comment", name: "IX_CM_LoaiCommentLevel1Id", newName: "IX_CM_LoaiCommentId");
            RenameColumn(table: "dbo.CM_Comment", name: "CM_LoaiCommentLevel1Id", newName: "CM_LoaiCommentId");
        }
    }
}
