namespace BooksShopCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Year = c.String(),
                        Info = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Books",
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
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Storages", t => t.StorageDataId, cascadeDelete: true)
                .Index(t => t.StorageDataId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Storages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameStorage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FormatsBook",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormatName = c.String(),
                        BookDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookDataId, cascadeDelete: true)
                .Index(t => t.BookDataId);
            
            CreateTable(
                "dbo.NameBooksTranslate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookDataId = c.Int(nullable: false),
                        NameBook = c.String(),
                        LanguageDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookDataId, cascadeDelete: true)
                .ForeignKey("dbo.Languages", t => t.LanguageDataId, cascadeDelete: true)
                .Index(t => t.BookDataId)
                .Index(t => t.LanguageDataId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Previews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Data = c.String(),
                        BookDataId = c.Int(nullable: false),
                        Format_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookDataId, cascadeDelete: true)
                .ForeignKey("dbo.FormatsPreview", t => t.Format_Id)
                .Index(t => t.BookDataId)
                .Index(t => t.Format_Id);
            
            CreateTable(
                "dbo.FormatsPreview",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormatName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PricePolicies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookDataId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyDataId = c.Int(nullable: false),
                        CountryDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookDataId, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.CountryDataId, cascadeDelete: true)
                .ForeignKey("dbo.Currencies", t => t.CurrencyDataId, cascadeDelete: true)
                .Index(t => t.BookDataId)
                .Index(t => t.CurrencyDataId)
                .Index(t => t.CountryDataId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrencyCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BuyersAddress",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BuyerDataId = c.Int(nullable: false),
                        Adress = c.String(),
                        FormatAdressBuyer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Buyers", t => t.BuyerDataId, cascadeDelete: true)
                .ForeignKey("dbo.FormatsAdressBuyer", t => t.FormatAdressBuyer_Id)
                .Index(t => t.BuyerDataId)
                .Index(t => t.FormatAdressBuyer_Id);
            
            CreateTable(
                "dbo.Buyers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        BuyerDataId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Buyers", t => t.BuyerDataId, cascadeDelete: true)
                .Index(t => t.BuyerDataId);
            
            CreateTable(
                "dbo.Purchases",
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
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Buyers", t => t.BuyerId, cascadeDelete: true)
                .ForeignKey("dbo.Currencies", t => t.CurrencyDataId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderDataId, cascadeDelete: true)
                .Index(t => t.BuyerId)
                .Index(t => t.BookId)
                .Index(t => t.OrderDataId)
                .Index(t => t.CurrencyDataId);
            
            CreateTable(
                "dbo.FormatsAdressBuyer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormatAdressName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExchangeRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyDataFromId = c.Int(nullable: false),
                        CurrencyDataToId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.CurrencyDataFromId, cascadeDelete: true)
                .ForeignKey("dbo.Currencies", t => t.CurrencyDataToId, cascadeDelete: true)
                .Index(t => t.CurrencyDataFromId)
                .Index(t => t.CurrencyDataToId);
            
            CreateTable(
                "dbo.Promocodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Percent = c.Int(nullable: false),
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
                .ForeignKey("dbo.Books", t => t.BookData_Id, cascadeDelete: true)
                .ForeignKey("dbo.Authors", t => t.AuthorData_Id, cascadeDelete: true)
                .Index(t => t.BookData_Id)
                .Index(t => t.AuthorData_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExchangeRates", "CurrencyDataToId", "dbo.Currencies");
            DropForeignKey("dbo.ExchangeRates", "CurrencyDataFromId", "dbo.Currencies");
            DropForeignKey("dbo.BuyersAddress", "FormatAdressBuyer_Id", "dbo.FormatsAdressBuyer");
            DropForeignKey("dbo.Purchases", "OrderDataId", "dbo.Orders");
            DropForeignKey("dbo.Purchases", "CurrencyDataId", "dbo.Currencies");
            DropForeignKey("dbo.Purchases", "BuyerId", "dbo.Buyers");
            DropForeignKey("dbo.Purchases", "BookId", "dbo.Books");
            DropForeignKey("dbo.Orders", "BuyerDataId", "dbo.Buyers");
            DropForeignKey("dbo.BuyersAddress", "BuyerDataId", "dbo.Buyers");
            DropForeignKey("dbo.PricePolicies", "CurrencyDataId", "dbo.Currencies");
            DropForeignKey("dbo.PricePolicies", "CountryDataId", "dbo.Countries");
            DropForeignKey("dbo.PricePolicies", "BookDataId", "dbo.Books");
            DropForeignKey("dbo.Previews", "Format_Id", "dbo.FormatsPreview");
            DropForeignKey("dbo.Previews", "BookDataId", "dbo.Books");
            DropForeignKey("dbo.NameBooksTranslate", "LanguageDataId", "dbo.Languages");
            DropForeignKey("dbo.NameBooksTranslate", "BookDataId", "dbo.Books");
            DropForeignKey("dbo.FormatsBook", "BookDataId", "dbo.Books");
            DropForeignKey("dbo.BooksStorages", "StorageDataId", "dbo.Storages");
            DropForeignKey("dbo.BooksStorages", "BookId", "dbo.Books");
            DropForeignKey("dbo.BookDataAuthorDatas", "AuthorData_Id", "dbo.Authors");
            DropForeignKey("dbo.BookDataAuthorDatas", "BookData_Id", "dbo.Books");
            DropIndex("dbo.BookDataAuthorDatas", new[] { "AuthorData_Id" });
            DropIndex("dbo.BookDataAuthorDatas", new[] { "BookData_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "CurrencyDataToId" });
            DropIndex("dbo.ExchangeRates", new[] { "CurrencyDataFromId" });
            DropIndex("dbo.Purchases", new[] { "CurrencyDataId" });
            DropIndex("dbo.Purchases", new[] { "OrderDataId" });
            DropIndex("dbo.Purchases", new[] { "BookId" });
            DropIndex("dbo.Purchases", new[] { "BuyerId" });
            DropIndex("dbo.Orders", new[] { "BuyerDataId" });
            DropIndex("dbo.BuyersAddress", new[] { "FormatAdressBuyer_Id" });
            DropIndex("dbo.BuyersAddress", new[] { "BuyerDataId" });
            DropIndex("dbo.PricePolicies", new[] { "CountryDataId" });
            DropIndex("dbo.PricePolicies", new[] { "CurrencyDataId" });
            DropIndex("dbo.PricePolicies", new[] { "BookDataId" });
            DropIndex("dbo.Previews", new[] { "Format_Id" });
            DropIndex("dbo.Previews", new[] { "BookDataId" });
            DropIndex("dbo.NameBooksTranslate", new[] { "LanguageDataId" });
            DropIndex("dbo.NameBooksTranslate", new[] { "BookDataId" });
            DropIndex("dbo.FormatsBook", new[] { "BookDataId" });
            DropIndex("dbo.BooksStorages", new[] { "BookId" });
            DropIndex("dbo.BooksStorages", new[] { "StorageDataId" });
            DropTable("dbo.BookDataAuthorDatas");
            DropTable("dbo.Promocodes");
            DropTable("dbo.ExchangeRates");
            DropTable("dbo.FormatsAdressBuyer");
            DropTable("dbo.Purchases");
            DropTable("dbo.Orders");
            DropTable("dbo.Buyers");
            DropTable("dbo.BuyersAddress");
            DropTable("dbo.Currencies");
            DropTable("dbo.Countries");
            DropTable("dbo.PricePolicies");
            DropTable("dbo.FormatsPreview");
            DropTable("dbo.Previews");
            DropTable("dbo.Languages");
            DropTable("dbo.NameBooksTranslate");
            DropTable("dbo.FormatsBook");
            DropTable("dbo.Storages");
            DropTable("dbo.BooksStorages");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
