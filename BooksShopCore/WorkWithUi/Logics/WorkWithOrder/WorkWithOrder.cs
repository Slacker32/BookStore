using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using BooksShopCore.WorkWithUi.EntityUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BooksShopCore.WorkWithUi.Logics.WorkWithOrder
{
    public class WorkWithOrder : IWorkWithOrder
    {
        GenericRepository<PromocodeData> PromocodeRepository { get; set; }
        BuyerUi Buyer { get; set; }

        public WorkWithOrder(BuyerUi buyer)
        {
            this.Buyer = buyer;
        }

        public IList<BookUi> ApplyPromocode(string promocode)
        {
            var ret = new List<BookUi>();
            try
            {
                var listBook = PromocodeRepository.ReadAll();
                var d = listBook.Where((p) => p.Code.Contains(promocode)).OrderByDescending(p=>p.Date).First();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка поиска книги в хранилище данных:{ex}");
            }
            return ret;
        }

        public IList<BookUi> ShowListCurrentPurchases()
        {
            throw new NotImplementedException();
        }

        public bool CancelPurchase(int purchaseId)
        {
            throw new NotImplementedException();
        }

        public bool ConfirmationOrder()
        {
            throw new NotImplementedException();
        }

        public bool IsCompleted(int purchaseId)
        {
            throw new NotImplementedException();
        }

    }
}
