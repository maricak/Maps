namespace Maps.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLayerIsVisible : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Layer", "IsVisible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Layer", "IsVisible");
        }
    }
}
