namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161209UpdateTableCM_ReplyCommentv2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CM_ReplyComment", "MaNV_VTAReply", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CM_ReplyComment", "MaNV_VTAReply");
        }
    }
}
