namespace Maps.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddIcon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Layer", "Icon", c => c.String(maxLength: 150));
        }

        public override void Down()
        {
            DropColumn("dbo.Layer", "Icon");
        }
    }
}
