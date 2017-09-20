namespace WebComment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20161201UpdateTableCM_CommentaddLikesIPAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CM_Comment", "Likes", c => c.String());
            AddColumn("dbo.CM_ThongTinKhachHang", "IPAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CM_ThongTinKhachHang", "IPAddress");
            DropColumn("dbo.CM_Comment", "Likes");
        }
    }
}
