namespace BooksShopCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createDB2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthorDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Year = c.String(),
                        Info = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BooksStorages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StorageDataId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        CountInBlocked = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookDatas", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.StorageDatas", t => t.StorageDataId, cascadeDelete: true)
                .Index(t => t.StorageDataId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.StorageDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameStorage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FormatBookDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormatName = c.String(),
                        BookDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookDatas", t => t.BookDataId, cascadeDelete: true)
                .Index(t => t.BookDataId);
            
            CreateTable(
                "dbo.NameBooksTranslateDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookDataId = c.Int(nullable: false),
                        NameBook = c.String(),
                        LanguageDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookDatas", t => t.BookDataId, cascadeDelete: true)
                .ForeignKey("dbo.LanguageDatas", t => t.LanguageDataId, cascadeDelete: true)
                .Index(t => t.BookDataId)
                .Index(t => t.LanguageDataId);
            
            CreateTable(
                "dbo.LanguageDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PreviewDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Data = c.String(),
                        BookDataId = c.Int(nullable: false),
                        Format_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookDatas", t => t.BookDataId, cascadeDelete: true)
                .ForeignKey("dbo.FormatPreviewDatas", t => t.Format_Id)
                .Index(t => t.BookDataId)
                .Index(t => t.Format_Id);
            
            CreateTable(
                "dbo.FormatPreviewDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormatName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PricePolicyDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookDataId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyDataId = c.Int(nullable: false),
                        CountryDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookDatas", t => t.BookDataId, cascadeDelete: true)
                .ForeignKey("dbo.CountryDatas", t => t.CountryDataId, cascadeDelete: true)
                .ForeignKey("dbo.CurrencyDatas", t => t.CurrencyDataId, cascadeDelete: true)
                .Index(t => t.BookDataId)
                .Index(t => t.CurrencyDataId)
                .Index(t => t.CountryDataId);
            
            CreateTable(
                "dbo.CountryDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CurrencyDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrencyCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BuyerAddressDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BuyerDataId = c.Int(nullable: false),
                        Adress = c.String(),
                        FormatAdressBuyer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BuyerDatas", t => t.BuyerDataId, cascadeDelete: true)
                .ForeignKey("dbo.FormatAdressBuyerDatas", t => t.FormatAdressBuyer_Id)
                .Index(t => t.BuyerDataId)
                .Index(t => t.FormatAdressBuyer_Id);
            
            CreateTable(
                "dbo.BuyerDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        BuyerDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BuyerDatas", t => t.BuyerDataId, cascadeDelete: true)
                .Index(t => t.BuyerDataId);
            
            CreateTable(
                "dbo.PurchaseDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        BuyerId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        OrderDataId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyDataId = c.Int(nullable: false),
                        IsGetMoney = c.Boolean(nullable: false),
                        IsTransferComplete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookDatas", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.BuyerDatas", t => t.BuyerId, cascadeDelete: true)
                .ForeignKey("dbo.CurrencyDatas", t => t.CurrencyDataId, cascadeDelete: true)
                .ForeignKey("dbo.OrderDatas", t => t.OrderDataId, cascadeDelete: true)
                .Index(t => t.BuyerId)
                .Index(t => t.BookId)
                .Index(t => t.OrderDataId)
                .Index(t => t.CurrencyDataId);
            
            CreateTable(
                "dbo.FormatAdressBuyerDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormatAdressName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExchangeRatesDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyDataFromId = c.Int(nullable: false),
                        CurrencyDataToId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrencyDatas", t => t.CurrencyDataFromId, cascadeDelete: true)
                .ForeignKey("dbo.CurrencyDatas", t => t.CurrencyDataToId, cascadeDelete: true)
                .Index(t => t.CurrencyDataFromId)
                .Index(t => t.CurrencyDataToId);
            
            CreateTable(
                "dbo.PromocodeDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookDataAuthorDatas",
                c => new
                    {
                        BookData_Id = c.Int(nullable: false),
                        AuthorData_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BookData_Id, t.AuthorData_Id })
                .ForeignKey("dbo.BookDatas", t => t.BookData_Id, cascadeDelete: true)
                .ForeignKey("dbo.AuthorDatas", t => t.AuthorData_Id, cascadeDelete: true)
                .Index(t => t.BookData_Id)
                .Index(t => t.AuthorData_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExchangeRatesDatas", "CurrencyDataToId", "dbo.CurrencyDatas");
            DropForeignKey("dbo.ExchangeRatesDatas", "CurrencyDataFromId", "dbo.CurrencyDatas");
            DropForeignKey("dbo.BuyerAddressDatas", "FormatAdressBuyer_Id", "dbo.FormatAdressBuyerDatas");
            DropForeignKey("dbo.PurchaseDatas", "OrderDataId", "dbo.OrderDatas");
            DropForeignKey("dbo.PurchaseDatas", "CurrencyDataId", "dbo.CurrencyDatas");
            DropForeignKey("dbo.PurchaseDatas", "BuyerId", "dbo.BuyerDatas");
            DropForeignKey("dbo.PurchaseDatas", "BookId", "dbo.BookDatas");
            DropForeignKey("dbo.OrderDatas", "BuyerDataId", "dbo.BuyerDatas");
            DropForeignKey("dbo.BuyerAddressDatas", "BuyerDataId", "dbo.BuyerDatas");
            DropForeignKey("dbo.PricePolicyDatas", "CurrencyDataId", "dbo.CurrencyDatas");
            DropForeignKey("dbo.PricePolicyDatas", "CountryDataId", "dbo.CountryDatas");
            DropForeignKey("dbo.PricePolicyDatas", "BookDataId", "dbo.BookDatas");
            DropForeignKey("dbo.PreviewDatas", "Format_Id", "dbo.FormatPreviewDatas");
            DropForeignKey("dbo.PreviewDatas", "BookDataId", "dbo.BookDatas");
            DropForeignKey("dbo.NameBooksTranslateDatas", "LanguageDataId", "dbo.LanguageDatas");
            DropForeignKey("dbo.NameBooksTranslateDatas", "BookDataId", "dbo.BookDatas");
            DropForeignKey("dbo.FormatBookDatas", "BookDataId", "dbo.BookDatas");
            DropForeignKey("dbo.BooksStorages", "StorageDataId", "dbo.StorageDatas");
            DropForeignKey("dbo.BooksStorages", "BookId", "dbo.BookDatas");
            DropForeignKey("dbo.BookDataAuthorDatas", "AuthorData_Id", "dbo.AuthorDatas");
            DropForeignKey("dbo.BookDataAuthorDatas", "BookData_Id", "dbo.BookDatas");
            DropIndex("dbo.BookDataAuthorDatas", new[] { "AuthorData_Id" });
            DropIndex("dbo.BookDataAuthorDatas", new[] { "BookData_Id" });
            DropIndex("dbo.ExchangeRatesDatas", new[] { "CurrencyDataToId" });
            DropIndex("dbo.ExchangeRatesDatas", new[] { "CurrencyDataFromId" });
            DropIndex("dbo.PurchaseDatas", new[] { "CurrencyDataId" });
            DropIndex("dbo.PurchaseDatas", new[] { "OrderDataId" });
            DropIndex("dbo.PurchaseDatas", new[] { "BookId" });
            DropIndex("dbo.PurchaseDatas", new[] { "BuyerId" });
            DropIndex("dbo.OrderDatas", new[] { "BuyerDataId" });
            DropIndex("dbo.BuyerAddressDatas", new[] { "FormatAdressBuyer_Id" });
            DropIndex("dbo.BuyerAddressDatas", new[] { "BuyerDataId" });
            DropIndex("dbo.PricePolicyDatas", new[] { "CountryDataId" });
            DropIndex("dbo.PricePolicyDatas", new[] { "CurrencyDataId" });
            DropIndex("dbo.PricePolicyDatas", new[] { "BookDataId" });
            DropIndex("dbo.PreviewDatas", new[] { "Format_Id" });
            DropIndex("dbo.PreviewDatas", new[] { "BookDataId" });
            DropIndex("dbo.NameBooksTranslateDatas", new[] { "LanguageDataId" });
            DropIndex("dbo.NameBooksTranslateDatas", new[] { "BookDataId" });
            DropIndex("dbo.FormatBookDatas", new[] { "BookDataId" });
            DropIndex("dbo.BooksStorages", new[] { "BookId" });
            DropIndex("dbo.BooksStorages", new[] { "StorageDataId" });
            DropTable("dbo.BookDataAuthorDatas");
            DropTable("dbo.PromocodeDatas");
            DropTable("dbo.ExchangeRatesDatas");
            DropTable("dbo.FormatAdressBuyerDatas");
            DropTable("dbo.PurchaseDatas");
            DropTable("dbo.OrderDatas");
            DropTable("dbo.BuyerDatas");
            DropTable("dbo.BuyerAddressDatas");
            DropTable("dbo.CurrencyDatas");
            DropTable("dbo.CountryDatas");
            DropTable("dbo.PricePolicyDatas");
            DropTable("dbo.FormatPreviewDatas");
            DropTable("dbo.PreviewDatas");
            DropTable("dbo.LanguageDatas");
            DropTable("dbo.NameBooksTranslateDatas");
            DropTable("dbo.FormatBookDatas");
            DropTable("dbo.StorageDatas");
            DropTable("dbo.BooksStorages");
            DropTable("dbo.BookDatas");
            DropTable("dbo.AuthorDatas");
        }
    }
}
