namespace Maps.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class OnCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Column", "Layer_Id", "dbo.Layer");
            DropForeignKey("dbo.Layer", "Map_Id", "dbo.Map");
            DropForeignKey("dbo.Data", "Layer_Id", "dbo.Layer");
            AddForeignKey("dbo.Column", "Layer_Id", "dbo.Layer", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Layer", "Map_Id", "dbo.Map", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Data", "Layer_Id", "dbo.Layer", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Data", "Layer_Id", "dbo.Layer");
            DropForeignKey("dbo.Layer", "Map_Id", "dbo.Map");
            DropForeignKey("dbo.Column", "Layer_Id", "dbo.Layer");
            AddForeignKey("dbo.Data", "Layer_Id", "dbo.Layer", "Id");
            AddForeignKey("dbo.Layer", "Map_Id", "dbo.Map", "Id");
            AddForeignKey("dbo.Column", "Layer_Id", "dbo.Layer", "Id");
        }
    }
}
