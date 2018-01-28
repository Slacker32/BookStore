using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;

namespace BooksShopCore.WorkWithUi.WorkWithDataStorage
{
    public class WorkWithBooksStorage : IDisposable, IWorkWithDataStorage<BookUi>
    {
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

        private IDataRepositoryAsync<BookData> BookRepositoryAsync { get; set; }
        private IDataRepositoryAsync<ExchangeRatesData> ExchangeRatesRepositoryAsync { get; set; }
        private IDataRepositoryAsync<NameBooksTranslateData> NameBooksTranslateDataRepositoryAsync { get; set; }
        private IDataRepositoryAsync<LanguageData> LanguageDataRepositoryAsync { get; set; }
        private IDataRepositoryAsync<AuthorData> AuthorDataRepositoryAsync { get; set; }
        private IDataRepositoryAsync<BooksStorage> BooksStorageRepositoryAsync { get; set; }
        private IDataRepositoryAsync<StorageData> StorageDataRepositoryAsync { get; set; }
        private IDataRepositoryAsync<FormatBookData> FormatBookDataRepositoryAsync { get; set; }
        private IDataRepositoryAsync<PreviewData> PreviewDataRepositoryAsync { get; set; }
        private IDataRepositoryAsync<PricePolicyData> PricePolicyDataRepositoryAsync { get; set; }

        internal IList<BookUi> Books { get; set; }

        public WorkWithBooksStorage()
        {
            BookRepository = new GenericRepository<BookData>(new BookStoreContext());
            LanguageDataRepository = new GenericRepository<LanguageData>(new BookStoreContext());
            AuthorDataRepository = new GenericRepository<AuthorData>(new BookStoreContext());
            BooksStorageRepository = new GenericRepository<BooksStorage>(new BookStoreContext());
            StorageDataRepository = new GenericRepository<StorageData>(new BookStoreContext());
            FormatBookDataRepository = new GenericRepository<FormatBookData>(new BookStoreContext());
            PreviewDataRepository = new GenericRepository<PreviewData>(new BookStoreContext());
            NameBooksTranslateDataRepository = new GenericRepository<NameBooksTranslateData>(new BookStoreContext());

            BookRepositoryAsync = new GenericRepositoryAsync<BookData, BookStoreContext>();
            LanguageDataRepositoryAsync = new GenericRepositoryAsync<LanguageData, BookStoreContext>();
            AuthorDataRepositoryAsync = new GenericRepositoryAsync<AuthorData, BookStoreContext>();
            BooksStorageRepositoryAsync = new GenericRepositoryAsync<BooksStorage, BookStoreContext>();
            StorageDataRepositoryAsync = new GenericRepositoryAsync<StorageData, BookStoreContext>();
            FormatBookDataRepositoryAsync = new GenericRepositoryAsync<FormatBookData, BookStoreContext>();
            PreviewDataRepositoryAsync = new GenericRepositoryAsync<PreviewData, BookStoreContext>();
            NameBooksTranslateDataRepositoryAsync = new GenericRepositoryAsync<NameBooksTranslateData, BookStoreContext>();

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
        public async Task<IList<BookUi>> ReadAllAsync(string languageCode, string currencyCode)
        {
            IList<BookUi> ret = null;
            try
            {
                var booksListFromStorage =await BookRepositoryAsync.GetWithIncludeAsync(p => p.Authors, p => p.BooksStorages, p => p.NameBooksTranslates, p => p.PricePolicy);
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
                                var dataList = await ExchangeRatesRepositoryAsync.ReadAllAsync();
                                var rate = dataList.FirstOrDefault(p => p.CurrencyDataFromId.Equals(reCalcCurrencyCodeID) && p.CurrencyTo.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase))?.Rate;

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
                        nameBook.Language = LanguageDataRepository.ReadAll()?.FirstOrDefault(p => p.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
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
                        //проверка наличия автора
                        var authorData = AuthorDataRepository.ReadAll()?.FirstOrDefault(p => p.Name.Equals(author.Name, StringComparison.OrdinalIgnoreCase));

                        if (authorData == null)
                        {
                            //если автора нет - он добавляется
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
        public async Task CreateAsync(BookUi item, string languageCode)
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
                        var listData = await LanguageDataRepositoryAsync.ReadAllAsync();
                        nameBook.Language = listData?.FirstOrDefault(p => p.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
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
                        //проверка наличия автора
                        var listData = await AuthorDataRepositoryAsync.ReadAllAsync();
                        var authorData = listData?.FirstOrDefault(p => p.Name.Equals(author.Name, StringComparison.OrdinalIgnoreCase));

                        if (authorData == null)
                        {
                            //если автора нет - он добавляется
                            authorData = new AuthorData
                            {
                                Name = author.Name,
                                Books = new List<BookData> { bookData }
                            };
                        }

                        listAuthors.Add(authorData);
                    }
                    //добавка автора
                    bookData.Authors = listAuthors;

                    #endregion

                    #region добавка хранилища
                    var listBooksStorage = await BooksStorageRepositoryAsync.ReadAllAsync();
                    var bookStorage = listBooksStorage?.FirstOrDefault(p => p.BookId.Equals(item.BookId));
                    if (bookStorage == null)
                    {
                        var listStorage =await StorageDataRepositoryAsync.ReadAllAsync();
                        var storage = listStorage?.First();
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
                    var listFormat = await FormatBookDataRepositoryAsync.ReadAllAsync();
                    var formatBookData = listFormat?.FirstOrDefault(p => p.FormatName.Equals(item.Format?.FormatName, StringComparison.OrdinalIgnoreCase));
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
                    var listPreview = await PreviewDataRepositoryAsync.ReadAllAsync();
                    var previewData = listPreview?.FirstOrDefault(p => p.Path.Equals(item.Format?.FormatName, StringComparison.OrdinalIgnoreCase));
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
                var tt = this.GetType().ToString();
                ex.Data.Add(tt, "Ошибка получения книг из хранилища данных");
                throw;
            }
            return ret;
        }
        public async Task<IList<BookUi>> ReadAllAsync()
        {
            var ret = new List<BookUi>();
            try
            {
                var booksListFromStorage =await BookRepositoryAsync.GetWithIncludeAsync(
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
                var tt = this.GetType().ToString();
                ex.Data.Add(tt, "Ошибка получения книг из хранилища данных");
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
                    bookData.FormatBook = ConvertEntity.ToListFormatBookData(new List<FormatBookUi> {item.Format});
                    //bookData.BooksStorages = new List<BooksStorage>() { new BooksStorage() { Count = item.Count} }

                    #region перенос книги в хранилище
                    var storage = StorageDataRepository.ReadAll()?.First();
                    var bookStorage = new BooksStorage()
                    {
                        Book = bookData,
                        Count = item.Count,
                        Storage = storage
                    };

                    //добавка хранилища
                    bookData.BooksStorages = new List<BooksStorage> { bookStorage };
                    #endregion


                    BookRepository.Create(bookData);
                    BookRepository.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка добавления книги в хранилище данных");
                throw;
            }
        }
        public async Task CreateAsync(BookUi item)
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
                    bookData.FormatBook = ConvertEntity.ToListFormatBookData(new List<FormatBookUi> { item.Format });
                    //bookData.BooksStorages = new List<BooksStorage>() { new BooksStorage() { Count = item.Count} }

                    #region перенос книги в хранилище
                    var listStorage = await StorageDataRepositoryAsync.ReadAllAsync();
                    var storage = listStorage.First();
                    var bookStorage = new BooksStorage()
                    {
                        Book = bookData,
                        Count = item.Count,
                        Storage = storage
                    };

                    //добавка хранилища
                    bookData.BooksStorages = new List<BooksStorage> { bookStorage };
                    #endregion


                    await BookRepositoryAsync.CreateAsync(bookData);
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
            try
            {
                var book=BookRepository.GetWithInclude(p=>p.Id.Equals(id),p=>p.NameBooksTranslates).FirstOrDefault();
                if (book != null)
                {
                    foreach (var item in book.NameBooksTranslates)
                    {
                        NameBooksTranslateDataRepository.Delete(item.Id);
                        NameBooksTranslateDataRepository.SaveChanges();
                    }
                }

                BookRepository.Delete(id);
                BookRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка удаления книги из хранилища по индексу");
                throw;
            }
        }
        public async Task DeleteAsync(int id)
        {
            try
            {
                var bookList = await BookRepositoryAsync.GetWithIncludeAsync(p => p.Id.Equals(id), p => p.NameBooksTranslates);
                    var book = bookList.FirstOrDefault();
                if (book != null)
                {
                    foreach (var item in book.NameBooksTranslates)
                    {
                        NameBooksTranslateDataRepository.Delete(item.Id);
                        NameBooksTranslateDataRepository.SaveChanges();
                    }
                }

                await BookRepositoryAsync.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                ex.Data.Add(this.GetType().ToString(), "Ошибка удаления книги из хранилища по индексу");
                throw;
            }
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
