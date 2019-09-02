namespace Maps.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMapIsPublic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Map", "IsPublic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Map", "IsPublic");
        }
    }
}
