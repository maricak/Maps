namespace Maps.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveHasData : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Layer", "HasData");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Layer", "HasData", c => c.Boolean(nullable: false));
        }
    }
}
