namespace Maps.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsFilterVisibleToLayer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Layer", "IsFilterVisible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Layer", "IsFilterVisible");
        }
    }
}
