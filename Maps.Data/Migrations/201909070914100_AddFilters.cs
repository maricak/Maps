namespace Maps.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddFilters : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Column", "Filter_", c => c.String());
            AddColumn("dbo.Column", "IsFilterVisible", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Column", "IsFilterVisible");
            DropColumn("dbo.Column", "Filter_");
        }
    }
}
