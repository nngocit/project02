namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161201UpdateTableCM_CommentaddLikesIPAddress2nd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CM_Comment", "Rating", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CM_Comment", "Rating");
        }
    }
}
