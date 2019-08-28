namespace Maps.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemoveEnum : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Column", "EnumValues");
        }

        public override void Down()
        {
            AddColumn("dbo.Column", "EnumValues", c => c.String());
        }
    }
}
