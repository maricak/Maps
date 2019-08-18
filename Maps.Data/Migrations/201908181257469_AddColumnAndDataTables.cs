namespace Maps.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddColumnAndDataTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Column",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(maxLength: 50),
                    DataType = c.Int(nullable: false),
                    EnumValues = c.String(),
                    Layer_Id = c.Guid(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Layer", t => t.Layer_Id)
                .Index(t => t.Layer_Id);

            CreateTable(
                "dbo.Data",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Values = c.String(),
                    Layer_Id = c.Guid(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Layer", t => t.Layer_Id)
                .Index(t => t.Layer_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Data", "Layer_Id", "dbo.Layer");
            DropForeignKey("dbo.Column", "Layer_Id", "dbo.Layer");
            DropIndex("dbo.Data", new[] { "Layer_Id" });
            DropIndex("dbo.Column", new[] { "Layer_Id" });
            DropTable("dbo.Data");
            DropTable("dbo.Column");
        }
    }
}
