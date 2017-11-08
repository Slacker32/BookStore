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
        }

        public IList<BookUi> ShowAllBooks(string languageCode,string currencyCode)
        {
            var ret = new List<BookUi>();
            try
            {
                var booksListFromStorage = BookRepository.GetWithInclude(p=>p.Authors,p=>p.BooksStorages, p => p.NameBooksTranslates, p => p.PricePolicy);
                if (booksListFromStorage!=null)
                {
                    foreach (var item in booksListFromStorage)
                    {
                        var book = new BookUi();

                        //получение списка авторов
                        book.Author = String.Join(";", item.Authors.ToList());

                        //получение названия в зависимости от выбранного языка
                        var tempBookName = item.NameBooksTranslates.FirstOrDefault((p) => p.Language.LanguageCode.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
                        book.Name = tempBookName!=null ? tempBookName.NameBook: string.Empty;

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
                            if (item.PricePolicy?.Count>0)
                            {
                                var reCalcPrice = item.PricePolicy[0].Price;
                                var reCalcCurrencyCodeID = item.PricePolicy[0].CurrencyDataId;
                                var rate = ExchangeRatesRepository.ReadAll().FirstOrDefault(p=>p.CurrencyDataFromId.Equals(reCalcCurrencyCodeID)&& p.CurrencyTo.CurrencyCode.Equals(currencyCode,StringComparison.OrdinalIgnoreCase))?.Rate;

                                tempPrice = tempPrice * (rate.HasValue ? rate.Value : 0);
                                tempCurrency = currencyCode;
                            }
                        }
                        #endregion

                        book.Price = tempPrice;
                        book.Currency = CurrencyStorage.Read(currencyCode);


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
            catch(Exception ex)
            {
                throw new ApplicationException($"Ошибка получения книг из хранилища данных:{ex}");
            }
            return ret;
        }

        public IList<BookUi> FindBooks(string searchStr, string languageCode, string currency)
        {
            var ret = new List<BookUi>();
            try
            {
                var listBook = ShowAllBooks(languageCode, currency);
                ret = listBook.Where((p) => p.Author.Contains(searchStr) || p.Name.Contains(searchStr)).ToList();
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
