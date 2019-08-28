namespace Maps.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddHasChart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Column", "HasChart", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Column", "HasChart");
        }
    }
}
