namespace vts.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removedmobilefromidentitymodel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User_Users", "Mobile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User_Users", "Mobile", c => c.String());
        }
    }
}
