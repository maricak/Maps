namespace Maps.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeLayerNameUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Layer", "Name", c => c.String(maxLength: 50));
            CreateIndex("dbo.Layer", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Layer", new[] { "Name" });
            AlterColumn("dbo.Layer", "Name", c => c.String());
        }
    }
}
