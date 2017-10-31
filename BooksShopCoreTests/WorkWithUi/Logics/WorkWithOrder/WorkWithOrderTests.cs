using Microsoft.VisualStudio.TestTools.UnitTesting;
using BooksShopCore.WorkWithUi.Logics.WorkWithOrder;
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

namespace BooksShopCore.WorkWithUi.Logics.WorkWithOrder.Tests
{
    [TestClass()]
    public class WorkWithOrderTests
    {
        private BuyerUi tempBuyerUi = new BuyerUi();
        BookUi book = new BookUi()
        {
            Author = "Рихтер",
            BookId = 1,
            Count = 1,
            Currency = "BUN",
            Name = "Clr via C#",
            Price = 123.43m,
            Year = new DateTime(2017, 1, 1)
        };

        [TestMethod()]
        public void BuyTest()
        {
            var workWithOrder = new WorkWithOrder(this.tempBuyerUi);

            //ожидаемое значение
            var expected = true;

            //результат
            var actual = workWithOrder.Buy(book);

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
                workWithOrder.PromocodeRepository = mock.Object;

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
    }
}