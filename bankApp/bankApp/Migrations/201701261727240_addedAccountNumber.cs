namespace bankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedAccountNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "AccountNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "AccountNumber");
        }
    }
}
