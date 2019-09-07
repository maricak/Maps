namespace Maps.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCenterToLayer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Layer", "Center_", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Layer", "Center_");
        }
    }
}
