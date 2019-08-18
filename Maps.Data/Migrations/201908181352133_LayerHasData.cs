namespace Maps.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class LayerHasData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Layer", "HasData", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Layer", "HasData");
        }
    }
}
