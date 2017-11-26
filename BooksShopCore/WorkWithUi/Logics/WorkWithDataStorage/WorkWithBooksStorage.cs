using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;

namespace BooksShopCore.WorkWithUi.Logics.WorkWithDataStorage
{
    public class WorkWithBooksStorage : IDisposable, IWorkWithDataStorage<BookUi>
    {
        private readonly BookStoreContext db;
        private IDataRepository<BookData> BookRepository { get; set; }
        private IDataRepository<ExchangeRatesData> ExchangeRatesRepository { get; set; }
        private IDataRepository<NameBooksTranslateData> NameBooksTranslateDataRepository { get; set; }
        private IDataRepository<LanguageData> LanguageDataRepository { get; set; }
        private IDataRepository<AuthorData> AuthorDataRepository { get; set; }
        private IDataRepository<BooksStorage> BooksStorageRepository { get; set; }
        private IDataRepository<StorageData> StorageDataRepository { get; set; }
        private IDataRepository<FormatBookData> FormatBookDataRepository { get; set; }
        private IDataRepository<PreviewData> PreviewDataRepository { get; set; }
        private IDataRepository<PricePolicyData> PricePolicyDataRepository { get; set; }

        internal IList<BookUi> Books { get; set; }

        public WorkWithBooksStorage()
        {
            this.db = new BookStoreContext();
            BookRepository = new GenericRepository<BookData>(db);

        }

        public IList<BookUi> ReadAll(string languageCode, string currencyCode)
        {
            IList<BookUi> ret = null;
            try
            {
                var booksListFromStorage = BookRepository.GetWithInclude(p => p.Authors, p => p.BooksStorages, p => p.NameBooksTranslates, p => p.PricePolicy);
                if (booksListFromStorage != null)
                {
                    foreach (var item in booksListFromStorage)
                    {
                        var book = new BookUi();

                        //получение списка авторов
                        //book.Authors = String.Join(";", item.Authors.ToList());
                        book.Authors = ConvertEntity.ToListAuthorUi(item.Authors);

                        //получение названия в зависимости от выбранного языка
                        var tempBookName = item.NameBooksTranslates.FirstOrDefault((p) => p.Language.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
                        //book.ListName = tempBookName != null ? tempBookName.NameBook : string.Empty;
                        book.ListName = new List<BookNameUi> { ConvertEntity.ToBookNameUi(tempBookName) };

                        //год издания
                        book.Year = item.Year;


                        #region цена и валюта в зависимости от ценовой политики
                        decimal tempPrice = 0;
                        string tempCurrency = string.Empty;
                        var priceData = item.PricePolicy.FirstOrDefault((p) => p.Currency.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
                        if (priceData != null)
                        {
                            tempPrice = priceData.Price;
                            tempCurrency = priceData.Currency.CurrencyCode;
                        }
                        else
                        {
                            if (item.PricePolicy?.Count > 0)
                            {
                                var reCalcPrice = item.PricePolicy[0].Price;
                                var reCalcCurrencyCodeID = item.PricePolicy[0].CurrencyDataId;
                                var rate = ExchangeRatesRepository.ReadAll().FirstOrDefault(p => p.CurrencyDataFromId.Equals(reCalcCurrencyCodeID) && p.CurrencyTo.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase))?.Rate;

                                tempPrice = tempPrice * (rate.HasValue ? rate.Value : 0);
                                tempCurrency = currencyCode;
                            }
                        }
                        #endregion

                        var currency = new WorkWithCurrencyStorage();

                        var price = new PriceUi()
                        {
                            Price = tempPrice,
                            Currency = currency.Read(tempCurrency)
                        };
                        book.ListPrice = book.ListPrice ?? new List<PriceUi>();
                        book.ListPrice.Add(price);


                        #region определение количества книги на разных складах
                        var tempCount = 0;
                        if (item.BooksStorages != null)
                        {
                            tempCount += item.BooksStorages.Sum(storage => storage.Count - storage.CountInBlocked);
                        }
                        #endregion
                        book.Count = tempCount;
                        book.BookId = item.Id;

                        ret.Add(book);
                    }
                }
            }
            catch (Exception)
            {

                throw new ApplicationException($"Ошибка получения данных");
            }

            return ret;
        }
        public void Create(BookUi item,string languageCode)
        {
            try
            {
                if (item != null)
                {
                    //добавление новой записи в книги в хранилище данных
                    var bookData = new BookData();

                    #region добавка названий

                    var listBooksName = new List<NameBooksTranslateData>();
                    foreach (var bookName in item.ListName)
                    {
                        var nameBook = new NameBooksTranslateData();
                        nameBook.Language =
                            LanguageDataRepository.ReadAll()?
                                .FirstOrDefault(
                                    p => p.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
                        nameBook.Book = bookData;
                        nameBook.NameBook = bookName.Name;
                        listBooksName.Add(nameBook);
                    }
                    //добавка названия
                    //bookData.NameBooksTranslates = new List<NameBooksTranslateData> {nameBook};
                    bookData.NameBooksTranslates = listBooksName;

                    #endregion

                    #region добавка авторов
                    var listAuthors = new List<AuthorData>();
                    foreach (var author in item.Authors)
                    {
                        var authorData =
                            AuthorDataRepository.ReadAll()?
                                .FirstOrDefault(p => p.Name.Equals(author.Name, StringComparison.OrdinalIgnoreCase));

                        if (authorData == null)
                        {
                            authorData = new AuthorData
                            {
                                Name = author.Name,
                                Books = new List<BookData> {bookData}
                            };
                        }

                        listAuthors.Add(authorData);
                    }
                    //добавка автора
                    bookData.Authors = listAuthors;

                    #endregion

                    #region добавка хранилища
                    var bookStorage = BooksStorageRepository.ReadAll()?.FirstOrDefault(p => p.BookId.Equals(item.BookId));
                    if (bookStorage == null)
                    {
                        var storage = StorageDataRepository.ReadAll()?.First();
                        if (storage == null)
                        {
                            storage = new StorageData()
                            {
                                NameStorage = "First storage"
                            };
                        }
                        bookStorage = new BooksStorage()
                        {
                            Book = bookData,
                            Count = item.Count,
                            Storage = storage
                        };
                    }
                    else
                    {
                        //книга существуют добавляем ее количество
                        bookStorage.Count += item.Count;
                    }

                    //добавка хранилища
                    bookData.BooksStorages = new List<BooksStorage> { bookStorage };

                    #endregion

                    #region добавка формата
                    var formatBookData = FormatBookDataRepository.ReadAll()?.FirstOrDefault(p => p.FormatName.Equals(item.Format?.FormatName, StringComparison.OrdinalIgnoreCase));
                    //if (formatBookData == null)
                    //{
                    //    var authorData = new AuthorData
                    //    {
                    //        Name = item.Author,
                    //        Books = new List<BookData> { bookData }
                    //    };
                    //}

                    //добавка формата
                    bookData.FormatBook = new List<FormatBookData> { formatBookData };

                    #endregion

                    #region добавка превью
                    var previewData = PreviewDataRepository.ReadAll()?.FirstOrDefault(p => p.Path.Equals(item.Format?.FormatName, StringComparison.OrdinalIgnoreCase));
                    if (previewData == null)
                    {
                        previewData = new PreviewData
                        {
                            Book = bookData,
                            Format = new FormatPreviewData()
                        };
                    }

                    //добавка превью
                    bookData.Preview = new List<PreviewData> { previewData };

                    #endregion

                    #region добавка ценовой политики
                    //var pricePolicyData = PricePolicyDataRepository.ReadAll()?.FirstOrDefault(p => p.Path.Equals(item.Format?.FormatName, StringComparison.OrdinalIgnoreCase));
                    //if (pricePolicyData == null)
                    //{
                    //    pricePolicyData = new PricePolicyData
                    //    {
                    //        Book = bookData,
                    //        Format = new FormatPreviewData()
                    //    };
                    //}

                    ////добавка ценовой политики
                    //bookData.PricePolicy = new List<PricePolicyData> { pricePolicyData };

                    #endregion

                    bookData.Year = item.Year;

                }
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException($"Ошибка добавления книги в хранилище данных: {ex}");
            }
        }

        public IList<BookUi> ReadAll_old()
        {
            IList<BookUi> ret = null;
            try
            {
                var booksListFromStorage = BookRepository.GetWithInclude(p => p.Authors, p => p.BooksStorages, p => p.NameBooksTranslates, p => p.PricePolicy);
                if (booksListFromStorage != null)
                {
                    foreach (var item in booksListFromStorage)
                    {
                        var book = new BookUi();

                        //получение списка авторов
                        book.Authors = ConvertEntity.ToListAuthorUi(item.Authors);
                        book.ListName = ConvertEntity.ToListBookNameUi(item.NameBooksTranslates);

                        //год издания
                        book.Year = item.Year;

                        var currencyStorege = new WorkWithCurrencyStorage();
                        book.ListPrice =
                            item.PricePolicy.Select(
                                tempPrice =>
                                    new PriceUi()
                                    {
                                        Price = tempPrice.Price,
                                        Currency = currencyStorege.Read(tempPrice.Currency.Id)
                                    }).ToList();

                        #region цена и валюта в зависимости от ценовой политики
                        //decimal tempPrice = 0;
                        //string tempCurrency = string.Empty;


                        //var priceData = item.PricePolicy.FirstOrDefault((p) => p.Currency.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
                        //if (priceData != null)
                        //{
                        //    tempPrice = priceData.Price;
                        //    tempCurrency = priceData.Currency.CurrencyCode;
                        //}
                        //else
                        //{
                        //    if (item.PricePolicy?.Count > 0)
                        //    {
                        //        var reCalcPrice = item.PricePolicy[0].Price;
                        //        var reCalcCurrencyCodeID = item.PricePolicy[0].CurrencyDataId;
                        //        var rate = ExchangeRatesRepository.ReadAll().FirstOrDefault(p => p.CurrencyDataFromId.Equals(reCalcCurrencyCodeID) && p.CurrencyTo.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase))?.Rate;

                        //        tempPrice = tempPrice * (rate.HasValue ? rate.Value : 0);
                        //        tempCurrency = currencyCode;
                        //    }
                        //}
                        #endregion

                        //var currency = new WorkWithCurrencyStorage();

                        //var price = new PriceUi()
                        //{
                        //    Price = tempPrice,
                        //    Currency = currency.Read(tempCurrency)
                        //};
                        //book.ListPrice = book.ListPrice ?? new List<PriceUi>();
                        //book.ListPrice.Add(price);


                        #region определение количества книги на разных складах
                        var tempCount = 0;
                        if (item.BooksStorages != null)
                        {
                            tempCount += item.BooksStorages.Sum(storage => storage.Count - storage.CountInBlocked);
                        }
                        #endregion
                        book.Count = tempCount;
                        book.BookId = item.Id;

                        ret.Add(book);
                    }
                }
            }
            catch (Exception)
            {
                throw new ApplicationException($"Ошибка получения данных");
            }

            return ret;
        }
        public IList<BookUi> ReadAll()
        {
            var ret = new List<BookUi>();
            try
            {
                var booksListFromStorage = BookRepository.GetWithInclude(
                    p => p.Authors, p => p.BooksStorages, p => p.NameBooksTranslates, p => p.PricePolicy, p => p.FormatBook,
                    p => p.NameBooksTranslates.Select(p1 => p1.Language), p => p.PricePolicy.Select(p1 => p1.Currency));
                if (booksListFromStorage != null)
                {
                    foreach (var item in booksListFromStorage)
                    {
                        var book = new BookUi();

                        book.BookId = item.Id;
                        //получение списка авторов
                        book.Authors = ConvertEntity.ToListAuthorUi(item.Authors);

                        //получение списка названий
                        book.ListName = ConvertEntity.ToListBookNameUi(item.NameBooksTranslates);

                        //год издания
                        book.Year = item.Year;

                        //получение списка цен
                        book.ListPrice = ConvertEntity.ToListPriceUi(item.PricePolicy);
                        

                        #region определение количества книги на разных складах
                        var tempCount = 0;
                        if (item.BooksStorages != null)
                        {
                            tempCount += item.BooksStorages.Sum(storage => storage.Count - storage.CountInBlocked);
                        }
                        #endregion
                        book.Count = tempCount;

                        book.Format = new FormatBookUi
                        {
                            FormatName = item.FormatBook.Aggregate(new StringBuilder(), (s, p) => s.Append(p.FormatName).Append(";")).ToString(),
                        };

                        ret.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка получения книг из хранилища данных");
                throw;
            }
            return ret;
        }

        public void Create(BookUi item)
        {
            try
            {
                if (item != null)
                {
                    var bookData = new BookData();
                    bookData.Authors = ConvertEntity.ToListAuthorData(item.Authors);
                    bookData.NameBooksTranslates = ConvertEntity.ToListBookNameData(item.ListName);
                    bookData.PricePolicy = ConvertEntity.ToListPricePolicyData(item.ListPrice);
                    bookData.Year = item.Year;
                    //bookData.FormatBook = ConvertEntity.ToListFormatBookData(item.Format);
                    //bookData.BooksStorages = item

                    #region сохранение автора в бд
                    foreach (var author in bookData.Authors)
                    {
                        AuthorDataRepository.AddOrUpdate(author);
                    }
                    #endregion


                    BookRepository.Create(bookData);
                    this.db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления книги в хранилище данных");
                throw;
            }
        }
        public BookUi Read(int index)
        {
            throw new NotImplementedException();
        }
        public BookUi Read(string findStr)
        {
            throw new NotImplementedException();
        }

        public void Update(BookUi elem)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdate(BookUi item)
        {
            throw new NotImplementedException();
        }

        #region частичная реализация паттерна очистки
        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    BookRepository.Dispose();
                }
                this.disposed = true;
            }
        }
        #endregion

    }
}
