using Microsoft.VisualStudio.TestTools.UnitTesting;
using BooksShopSite.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using BooksShopCore.WorkWithUi.EntityUi;

namespace BooksShopSite.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void IndexAsyncTest()
        {
            var mock = new Mock<BooksShopCore.AbstractBooksShop>();
            #region получение списка валют
            mock.Setup(p => p.Currency.GetAllCurrencyAsync()).ReturnsAsync(new List<CurrencyUi>() {
                new CurrencyUi()
                {
                    CurrencyId =1,
                    CurrencyCode="BYN",
                    CurrencyName="Белорусский рубль"
                },
                new CurrencyUi()
                {
                    CurrencyId =2,
                    CurrencyCode="RUB",
                    CurrencyName="Российский рубль"
                },
                new CurrencyUi()
                {
                    CurrencyId =3,
                    CurrencyCode="USD",
                    CurrencyName="Доллар США"
                },
                new CurrencyUi()
                {
                    CurrencyId =4,
                    CurrencyCode="GBP",
                    CurrencyName="Фунт стерлингов"
                }
            });
            #endregion
            #region получение всех книг
            mock.Setup(p => p.Books.ShowAllBooksAsync("rus", "byn")).ReturnsAsync(new List<BookUi>()
            {
                new BookUi()
                {
                    BookId=1,
                    Authors=new List<AuthorUi>()
                    {
                        new AuthorUi()
                        {
                            AuthorId=1,
                            Name="Автор1",
                            Info="Информация по автору1",
                            Year="2014"
                        }
                    },
                    Count=10,
                    Format=new FormatBookUi()
                    {
                        FormatBookId=0,
                        FormatName="paper"
                    },
                    ListName=new List<BookNameUi>()
                    {
                        new BookNameUi()
                        {
                            BookNameId=1,
                            LanguageBookCode = new LanguageUi()
                            {
                                LanguageId=1,
                                LanguageCode="rus",
                                LanguageName="Русский"
                            },
                            Name="Название книги1"
                        },
                        new BookNameUi()
                        {
                            BookNameId=1,
                            LanguageBookCode = new LanguageUi()
                            {
                                LanguageId=1,
                                LanguageCode="eng",
                                LanguageName="Английский"
                            },
                            Name="Name book1"
                        }
                    },
                    ListPrice = new List<PriceUi>()
                    {
                        new PriceUi()
                        {
                            PriceId=1,
                            Country=new CountryUi()
                            {
                                CountryId=1,
                                CountryCode="BY",
                                CountryName="Беларусь"
                            },
                            Currency=new CurrencyUi()
                            {
                                CurrencyId =1,
                                CurrencyCode="BYN",
                                CurrencyName="Белорусский рубль"
                            },
                            Price= 123
                        }
                    },
                    Year=new DateTime(2016,01,01)
                }
            });
            #endregion
            HomeController controller = new HomeController(mock.Object);
            //ожидаемое значение


            //результат выполнения
            var actual = controller.IndexAsync().Result as ViewResult;

            //сверка значений
            Assert.IsNotNull(actual);

        }
    }
}