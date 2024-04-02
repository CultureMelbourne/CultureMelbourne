namespace TP03MainProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCalendarDateAndEventsTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalenderDates",
                c => new
                    {
                        CalenderDateId = c.Int(nullable: false, identity: true),
                        CulturalDate = c.DateTime(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CalenderDateId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        CalenderDateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CalenderDates", t => t.CalenderDateId, cascadeDelete: true)
                .Index(t => t.CalenderDateId);
            
            DropTable("dbo.Products");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Events", "CalenderDateId", "dbo.CalenderDates");
            DropIndex("dbo.Events", new[] { "CalenderDateId" });
            DropTable("dbo.Events");
            DropTable("dbo.CalenderDates");
        }
    }
}
