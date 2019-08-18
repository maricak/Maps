namespace Maps.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Layer", new[] { "Name" });
            DropIndex("dbo.Map", new[] { "Name" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Map", "Name", unique: true);
            CreateIndex("dbo.Layer", "Name", unique: true);
        }
    }
}
