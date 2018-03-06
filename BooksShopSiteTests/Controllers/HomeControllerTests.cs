using Microsoft.VisualStudio.TestTools.UnitTesting;
using BooksShopSite.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using BooksShopCore.WorkWithUi.EntityUi;
using BooksShopSite.Models;

namespace BooksShopSite.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        private List<CurrencyUi> SetCurrencyUi()
        {
            return new List<CurrencyUi>()
            {
                new CurrencyUi()
                {
                    CurrencyId = 1,
                    CurrencyCode = "BYN",
                    CurrencyName = "Белорусский рубль"
                },
                new CurrencyUi()
                {
                    CurrencyId = 2,
                    CurrencyCode = "RUB",
                    CurrencyName = "Российский рубль"
                },
                new CurrencyUi()
                {
                    CurrencyId = 3,
                    CurrencyCode = "USD",
                    CurrencyName = "Доллар США"
                },
                new CurrencyUi()
                {
                    CurrencyId = 4,
                    CurrencyCode = "GBP",
                    CurrencyName = "Фунт стерлингов"
                }
            };
        }

        private BookUi AddBook()
        {
           return new BookUi()
            {
                BookId = 1,
                Authors = new List<AuthorUi>()
                {
                    new AuthorUi()
                    {
                        AuthorId = 1,
                        Name = "Автор1",
                        Info = "Информация по автору1",
                        Year = "2014"
                    }
                },
                Count = 10,
                Format = new FormatBookUi()
                {
                    FormatBookId = 0,
                    FormatName = "paper"
                },
                ListName = new List<BookNameUi>()
                {
                    new BookNameUi()
                    {
                        BookNameId = 1,
                        LanguageBookCode = new LanguageUi()
                        {
                            LanguageId = 1,
                            LanguageCode = "rus",
                            LanguageName = "Русский"
                        },
                        Name = "Название книги1"
                    },
                    new BookNameUi()
                    {
                        BookNameId = 1,
                        LanguageBookCode = new LanguageUi()
                        {
                            LanguageId = 1,
                            LanguageCode = "eng",
                            LanguageName = "Английский"
                        },
                        Name = "Name book1"
                    }
                },
                ListPrice = new List<PriceUi>()
                {
                    new PriceUi()
                    {
                        PriceId = 1,
                        Country = new CountryUi()
                        {
                            CountryId = 1,
                            CountryCode = "BY",
                            CountryName = "Беларусь"
                        },
                        Currency = new CurrencyUi()
                        {
                            CurrencyId = 1,
                            CurrencyCode = "BYN",
                            CurrencyName = "Белорусский рубль"
                        },
                        Price = 123
                    }
                },
                Year = new DateTime(2016, 01, 01)
            };
        }
        private List<BookUi> SetListBookUi()
        {
            return new List<BookUi>()
            {
                AddBook()
            };
        }
        private List<BookSite> GetBooksList(IList<BookUi> bookList, string language, string currency)
        {
            var bookListForViews = new List<BookSite>();
            foreach (var book in bookList)
            {
                var bookSite = new BookSite();
                bookSite.BookId = book.BookId;
                bookSite.Authors = book.Authors.Aggregate(new StringBuilder(), (s, p) => s.Append(p.Name).Append(";")).ToString();
                bookSite.Name = book.ListName.FirstOrDefault(p => p.LanguageBookCode.LanguageCode.Equals(language, StringComparison.OrdinalIgnoreCase))?.Name;
                bookSite.Year = book.Year;
                bookSite.Price = book.ListPrice.FirstOrDefault(p => p.Currency.CurrencyCode.Equals(currency, StringComparison.OrdinalIgnoreCase))?.Price ?? 0;
                bookSite.Format = book.Format.FormatName;
                if (bookSite.Price > 0)
                {
                    bookListForViews.Add(bookSite);
                }
            }
            return bookListForViews;
        }
        private CurrencySite GetCurrencyList(IList<CurrencyUi> listCurrency, string selectedCurrency)
        {
            var currencyForView = new CurrencySite();
            if (listCurrency != null)
            {
                var listCurrencyCode = listCurrency.Select(p => p.CurrencyCode);
                currencyForView.SelectedCurrency = selectedCurrency;
                foreach (var item in listCurrencyCode)
                {
                    if (currencyForView.Currencies == null)
                    {
                        currencyForView.Currencies = new List<SelectListItem>();
                    }
                    currencyForView.Currencies.Add(new SelectListItem { Value = item, Text = item });

                }
            }
            return currencyForView;
        }

        [TestMethod()]
        public void IndexAsyncTest()
        {
            #region инициализация
            var mock = new Mock<BooksShopCore.IBooksShop>();
            #region получение списка валют

            var listCurrency = SetCurrencyUi();
            mock.Setup(p => p.Currency.GetAllCurrencyAsync()).ReturnsAsync(listCurrency);
            #endregion
            #region получение всех книг

            var listBook = SetListBookUi();
            mock.Setup(p => p.Books.ShowAllBooksAsync("rus", "byn")).ReturnsAsync(listBook);
            #endregion
            HomeController controller = new HomeController(mock.Object);
            #endregion
            
            //ожидаемое значение
            var countCurrensy = listCurrency.Count;
            var bookName = listBook[0].ListName[0].Name;

            //результат выполнения
            var actual = controller.IndexAsync().Result as ViewResult;
            var znach = actual.Model as BookCurrencyViewModel;
            //сверка значений
            Assert.AreEqual(countCurrensy, znach?.CurrencyView.Currencies.Count);
            Assert.AreEqual(bookName, (znach?.BooksView as List<BookSite>)?[0].Name);


        }

        [TestMethod()]
        public void SetCurrencyTest()
        {
            #region инициализация
            var mock = new Mock<BooksShopCore.IBooksShop>();
            var language = "rus";
            var currency = "byn";
            #region получение списка валют

            var listCurrency = SetCurrencyUi();
            mock.Setup(p => p.Currency.GetAllCurrencyAsync()).ReturnsAsync(listCurrency);
            #endregion
            #region получение всех книг
            var listBook = SetListBookUi();
            mock.Setup(p => p.Books.ShowAllBooksAsync(language, currency)).ReturnsAsync(listBook);
            #endregion
            HomeController controller = new HomeController(mock.Object);
            #endregion

            //формирование запроса
            //currency = "usd";
            var request = new BookCurrencyViewModel() { BooksView = GetBooksList(listBook, language, currency), CurrencyView = GetCurrencyList(listCurrency, currency) };
            //ожидаемое значение



            //результат выполнения
            var actual = controller.SetCurrency(request).Result as ViewResult;
            var znach = actual.Model as BookCurrencyViewModel;
            //сверка значений
            Assert.AreEqual(request?.CurrencyView.SelectedCurrency, znach?.CurrencyView.SelectedCurrency);
            Assert.AreEqual((request?.BooksView as List<BookSite>)?.Count, (znach?.BooksView as List<BookSite>)?.Count);

        }

        [TestMethod()]
        public void SearchBookAsyncTest()
        {
            #region инициализация 
            var mock = new Mock<BooksShopCore.IBooksShop>();
            var language = "rus";
            var currency = "byn";
            var seachString = "book1";
            #region получение списка валют

            var listCurrency = SetCurrencyUi();
            mock.Setup(p => p.Currency.GetAllCurrencyAsync()).ReturnsAsync(listCurrency);
            #endregion
            #region получение всех книг
            var listBook = SetListBookUi();
            mock.Setup(p => p.Books.FindBooksAsync(seachString, language, currency)).ReturnsAsync(listBook);
            #endregion
            HomeController controller = new HomeController(mock.Object);
            #endregion

            //ожидаемое значение
            var colSearch = 1;


            //результат выполнения
            var actual = controller.SearchBookAsync(seachString).Result as ViewResult;
            var znach = actual?.Model as BookCurrencyViewModel;
            //сверка значений
            Assert.AreEqual((znach?.BooksView as List<BookSite>)?.Count, colSearch);

        }

        [TestMethod()]
        public void PreviewAsyncTest()
        {
            #region инициализация 
            var mock = new Mock<BooksShopCore.IBooksShop>();
            var language = "rus";
            var currency = "byn";
            var idBook = 1;
            #region получение списка валют

            var listCurrency = SetCurrencyUi();
            mock.Setup(p => p.Currency.GetAllCurrencyAsync()).ReturnsAsync(listCurrency);
            #endregion
            #region получение всех книг
            var book = AddBook();
            mock.Setup(p => p.Books.GetBookAsync(idBook, language, currency)).ReturnsAsync(book);
            #endregion
            #region получение всех книг
            var preview = String.Empty;
            mock.Setup(p => p.Preview.GetPreviewAsync(idBook)).ReturnsAsync(preview);
            #endregion
            HomeController controller = new HomeController(mock.Object);
            #endregion

            //ожидаемое значение

            //результат выполнения
            var actual = controller.PreviewAsync(idBook).Result as ViewResult;
            //сверка значений
            Assert.IsNotNull(actual);
        }
    }
}