using BooksShopCore.WorkWithStorage.EntityStorage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksShopCore.WorkWithStorage
{
    class BookStoreInitializer: DropCreateDatabaseIfModelChanges<BookStoreContext>
    {
        private BookStoreContext db = null;
        protected override void Seed(BookStoreContext db)
        {

            //if (!System.Diagnostics.Debugger.IsAttached) 
            //    System.Diagnostics.Debugger.Launch();

            this.db = db;
            AddBooks();

        }

        private void AddLanguageCodes(string nameLanguage,string code)
        {
            var languageData = new LanguageData()
            {
                LanguageName = nameLanguage,
                LanguageCode = code
            };
            db.Language.AddOrUpdate(languageData);
            db.SaveChanges();
        }
        private void AddCountryCodes(string nameCountry, string code)
        {
            var countryData = new CountryData()
            {
                CountryCode=code,
                CountryName= nameCountry
            };
            db.Countries.AddOrUpdate(countryData);
            db.SaveChanges();
        }
        private void AddCurrencyCodes(string nameCurrency, string code)
        {
            var currencyData = new CurrencyData()
            {
                CurrencyCode= code,
                CurrencyName = nameCurrency
            };
            db.Currencies.AddOrUpdate(currencyData);
            db.SaveChanges();
        }
        private void AddAuthor(string AuthorName,string Info,string Year)
        {
            var author = new AuthorData()
            {
                Name = "Джеффри Рихтер",
                Info = "компьютерный специалист, автор наиболее продаваемых книг в области Win32 и .NET. Рихтер — соучредитель компании Wintellect, которая обучает ИТ-специалистов и консультирует фирмы в области создания ПО.",
                Year = "27 июля 1964 г."
            };
            db.Authors.AddOrUpdate(author);
            db.SaveChanges();
        }
        private void AddStorage(string nameStorage)
        {
            var storage = new StorageData()
            {
                NameStorage= nameStorage
            };
            db.Storages.AddOrUpdate(storage);
            db.SaveChanges();
        }
        private void AddFormatPreview(string typePreview)
        {
            var formatPreviewData = new FormatPreviewData()
            {
                FormatName=typePreview
            };
            db.FormatsPreview.AddOrUpdate(formatPreviewData);
            db.SaveChanges();
        }
        private void AddBooks()
        {
            #region Добавка автора
            var nameAuthor = "Джеффри Рихтер";
            var info = "компьютерный специалист, автор наиболее продаваемых книг в области Win32 и .NET. Рихтер — соучредитель компании Wintellect, которая обучает ИТ-специалистов и консультирует фирмы в области создания ПО.";
            var year = "27 июля 1964 г.";
            AddAuthor(nameAuthor, info, year);
            #endregion

            #region Добавка языка
            AddLanguageCodes("Английский", "eng");
            AddLanguageCodes("Русский", "rus");
            #endregion

            #region Добавка cтран
            AddCountryCodes("Великобритания", "GB");
            AddCountryCodes("Россия", "RU");
            AddCountryCodes("США", "US");
            AddCountryCodes("Беларусь", "BY");
            #endregion

            #region Добавка валют
            AddCurrencyCodes("Белорусский рубль", "BYN");
            AddCurrencyCodes("Российский рубль", "RUB");
            AddCurrencyCodes("Доллар США", "USD");
            AddCurrencyCodes("Фунт стерлингов", "GBP");
            #endregion

            #region Добавка форматов превью
            AddFormatPreview("pdf");
            AddFormatPreview("jpg");
            AddFormatPreview("xml");
            AddFormatPreview("txt");
            #endregion

            BookData book = new BookData();
            book.NameBooksTranslates = new List<NameBooksTranslateData>();
            #region Добавка языка названия книги
            var nameBooksTranslateName = new NameBooksTranslateData()
            {
                NameBook = "CLR via C#. Программирование на платформе Microsoft .NET Framework 4.5 на языке C#. — 4-е изд. — СПб.: Питер, 2013. — 896 с. — ISBN 978-5-496-00433-6.",
                Language = db.Language.FirstOrDefault(p => p.LanguageCode == "rus"),
                Book = book
            };
            db.NameBooksTranslate.AddOrUpdate(nameBooksTranslateName);
            book.NameBooksTranslates.Add(nameBooksTranslateName);
            #endregion

            book.Year = new DateTime(2013, 01, 01);
            book.FormatBook = new List<FormatBookData>();
            #region Добавка формата книги
            var formatBook = new FormatBookData()
            {
                FormatName="paper",
                Book=book
            };
            db.FormatsBook.AddOrUpdate(formatBook);
            book.FormatBook.Add(formatBook);
            #endregion

            book.BooksStorages = new List<BooksStorage>();
            #region Добавка хранилища для книги
            var nameStorage = "Хранилище 1";
            AddStorage(nameStorage);
            var storageBook = new BooksStorage()
            {
                Book=book,
                Count=1,
                CountInBlocked=0,
                Storage=db.Storages.FirstOrDefault(p=>p.NameStorage.Equals(nameStorage, StringComparison.OrdinalIgnoreCase))
            };
            var storage=db.Storages.FirstOrDefault(p => p.NameStorage.Equals(nameStorage, StringComparison.OrdinalIgnoreCase));
            storage.BooksStorages = storage.BooksStorages ?? new List<BooksStorage>();
            storage.BooksStorages.Add(storageBook);
            db.BookStorages.AddOrUpdate(storageBook);
            db.Storages.AddOrUpdate(storage);
            book.BooksStorages.Add(storageBook);
            #endregion

            book.Authors = new List<AuthorData>();
            book.Authors.Add(db.Authors.FirstOrDefault(p => p.Name.Equals(nameAuthor)));

            book.PricePolicy = new List<PricePolicyData>();
            #region Добавка ценовой политики книги
            var pricePolicy = new PricePolicyData()
            {
                Book=book,
                Price= 74.61m,
                Country = db.Countries.FirstOrDefault(p => p.CountryCode.Equals("BY")),
                Currency = db.Currencies.FirstOrDefault(p => p.CurrencyCode.Equals("BYN"))
            };
            db.PricePolicies.AddOrUpdate(pricePolicy);
            book.PricePolicy.Add(pricePolicy);
            #endregion

            book.Preview = new List<PreviewData>();
            #region Добавка превью книги
            var previewData = new PreviewData()
            {
                Book=book,
                Path= "//Previews//Richter_CLR_via.htm",
                Data=string.Empty,
                Format= db.FormatsPreview.FirstOrDefault(p => p.FormatName.Equals("txt",StringComparison.OrdinalIgnoreCase))
            };
            db.Previews.AddOrUpdate(previewData);
            book.Preview.Add(previewData);
            #endregion

            db.Books.AddOrUpdate(book);
            db.SaveChanges();
        }
    }
}
