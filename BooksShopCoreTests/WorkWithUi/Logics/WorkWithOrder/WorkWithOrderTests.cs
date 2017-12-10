using Microsoft.VisualStudio.TestTools.UnitTesting;
using BooksShopCore.WorkWithUi.LogicsSite.WorkWithOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;
using Moq;

namespace BooksShopCore.WorkWithUi.LogicsSite.WorkWithOrder.Tests
{
    [TestClass()]
    public class WorkWithOrderTests
    {
        private BuyerUi tempBuyerUi = new BuyerUi();
        private static BookUi book = new BookUi();
        

        static WorkWithOrderTests()
        {
            var bookName = new BookNameUi()
            {
                LanguageBookCode = new LanguageUi()
                {
                    LanguageCode = "ENG",
                    LanguageName = "Английский"
                },
                Name = "CLR via C#"

            };

            book.Authors = new List<AuthorUi> {new AuthorUi() {Name="Рихтер"}};
            book.BookId = 1;
            book.Count = 1;
            book.ListName = new List<BookNameUi> {bookName};
            book.Year = new DateTime(2017, 1, 1);

        }

        [TestMethod()]
        public void BuyTest()
        {
            var workWithOrder = new WorkWithOrder(this.tempBuyerUi);

            //ожидаемое значение
            var expected = true;

            //результат
            var actual = workWithOrder.Buy(book,100,new CurrencyUi() {CurrencyCode = "BUN"});

            //сверка значений
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void ReversalTest()
        {
            //инициализация исходных данных
            var workWithOrder = new WorkWithOrder(this.tempBuyerUi);
            BuyTest();

            //ожидаемое значение
            var expected = true;

            //результат
            var actual = workWithOrder.Reversal(book);

            //сверка значений
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ApplyPromocodeTest()
        {
            try
            {
                //инициализация исходных данных
                var verifyPromoCode = "somecode";
                var mock = new Mock<IDataRepository<PromocodeData>>();
                mock.Setup(a => a.ReadAll()).Returns(
                    new List<PromocodeData>()
                    {
                        new PromocodeData()
                        {
                            Code = verifyPromoCode,
                            Date = DateTime.Now,
                            Id = 1,
                            Percent = 10
                        }
                    });

                var workWithOrder = new WorkWithOrder(this.tempBuyerUi);
                workWithOrder.promocodeRepository = mock.Object;

                BuyTest();

                //ожидаемое значение
                var expected = true;

                //результат
                var actual = workWithOrder.ApplyPromocode("somecode");

                //сверка значений
                Assert.AreEqual(expected, actual);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod()]
        public void ShowListCurrentPurchasesTest()
        {
            //инициализация исходных данных
            var workWithOrder = new WorkWithOrder(this.tempBuyerUi);
            BuyTest();

            //ожидаемое значение
            var expected = 1;

            //результат
            var actual = workWithOrder.ShowListCurrentPurchases();

            //сверка значений
            Assert.AreEqual(expected, actual.Count);
        }

        [TestMethod()]
        public void CancelPurchaseAfterConfirmationTest()
        {
            //инициализация исходных данных
            var workWithOrder = new WorkWithOrder(this.tempBuyerUi);
            BuyTest();

            //ожидаемое значение
            var expected = 1;

            //результат
            var actual = workWithOrder.ShowListCurrentPurchases();

            //сверка значений
            Assert.AreEqual(expected, actual.Count);
        }
    }
}