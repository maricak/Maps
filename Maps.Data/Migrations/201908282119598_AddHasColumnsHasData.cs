namespace Maps.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddHasColumnsHasData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Layer", "HasData", c => c.Boolean(nullable: false));
            AddColumn("dbo.Layer", "HasColumns", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Layer", "HasColumns");
            DropColumn("dbo.Layer", "HasData");
        }
    }
}
