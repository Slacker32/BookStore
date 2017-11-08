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
    public class WorkWithBooksStorage : IDisposable
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
                        book.Author = String.Join(";", item.Authors.ToList());

                        //получение названия в зависимости от выбранного языка
                        var tempBookName = item.NameBooksTranslates.FirstOrDefault((p) => p.Language.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
                        book.Name = tempBookName != null ? tempBookName.NameBook : string.Empty;

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

                        book.Price = tempPrice;
                        book.Currency = tempCurrency;


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

        public void Create(BookUi item, string languageCode)
        {
            try
            {
                if (item != null)
                {
                    //добавление новой записи в книги в хранилище данных
                    var bookData = new BookData();

                    #region добавка названия
                    var nameBook = new NameBooksTranslateData();
                    nameBook.Language = LanguageDataRepository.ReadAll()?.FirstOrDefault(p => p.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
                    nameBook.Book = bookData;
                    nameBook.NameBook = item.Name;

                    //добавка названия
                    bookData.NameBooksTranslates = new List<NameBooksTranslateData> {nameBook};

                    #endregion

                    #region добавка автора
                    var authorData = AuthorDataRepository.ReadAll()?.FirstOrDefault(p => p.Name.Equals(item.Author, StringComparison.OrdinalIgnoreCase));
                    if (authorData == null)
                    {
                        authorData = new AuthorData
                        {
                            Name = item.Author,
                            Books = new List<BookData> {bookData}
                        };
                    }

                    //добавка автора
                    bookData.Authors = new List<AuthorData> { authorData };

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
                    if (formatBookData == null)
                    {
                        authorData = new AuthorData
                        {
                            Name = item.Author,
                            Books = new List<BookData> { bookData }
                        };
                    }

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

        public BookUi Read(int index)
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
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
        #endregion

    }
}
