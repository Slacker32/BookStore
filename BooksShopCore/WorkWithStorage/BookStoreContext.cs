using BooksShopCore.WorkWithStorage.EntityStorage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage
{
    internal class BookStoreContext : DbContext
    {
        public BookStoreContext() : base("BookStoreDbConnection")
        { }

        public DbSet<AuthorData> Authors { get; set; }
        public DbSet<BookData> Books { get; set; }
        public DbSet<BooksStorage> BookStorages { get; set; }
        public DbSet<BuyerAddressData> BuyerAddresses { get; set; }
        public DbSet<BuyerData> Buyers { get; set; }
        public DbSet<CountryData> Countries { get; set; }
        public DbSet<CurrencyData> Currencies { get; set; }
        public DbSet<ExchangeRatesData> ExchangeRates { get; set; }
        public DbSet<FormatAdressBuyerData> FormatsAdressBuyer { get; set; }
        public DbSet<FormatBookData> FormatsBook { get; set; }
        public DbSet<FormatPreviewData> FormatsPreview { get; set; }
        public DbSet<LanguageData> Language { get; set; }
        public DbSet<NameBooksTranslateData> NameBooksTranslate { get; set; }
        public DbSet<OrderData> Orders { get; set; }
        public DbSet<PreviewData> Previews { get; set; }
        public DbSet<PricePolicyData> PricePolicies { get; set; }
        public DbSet<PromocodeData> Promocodes { get; set; }
        public DbSet<PurchaseData> Purchases { get; set; }
        public DbSet<StorageData> Storages { get; set; }
    }
}
