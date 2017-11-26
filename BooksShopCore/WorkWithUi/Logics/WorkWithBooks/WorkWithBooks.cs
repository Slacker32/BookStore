using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithUi.Logics.WorkWithDataStorage;

namespace BooksShopCore.WorkWithUi.Logics.WorkWithBooks
{
    public class WorkWithBooks:IWorkWithBooks
    {
        private IDataRepository<BookData> BookRepository { get; set; }
        private IDataRepository<ExchangeRatesData> ExchangeRatesRepository { get; set; }
        private WorkWithCurrencyStorage CurrencyStorage { get; set; }

        public WorkWithBooks()
        {
            BookRepository = new GenericRepository<BookData>(new BookStoreContext());
            ExchangeRatesRepository = new GenericRepository<ExchangeRatesData>(new BookStoreContext());
            CurrencyStorage = new WorkWithCurrencyStorage();
        }

        //public IList<BookUi> ShowAllBooks(string languageCode=null,string currencyCode=null)
        //{
        //    var ret = new List<BookUi>();
        //    try
        //    {
        //        var booksListFromStorage = BookRepository.GetWithInclude(
        //            p => p.Authors,p => p.BooksStorages, p => p.NameBooksTranslates, p => p.PricePolicy, p => p.FormatBook,
        //            p => p.NameBooksTranslates.Select(p1=>p1.Language), p => p.PricePolicy.Select(p1 => p1.Currency));
        //        if (booksListFromStorage!=null)
        //        {
        //            foreach (var item in booksListFromStorage)
        //            {
        //                var book = new BookUi();

        //                book.BookId = item.Id;
        //                //получение списка авторов
        //                //book.Author = item.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
        //                book.Authors = ConvertEntity.ToListAuthorUi(item.Authors);

        //                //получение названия в зависимости от выбранного языка
        //                var tempBookName = item.NameBooksTranslates[0];
        //                if (!string.IsNullOrEmpty(languageCode))
        //                {
        //                    tempBookName = item.NameBooksTranslates.FirstOrDefault((p) => p.Language.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
        //                }
        //                //book.Name = tempBookName != null ? tempBookName.NameBook : string.Empty;
        //                book.ListName = new List<BookNameUi> {ConvertEntity.ToBookNameUi(tempBookName)};

        //                //год издания
        //                book.Year = item.Year;


        //                #region цена и валюта в зависимости от ценовой политики
        //                decimal tempPrice = 0;
        //                string tempCurrency = string.Empty;
        //                var priceData = item.PricePolicy.FirstOrDefault((p) => p.Currency.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
        //                if (priceData != null)
        //                {
        //                    tempPrice = priceData.Price;
        //                    tempCurrency = priceData.Currency.CurrencyCode;
        //                }
        //                else
        //                {
        //                    if (item.PricePolicy?.Count>0)
        //                    {
        //                        var reCalcPrice = item.PricePolicy[0].Price;
        //                        var reCalcCurrencyCodeID = item.PricePolicy[0].CurrencyDataId;
        //                        var rate = ExchangeRatesRepository.ReadAll().FirstOrDefault(p=>p.CurrencyDataFromId.Equals(reCalcCurrencyCodeID)&& p.CurrencyTo.CurrencyCode.Equals(currencyCode,StringComparison.OrdinalIgnoreCase))?.Rate;

        //                        tempPrice = tempPrice * (rate.HasValue ? rate.Value : 0);
        //                        tempCurrency = currencyCode;
        //                    }
        //                }
        //                #endregion

        //                book.ListPrice = new List<PriceUi>();
        //                book.ListPrice.Add(new PriceUi()
        //                        {
        //                            Price = tempPrice,
        //                            Currency = CurrencyStorage.Read(currencyCode)
        //                        }
        //                    );
        //                //book.Price = tempPrice;
        //                //book.Currency = CurrencyStorage.Read(currencyCode);


        //                #region определение количества книги на разных складах
        //                var tempCount = 0;
        //                if (item.BooksStorages != null)
        //                {
        //                    tempCount += item.BooksStorages.Sum(storage => storage.Count - storage.CountInBlocked);
        //                }
        //                #endregion
        //                book.Count = tempCount;

        //                book.Format = new FormatBookUi
        //                {
        //                    FormatName = item.FormatBook.Aggregate(new StringBuilder(), (s, p) => s.Append(p.FormatName).Append(";")).ToString(),
        //                };

        //                ret.Add(book);
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        ex.Data.Add("BooksShopCore", "Ошибка получения книг из хранилища данных");
        //        throw;
        //        //throw new ApplicationException($"Ошибка получения книг из хранилища данных:{ex},{ex.StackTrace}");
        //    }
        //    return ret;
        //}
        public IList<BookUi> ShowAllBooks(string languageCode = null, string currencyCode = null)
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
                        //book.Author = item.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
                        book.Authors = ConvertEntity.ToListAuthorUi(item.Authors);

                        //получение названия в зависимости от выбранного языка
                        var tempBookName = item.NameBooksTranslates[0];
                        if (!string.IsNullOrEmpty(languageCode))
                        {
                            tempBookName = item.NameBooksTranslates.FirstOrDefault((p) => p.Language.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
                        }
                        //book.Name = tempBookName != null ? tempBookName.NameBook : string.Empty;
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

                        book.ListPrice = new List<PriceUi>();
                        book.ListPrice.Add(new PriceUi()
                        {
                            Price = tempPrice,
                            Currency = CurrencyStorage.Read(currencyCode)
                        }
                            );
                        //book.Price = tempPrice;
                        //book.Currency = CurrencyStorage.Read(currencyCode);


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
                ex.Data.Add("BooksShopCore", "Ошибка получения книг из хранилища данных");
                throw;
                //throw new ApplicationException($"Ошибка получения книг из хранилища данных:{ex},{ex.StackTrace}");
            }
            return ret;
        }

        public IList<BookUi> FindBooks(string searchStr, string languageCode, string currency)
        {
            var ret = new List<BookUi>();
            try
            {
                var listBook = ShowAllBooks(languageCode, currency);
                //ret = listBook.Where((p) => p.Authors.Contains(searchStr) || p.ListName.Contains(searchStr)).ToList();
                ret = listBook.Where((p) => p.FindAuthor(searchStr) || p.FindName(searchStr)).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка поиска книги в хранилище данных:{ex}");
            }
            return ret;
        }

        public IList<BookUi> ChangeCurrencyBook(int idCurrency, string languageCode)
        {
            var ret = new List<BookUi>();
            try
            {
                var listBook = ShowAllBooks(languageCode, languageCode);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка при изменении валюты для книг:{ex}");
            }
            return ret;
        }

    }
}
