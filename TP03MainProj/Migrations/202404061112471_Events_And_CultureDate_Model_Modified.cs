namespace TP03MainProj.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Events_And_CultureDate_Model_Modified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CalenderDates", "Culture", c => c.String(nullable: false));
            AddColumn("dbo.CalenderDates", "Start_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.CalenderDates", "End_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.CalenderDates", "Title", c => c.String());
            AddColumn("dbo.Events", "Culture", c => c.String(nullable: false));
            AddColumn("dbo.Events", "StartDate", c => c.String(nullable: false));
            AddColumn("dbo.Events", "EndDate", c => c.String(nullable: false));
            AddColumn("dbo.Events", "Url", c => c.String(nullable: false));
            DropColumn("dbo.CalenderDates", "CulturalDate");
            DropColumn("dbo.CalenderDates", "Name");
            DropColumn("dbo.Events", "StartTime");
            DropColumn("dbo.Events", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Events", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.CalenderDates", "Name", c => c.String());
            AddColumn("dbo.CalenderDates", "CulturalDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Events", "Url");
            DropColumn("dbo.Events", "EndDate");
            DropColumn("dbo.Events", "StartDate");
            DropColumn("dbo.Events", "Culture");
            DropColumn("dbo.CalenderDates", "Title");
            DropColumn("dbo.CalenderDates", "End_Date");
            DropColumn("dbo.CalenderDates", "Start_Date");
            DropColumn("dbo.CalenderDates", "Culture");
        }
    }
}
