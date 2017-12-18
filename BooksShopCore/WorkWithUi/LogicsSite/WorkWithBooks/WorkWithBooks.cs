using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithUi.WorkWithDataStorage;

namespace BooksShopCore.WorkWithUi.LogicsSite.WorkWithBooks
{
    public class WorkWithBooks:IWorkWithBooks
    {
        private IDataRepository<BookData> bookRepository;
        private IDataRepository<ExchangeRatesData> exchangeRatesRepository;
        private WorkWithCurrencyStorage currencyStorage;

        public WorkWithBooks()
        {
            bookRepository = new GenericRepository<BookData>(new BookStoreContext());
            exchangeRatesRepository = new GenericRepository<ExchangeRatesData>(new BookStoreContext());
            currencyStorage = new WorkWithCurrencyStorage();
        }

        public IList<BookUi> ShowAllBooks(string languageCode = null, string currencyCode = null)
        {
            var ret = new List<BookUi>();
            try
            {
                var booksListFromStorage = bookRepository.GetWithInclude(
                    p => p.Authors, p => p.BooksStorages, p => p.NameBooksTranslates, p => p.PricePolicy, p => p.FormatBook,
                    p => p.NameBooksTranslates.Select(p1 => p1.Language), p => p.PricePolicy.Select(p1 => p1.Currency));

                ret = MapBookData(booksListFromStorage, languageCode, currencyCode);
            }
            catch (Exception ex)
            {
                ex.Data.Add("ShowAllBooks", "Ошибка получения книг из хранилища данных");
                throw;
            }
            return ret;
        }

        public IList<BookUi> FindBooks(string searchStr, string languageCode, string currencyCode)
        {      
            var ret = new List<BookUi>();
            try
            {

                var booksListFromStorage = bookRepository.GetWithInclude(
                    p => p.Authors, p => p.BooksStorages, p => p.NameBooksTranslates, p => p.PricePolicy, p => p.FormatBook,
                    p => p.NameBooksTranslates.Select(p1 => p1.Language), p => p.PricePolicy.Select(p1 => p1.Currency)
                    ).Where(
                            p => p.Authors.Count(p1 => p1.Name.IndexOf(searchStr, StringComparison.OrdinalIgnoreCase) >= 0) > 0 ||
                                 p.NameBooksTranslates.Count(p1 => p1.NameBook.IndexOf(searchStr, StringComparison.OrdinalIgnoreCase) >= 0) > 0).ToList();

                ret = MapBookData(booksListFromStorage, languageCode, currencyCode);
            }
            catch (Exception ex)
            {
                ex.Data.Add("ShowAllBooks", $"Ошибка поиска книг имя или автор которых содержится в строке \"{searchStr}\" из хранилища данных");
                throw;
            }
            return ret;
        }

        public BookUi GetBook(int index, string languageCode, string currencyCode)
        {
            BookUi ret = null;
            try
            {
                var booksListFromStorage = bookRepository.GetWithInclude(p => p.Id.Equals(index), 
                    p => p.Authors, p => p.BooksStorages, p => p.NameBooksTranslates, p => p.PricePolicy, p => p.FormatBook,
                    p => p.NameBooksTranslates.Select(p1 => p1.Language), p => p.PricePolicy.Select(p1 => p1.Currency));

                ret = MapBookData(booksListFromStorage, languageCode, currencyCode).First();
            }
            catch (Exception ex)
            {
                ex.Data.Add("GetBook", "Ошибка получения книги по ее индексу");
                throw;
            }

            return ret;
        }

        private List<BookUi> MapBookData(IList<BookData> booksListFromStorage, string languageCode = null, string currencyCode = null)
        {
            var ret = new List<BookUi>();
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
                            var rate = exchangeRatesRepository.ReadAll().FirstOrDefault(p => p.CurrencyDataFromId.Equals(reCalcCurrencyCodeID) && p.CurrencyTo.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase))?.Rate;

                            tempPrice = tempPrice * (rate.HasValue ? rate.Value : 0);
                            tempCurrency = currencyCode;
                        }
                    }
                    #endregion

                    book.ListPrice = new List<PriceUi>();
                    book.ListPrice.Add(new PriceUi()
                    {
                        Price = tempPrice,
                        Currency = currencyStorage.Read(currencyCode)
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
            return ret;
        }
    }
}
