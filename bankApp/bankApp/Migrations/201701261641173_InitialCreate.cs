namespace bankApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Solde = c.Double(nullable: false),
                        Owner_ID = c.Int(nullable: false),
                        BIC = c.String(),
                        IBAN = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.Owner_ID, cascadeDelete: true)
                .Index(t => t.Owner_ID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Password = c.String(),
                        Banker_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Bankers", t => t.Banker_ID, cascadeDelete: true)
                .Index(t => t.Banker_ID);
            
            CreateTable(
                "dbo.Bankers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Mail = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        Title = c.String(),
                        Agency = c.String(),
                        CashDispanserName = c.String(),
                        CdType = c.Int(),
                        AgencyName = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Account_ID = c.Int(),
                        Destination_ID = c.Int(),
                        Source_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Accounts", t => t.Account_ID)
                .ForeignKey("dbo.Accounts", t => t.Destination_ID)
                .ForeignKey("dbo.Accounts", t => t.Source_ID)
                .Index(t => t.Account_ID)
                .Index(t => t.Destination_ID)
                .Index(t => t.Source_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "Source_ID", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "Destination_ID", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "Account_ID", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "Owner_ID", "dbo.Customers");
            DropForeignKey("dbo.Customers", "Banker_ID", "dbo.Bankers");
            DropIndex("dbo.Transactions", new[] { "Source_ID" });
            DropIndex("dbo.Transactions", new[] { "Destination_ID" });
            DropIndex("dbo.Transactions", new[] { "Account_ID" });
            DropIndex("dbo.Customers", new[] { "Banker_ID" });
            DropIndex("dbo.Accounts", new[] { "Owner_ID" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Bankers");
            DropTable("dbo.Customers");
            DropTable("dbo.Accounts");
        }
    }
}
