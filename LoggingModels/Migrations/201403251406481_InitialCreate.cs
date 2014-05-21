namespace LoggingModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddedProductInfoes",
                c => new
                    {
                        ProdUpdateId = c.Int(nullable: false, identity: true),
                        PInfo = c.Int(nullable: false),
                        Lang = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProdUpdateId);
            
            CreateTable(
                "dbo.DeletedProducts",
                c => new
                    {
                        ProdUpdateId = c.Int(nullable: false, identity: true),
                        ExternalId = c.Int(nullable: false),
                        Provider = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProdUpdateId);
            
            CreateTable(
                "dbo.ExecutedUpdates",
                c => new
                    {
                        UpdateId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                        ResponseTime = c.String(),
                    })
                .PrimaryKey(t => t.UpdateId);
            
            CreateTable(
                "dbo.UpdatedProductInfoes",
                c => new
                    {
                        ProdUpdateId = c.Int(nullable: false, identity: true),
                        PInfo = c.Int(nullable: false),
                        Lang = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProdUpdateId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UpdatedProductInfoes");
            DropTable("dbo.ExecutedUpdates");
            DropTable("dbo.DeletedProducts");
            DropTable("dbo.AddedProductInfoes");
        }
    }
}
